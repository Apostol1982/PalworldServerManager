﻿
namespace PalworldServerManager
{
    partial class Form_RCON
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_RCON));
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            richTextBox_output = new System.Windows.Forms.RichTextBox();
            tableLayoutPanel_sendSection = new System.Windows.Forms.TableLayoutPanel();
            textBox_commandText = new System.Windows.Forms.TextBox();
            button_sendCommand = new System.Windows.Forms.Button();
            panel_playersSection = new System.Windows.Forms.Panel();
            panel_playerListSection = new System.Windows.Forms.Panel();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            button_kick = new System.Windows.Forms.Button();
            button_ban = new System.Windows.Forms.Button();
            panel3 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            tableLayoutPanel_topSection = new System.Windows.Forms.TableLayoutPanel();
            textBox_ipRCON = new System.Windows.Forms.TextBox();
            textBox_portRCON = new System.Windows.Forms.TextBox();
            button_connectRCON = new System.Windows.Forms.Button();
            button_disconnectRCON = new System.Windows.Forms.Button();
            textBox_passwordRCON = new System.Windows.Forms.TextBox();
            button1 = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel_sendSection.SuspendLayout();
            panel_playersSection.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel_topSection.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(panel_playersSection);
            panel1.Controls.Add(tableLayoutPanel_topSection);
            resources.ApplyResources(panel1, "panel1");
            panel1.Name = "panel1";
            // 
            // panel2
            // 
            panel2.Controls.Add(richTextBox_output);
            panel2.Controls.Add(tableLayoutPanel_sendSection);
            resources.ApplyResources(panel2, "panel2");
            panel2.Name = "panel2";
            // 
            // richTextBox_output
            // 
            resources.ApplyResources(richTextBox_output, "richTextBox_output");
            richTextBox_output.Name = "richTextBox_output";
            richTextBox_output.ReadOnly = true;
            // 
            // tableLayoutPanel_sendSection
            // 
            resources.ApplyResources(tableLayoutPanel_sendSection, "tableLayoutPanel_sendSection");
            tableLayoutPanel_sendSection.Controls.Add(textBox_commandText, 0, 0);
            tableLayoutPanel_sendSection.Controls.Add(button_sendCommand, 1, 0);
            tableLayoutPanel_sendSection.Name = "tableLayoutPanel_sendSection";
            // 
            // textBox_commandText
            // 
            textBox_commandText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(textBox_commandText, "textBox_commandText");
            textBox_commandText.Name = "textBox_commandText";
            // 
            // button_sendCommand
            // 
            resources.ApplyResources(button_sendCommand, "button_sendCommand");
            button_sendCommand.Name = "button_sendCommand";
            button_sendCommand.UseVisualStyleBackColor = true;
            button_sendCommand.Click += button_sendCommand_Click;
            // 
            // panel_playersSection
            // 
            panel_playersSection.Controls.Add(panel_playerListSection);
            panel_playersSection.Controls.Add(tableLayoutPanel1);
            panel_playersSection.Controls.Add(panel3);
            resources.ApplyResources(panel_playersSection, "panel_playersSection");
            panel_playersSection.Name = "panel_playersSection";
            // 
            // panel_playerListSection
            // 
            resources.ApplyResources(panel_playerListSection, "panel_playerListSection");
            panel_playerListSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel_playerListSection.Name = "panel_playerListSection";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(tableLayoutPanel1, "tableLayoutPanel1");
            tableLayoutPanel1.Controls.Add(button_kick, 0, 0);
            tableLayoutPanel1.Controls.Add(button_ban, 1, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // button_kick
            // 
            resources.ApplyResources(button_kick, "button_kick");
            button_kick.Name = "button_kick";
            button_kick.UseVisualStyleBackColor = true;
            button_kick.Click += button_kick_Click;
            // 
            // button_ban
            // 
            resources.ApplyResources(button_ban, "button_ban");
            button_ban.Name = "button_ban";
            button_ban.UseVisualStyleBackColor = true;
            button_ban.Click += button_ban_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(label1);
            resources.ApplyResources(panel3, "panel3");
            panel3.Name = "panel3";
            // 
            // label1
            // 
            label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // tableLayoutPanel_topSection
            // 
            resources.ApplyResources(tableLayoutPanel_topSection, "tableLayoutPanel_topSection");
            tableLayoutPanel_topSection.Controls.Add(textBox_ipRCON, 0, 1);
            tableLayoutPanel_topSection.Controls.Add(textBox_portRCON, 1, 1);
            tableLayoutPanel_topSection.Controls.Add(button_connectRCON, 0, 0);
            tableLayoutPanel_topSection.Controls.Add(button_disconnectRCON, 1, 0);
            tableLayoutPanel_topSection.Controls.Add(textBox_passwordRCON, 1, 2);
            tableLayoutPanel_topSection.Controls.Add(button1, 0, 2);
            tableLayoutPanel_topSection.Name = "tableLayoutPanel_topSection";
            // 
            // textBox_ipRCON
            // 
            resources.ApplyResources(textBox_ipRCON, "textBox_ipRCON");
            textBox_ipRCON.Name = "textBox_ipRCON";
            // 
            // textBox_portRCON
            // 
            resources.ApplyResources(textBox_portRCON, "textBox_portRCON");
            textBox_portRCON.Name = "textBox_portRCON";
            // 
            // button_connectRCON
            // 
            resources.ApplyResources(button_connectRCON, "button_connectRCON");
            button_connectRCON.Name = "button_connectRCON";
            button_connectRCON.UseVisualStyleBackColor = true;
            button_connectRCON.Click += button_connectRCON_Click;
            // 
            // button_disconnectRCON
            // 
            resources.ApplyResources(button_disconnectRCON, "button_disconnectRCON");
            button_disconnectRCON.Name = "button_disconnectRCON";
            button_disconnectRCON.UseVisualStyleBackColor = true;
            button_disconnectRCON.Click += button_disconnectRCON_Click;
            // 
            // textBox_passwordRCON
            // 
            resources.ApplyResources(textBox_passwordRCON, "textBox_passwordRCON");
            textBox_passwordRCON.Name = "textBox_passwordRCON";
            // 
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form_RCON
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "Form_RCON";
            Load += Form_RCON_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanel_sendSection.ResumeLayout(false);
            tableLayoutPanel_sendSection.PerformLayout();
            panel_playersSection.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tableLayoutPanel_topSection.ResumeLayout(false);
            tableLayoutPanel_topSection.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox_output;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_sendSection;
        private System.Windows.Forms.TextBox textBox_commandText;
        private System.Windows.Forms.Button button_sendCommand;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_topSection;
        private System.Windows.Forms.TextBox textBox_ipRCON;
        private System.Windows.Forms.TextBox textBox_portRCON;
        private System.Windows.Forms.Button button_connectRCON;
        private System.Windows.Forms.Button button_disconnectRCON;
        private System.Windows.Forms.TextBox textBox_passwordRCON;
        private System.Windows.Forms.Panel panel_playersSection;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel_playerListSection;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_kick;
        private System.Windows.Forms.Button button_ban;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}