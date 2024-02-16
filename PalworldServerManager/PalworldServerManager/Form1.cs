﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalworldServerManager
{
    public partial class Form1 : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // WS_EX_COMPOSITED
                return cp;
            }
        }


        public string publicIP;
        public string localIP;

        // To download Palworld Server using steamcmd
        // steamcmd +login anonymous +app_update 2394010 validate +quit
        //

        private string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private string isCommunityserver;
        private string isUseperfthreads;
        private string isNoAsyncLoadingThread;
        private string isUseMultithreadForDS;
        private string isLog;
        private string isNoSteam;
        Form_ServerSettings serverSettingsForm;
        Form_RCON rconForm;
        Form_ServerRestart serverRestartForm;
        public bool isServerStarted = false;
        private bool isOnCrashRestart = false;

        public Form1()
        {
            LoadLanguage();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OnLoad();
        }

        private void LoadLanguage()
        {
            string selectedLanguage = Properties.Settings.Default.Seleceted_Language;

            if (!string.IsNullOrEmpty(selectedLanguage))
            {
                try
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(selectedLanguage);
                }
                catch (CultureNotFoundException ex)
                {
                    //Default Language if somehow the code fails to get the language in a readable state
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                }
            }
            else
            {
                //Default Language
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            }
        }

        private void OnLoad()
        {
            //Load Mainform values
            //publicIP = GetPublicIpAddress();
            //textBox1.Text = publicIP;
            //localIP = GetLocalAddress();
            //textBox2.Text = localIP;
            ReadStartServerArg();
            //Load Form
            rconForm = new Form_RCON();
            serverRestartForm = new Form_ServerRestart(this);
            serverSettingsForm = new Form_ServerSettings(this);
            LoadForm(rconForm, false);
            LoadForm(serverSettingsForm, true);
            LoadForm(serverRestartForm, true);

            timer_checkServerCrash.Interval = 1000;
            checkBox_onCMDCrashRestart.Checked = Properties.Settings.Default.Saved_OnCMDCrashRestart;
            checkBox_onCMDCrashRestart.CheckedChanged += OnServerCMDCrashRestart_OnChange;


        }

        private void LoadForm(Form formToLoad, bool isShow)
        {

            if (formToLoad != null)
            {
                formToLoad.TopLevel = false;
                panel_chilForm.Controls.Add(formToLoad);
                formToLoad.FormBorderStyle = FormBorderStyle.None;
                formToLoad.Dock = DockStyle.Fill;
                if (isShow)
                {
                    formToLoad.Show();
                }
                else
                {
                    formToLoad.Hide();
                }

            }
        }

        private void ShowForm(Form formToShow)
        {
            foreach (Control control in panel_chilForm.Controls)
            {
                if (control is Form form)
                {
                    //hide the form
                    form.Hide();
                }
            }

            if (formToShow != null)
            {
                formToShow.Show();
            }
        }

        //MAIN FORM SECTION
        private string GetPublicIpAddress()
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    // Make a request to api.ipify.org to get the public IP address
                    string response = webClient.DownloadString("https://api.ipify.org?format=json");

                    // Parse the JSON response to extract the public IP address
                    int startIndex = response.IndexOf("\"ip\":\"") + 6;
                    int endIndex = response.IndexOf("\"", startIndex);
                    string publicIpAddress = response.Substring(startIndex, endIndex - startIndex);
                    Debug.WriteLine(publicIpAddress);
                    return publicIpAddress;
                }
            }
            catch (Exception ex)
            {
                return "Error getting public IP address: " + ex.Message;
            }
        }

        private void OnServerCMDCrashRestart_OnChange (object sender, EventArgs e)
        {

            Properties.Settings.Default.Saved_OnCMDCrashRestart = checkBox_onCMDCrashRestart.Checked;
            Properties.Settings.Default.Save();
        }

        private string GetLocalAddress()
        {
            try
            {
                string localIpAddress = "";
                IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress address in ip)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIpAddress = address.ToString();
                        Debug.WriteLine(localIpAddress);
                        return localIpAddress;
                    }
                }
                return localIpAddress;
            }
            catch (Exception ex)
            {
                return "Error getting local IP address: " + ex.Message;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            publicIP = GetPublicIpAddress();
            textBox1.Text = publicIP;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            localIP = GetLocalAddress();
            textBox2.Text = localIP;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string zipUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
            string fileName = "steamcmd.zip";
            string savePath = Path.Combine(baseDirectory, fileName);
            string extractPath = baseDirectory;

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(zipUrl, savePath);

                    // Unzip the downloaded file
                    ZipFile.ExtractToDirectory(savePath, extractPath);

                    serverSettingsForm.SendMessageToConsole("Download and extraction of steamcmd completed!");
                }
                catch (Exception ex)
                {
                    serverSettingsForm.SendMessageToConsole($"Download steamcmd catched error: {ex.Message}");
                }
                finally
                {
                    // delete the downloaded .zip file after extraction
                    File.Delete(savePath);
                }
            }
        }

        private void RunDownloadServerBatchFile()
        {
            // Get the executable directory

            // Specify the path for the bat file
            string batFilePath = Path.Combine(baseDirectory, "DownloadUpdateVerifyServer.bat");
            try
            {
                //Run bat file
                try
                {
                    // Create a new process to run the batch file
                    Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = batFilePath,
                            UseShellExecute = true,
                            CreateNoWindow = false
                        }
                    };

                    // Start the process
                    process.Start();
                    process.WaitForExit();
                    MessageBox.Show("Finished Download/Verify/Update Server");

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"RunDownloadServerBatFile catched error: {ex.Message}");
                }

                //MessageBox.Show("Batch file generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RunDownloadServerBatFile catched error: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!CheckSteamCMD())
            {
                return;
            }

            bool isFirstTime = false;

            if (!CheckPalServer())
            {
                isFirstTime = true;
            }


            if (!isFirstTime)
            {
                DialogResult askBeforeDownloadUpdateVerify = MessageBox.Show("Please create a manual backup before proceeding \npress Yes to continue, press no to cancel the download/update/verify process", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (askBeforeDownloadUpdateVerify == DialogResult.Yes)
                {
                    //What to do
                }
                else
                {
                    return;
                }
            }



            // Get the content from the TextBox
            string batContent = "steamcmd +login anonymous +app_update 2394010 validate +quit";

            // Specify the path for the bat file
            string batFilePath = Path.Combine(baseDirectory, "DownloadUpdateVerifyServer.bat");

            try
            {
                // Write the content to the bat file
                File.WriteAllText(batFilePath, batContent);

                // Start a new thread to run the batch file asynchronously
                Thread thread = new Thread(new ThreadStart(RunDownloadServerBatchFile));
                thread.Start();
                serverSettingsForm.SendMessageToConsole("Started Download/Verify/Update Server");
            }
            catch (Exception ex)
            {
                serverSettingsForm.SendMessageToConsole($"Download/Update/Verify button catched error: {ex.Message}");
            }
        }

        private void RunStartServerBatchFile()
        {

            // Specify the path for the bat file
            string batFilePath = Path.Combine(baseDirectory, "RunServer.bat");
            try
            {
                // Create a new process to run the batch file
                Process serverProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = batFilePath,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };

                // Start the process
                serverProcess.Start();
                // Store the reference to the child process
                serverProcess.WaitForExit();



            }
            catch (Exception ex)
            {
                MessageBox.Show($"Run server start bat catched error: {ex.Message}");
            }
        }

        private bool CheckSteamCMD()
        {
            if (File.Exists("steamcmd.exe"))
            {
                //MessageBox.Show("Steamcmd is there");
                return true;
            }
            else
            {
                serverSettingsForm.SendMessageToConsole("Missing steamcmd.exe \nPress download steamcmd");
                return false;
            }
        }

        private bool CheckPalServer()
        {
            // Combine the executable directory with the relative path to the INI file
            string palServerEXEPath = Path.Combine(baseDirectory, "steamapps", "common", "PalServer", "PalServer.exe");
            if (File.Exists(palServerEXEPath))
            {
                //MessageBox.Show("palServerEXEPath is there");
                return true;
            }
            else
            {
                serverSettingsForm.SendMessageToConsole($"Missing PalServer.exe \nPress the download/update/verify server button to validate your files.");
                return false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        public void StartServer()
        {
            if (!CheckSteamCMD())
            {
                return;
            }

            if (!CheckPalServer())
            {
                return;
            }

            

            if (!isServerStarted)
            {
                isCommunityserver = checkBox_communityServer.Checked ? " EpicApp=PalServer" : "";
                isUseperfthreads = checkBox_useperfthreads.Checked ? " -useperfthreads" : "";
                isNoAsyncLoadingThread = checkBox_noAsyncLoadingThread.Checked ? " -NoAsyncLoadingThread" : "";
                isUseMultithreadForDS = checkBox_useMultithreadForDS.Checked ? " -UseMultithreadForDS" : "";
                isLog = checkBox_log.Checked ? " -log" : "";
                isNoSteam = checkBox_noSteam.Checked ? " -nosteam" : "";
                WriteStartServerArg();
                // Get the content from the TextBox
                string batContent = $"cd .\\steamapps\\common\\PalServer\nPalServer.exe{isCommunityserver}{isUseperfthreads}{isNoAsyncLoadingThread}{isUseMultithreadForDS}{isLog}{isNoSteam}";
                // Specify the path for the bat file
                string batFilePath = Path.Combine(baseDirectory, "RunServer.bat");
                try
                {
                    serverSettingsForm.SendMessageToConsole("Server Started");
                    // Write the content to the bat file
                    File.WriteAllText(batFilePath, batContent);

                    // Start a new thread to run the batch file asynchronously
                    Thread thread = new Thread(new ThreadStart(RunStartServerBatchFile));
                    thread.Start();
                    serverSettingsForm.SaveGameTimer_Start();
                    serverSettingsForm.AutoRestartServerTimer_Start();
                    if (checkBox_onCMDCrashRestart.Checked == true)
                    {
                        timer_checkServerCrash.Start();
                    }
                    isServerStarted = true;
                    button_startServer.Enabled = false;
                    button_stopServer.Enabled = true;

                }
                catch (Exception ex)
                {
                    serverSettingsForm.SendMessageToConsole($"Server start catched error: {ex.Message}");
                }
            }
        }

        private void button_stopServer_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        public void StopServer()
        {
            if (isServerStarted)
            {

                // Specify the name of the process without the .exe extension
                string processName = "PalServer-Win64-Test-Cmd";

                // Find and kill the process if it's running
                Process[] processes = Process.GetProcessesByName(processName);
                foreach (Process process in processes)
                {
                    process.Kill();
                }
                isServerStarted = false;
                button_startServer.Enabled = true;
                button_stopServer.Enabled = false;
                serverSettingsForm.SaveGameTimer_Stop();
                serverSettingsForm.AutoRestartServerTimer_Stop();
                timer_checkServerCrash.Stop();
                serverSettingsForm.SendMessageToConsole("Server Stopped");

            }
        }


        private void WriteStartServerArg()
        {
            using (StreamWriter sw = new StreamWriter("serverstartuppreset.txt"))
            {
                sw.WriteLine((int)checkBox_communityServer.CheckState);
                sw.WriteLine((int)checkBox_useperfthreads.CheckState);
                sw.WriteLine((int)checkBox_noAsyncLoadingThread.CheckState);
                sw.WriteLine((int)checkBox_useMultithreadForDS.CheckState);
                sw.WriteLine((int)checkBox_log.CheckState);
                sw.WriteLine((int)checkBox_noSteam.CheckState);
            }
        }

        private void ReadStartServerArg()
        {
            // Read values from the text file and set them to TextBoxes on form load
            if (File.Exists("serverstartuppreset.txt"))
            {
                using (StreamReader sr = new StreamReader("serverstartuppreset.txt"))
                {
                    checkBox_communityServer.CheckState = ParseCheckState(sr.ReadLine());
                    checkBox_useperfthreads.CheckState = ParseCheckState(sr.ReadLine());
                    checkBox_noAsyncLoadingThread.CheckState = ParseCheckState(sr.ReadLine());
                    checkBox_useMultithreadForDS.CheckState = ParseCheckState(sr.ReadLine());
                    checkBox_log.CheckState = ParseCheckState(sr.ReadLine());
                    checkBox_noSteam.CheckState = ParseCheckState(sr.ReadLine());
                }
            }
        }

        private CheckState ParseCheckState(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                // Handle null or empty string, assuming Unchecked in this case
                return CheckState.Unchecked;
            }

            return (CheckState)Enum.Parse(typeof(CheckState), value);
        }

        //TOOLSTRIPMENU SECTION
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en-GB", "English"); //culture code, languagename(to let myself know what language it is)
            Application.Restart();
        }

        private void chineseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("zh-Hans", "Chinese");
            Application.Restart();
        }

        private void ChangeLanguage(string cultureCode, string languageName)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
            Properties.Settings.Default.Seleceted_Language = cultureCode;
            Properties.Settings.Default.Save();
        }

        private void rCONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(rconForm);
        }

        private void serverSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void baseDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDirectoryGiven(baseDirectory);
        }

        private void instructionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string title = "Instruction";
            string message = "1)Click Download Steam CMD\n" +
                "2)Click Download/Update/Verify Server\n" +
                "3)Start the server\n" +
                "4)Let the server loadup to generate more server files then shutdown the server\n" +
                "5)Edit any server world settings/startup settings you want\n" +
                "6)Save it\n" +
                "7)Done. LocalHostIP is 127.0.0.1\n\n" +
                "8)If you want others to join, make sure you have portforwarded your ports and added your ports to inbounrd and outbound firewall.\nCommon ports used in palworld: \n8211 ((UDP)Game Server port), \n27015 ((TCP)Steam Port), \n25575 (RCON Port).\n\nBest if you could portforward these with both UDP/TCP.";

            MessageBox.Show(message, title);
        }

        private void githubToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string githubUrl = @"https://github.com/Tianyu-00";

            OpenURLGiven(githubUrl);
        }

        private void repoPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string githubRepoUrl = "https://github.com/TianYu-00/PalworldServerManager";
            OpenURLGiven(githubRepoUrl);

        }

        private void OpenURLGiven(string URL)
        {
            try
            {
                //Process.Start(githubRepoUrl); //Wont work on .net core
                Process.Start(new ProcessStartInfo { FileName = URL, UseShellExecute = true }); //turns useshellexecute on which is defaulted to off after vs update.
                //System.Diagnostics.Process.Start("explorer.exe", URL); //Works
            }
            catch (Exception ex)
            {
                serverSettingsForm.SendMessageToConsole($"Repo page open catched error: {ex.Message}");
            }
        }

        private void OpenFileDirectoryGiven(string directory)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = directory, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                serverSettingsForm.SendMessageToConsole($"Open file directory given catched error: {ex.Message}");
            }
        }

        private void serverSettingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowForm(serverSettingsForm);
        }

        private void serverRestartScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(serverRestartForm);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer();
        }



        private void OnCrashRestart()
        {
            //PalServer-Win64-Test-Cmd.exe

            // ProcessName
            string processName = "PalServer-Win64-Test-Cmd";
            Process palServerProcess = null;

            //Find the process
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                //process.Kill();
                // Process Found
                palServerProcess = process;
            }

            if (isServerStarted && palServerProcess == null)
            {
                serverSettingsForm.SendMessageToConsole($"Detected server is started but process is not found, attempting to restart server...");
                serverSettingsForm.SendMessageToConsole($"Use 'Stop Server' Button instead if you want to shutdown your server.");
                StopServer();
                StartServer();
                
            }
            else 
            {
                // Dont do anything
            }

        }

        private void timer_checkServerCrash_Tick(object sender, EventArgs e)
        {
            OnCrashRestart();
        }
    }
}