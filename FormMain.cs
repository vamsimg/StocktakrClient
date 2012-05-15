using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

using System.Web.Script.Serialization;
using StocktakrClient.com.stocktakr;

namespace StocktakrClient
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FormMain : System.Windows.Forms.Form
	{


        private System.Windows.Forms.TextBox lbLog; 
	   private NotifyIcon TraynotifyIcon;
	   private OpenFileDialog RMOpenFileDialog;
	   private OpenFileDialog DPOpenFileDialog;
	   private TabPage DatabasesTab;
	   private Button SaveDatabaseSettingsButton;
	   private Button FindDPDBButton;
	   private TextBox DPDBTextBox;
	   private Label STLocationLabel;
	   private TabPage ConnectionTab;
	   private Label ConnectionErrorlabel;
	   private TextBox StoreIDtextBox;
	   private TextBox PasswordTextBox;
	   private Label TestConnectionErrorlabel;
	   private Button SaveConnectionSettingsButton;
	   private Label Storelabel;
	   private Label Passwordlabel;
	   private Button TestConnectionButton;
	   private TabControl MainTabControl;
	   private TabPage SyncTab;
	   private GroupBox POSSoftwareGroupBox;
	   private RadioButton MicrosoftRMSRadioButton;
	   private RadioButton MYOBRadioButton;
	   private SplitContainer POSSoftwareSplitContainer;
	   private Label label2;
	   private Button FindRMDBbutton;
	   private Label label1;
	   private TextBox RMDBTextBox;
	   private TextBox MicrosoftDBTextBox;
	   private Label label7;
	   private Label TestMicrosoftConnectionErrorLabel;
	   private Button MicrosoftTestConnectionButton;
	   private Label label6;
	   private TextBox MicrosoftPasswordTextBox;
	   private Label label5;
	   private TextBox MicrosoftUserTextBox;
	   private Label label4;
	   private TextBox MicrosoftLocationTextBox;
	   private Label label3;
	   private Label DatabaseSettingsErrorLabel;
	   private Panel panel2;
	   private GroupBox SendOptionsGroupBox;
	   private Button UpdateItemsButton;
	   private Button ReplaceItemsButton;
	   private Label SendItemsErrorLabel;
	   private Label CommitItemsErrorLabel;
	   private Button CommitStocktakeButton;
	   private Label GetStocktakeTransactionsErrorLabel;
	   private Button GetItemsButton;
	   private Label label9;
        private IContainer components;

		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.lbLog = new System.Windows.Forms.TextBox();
			this.TraynotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.RMOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.DPOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.DatabasesTab = new System.Windows.Forms.TabPage();
			this.DatabaseSettingsErrorLabel = new System.Windows.Forms.Label();
			this.POSSoftwareSplitContainer = new System.Windows.Forms.SplitContainer();
			this.label2 = new System.Windows.Forms.Label();
			this.FindRMDBbutton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.RMDBTextBox = new System.Windows.Forms.TextBox();
			this.MicrosoftDBTextBox = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.TestMicrosoftConnectionErrorLabel = new System.Windows.Forms.Label();
			this.MicrosoftTestConnectionButton = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.MicrosoftPasswordTextBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.MicrosoftUserTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.MicrosoftLocationTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.POSSoftwareGroupBox = new System.Windows.Forms.GroupBox();
			this.MicrosoftRMSRadioButton = new System.Windows.Forms.RadioButton();
			this.MYOBRadioButton = new System.Windows.Forms.RadioButton();
			this.SaveDatabaseSettingsButton = new System.Windows.Forms.Button();
			this.FindDPDBButton = new System.Windows.Forms.Button();
			this.DPDBTextBox = new System.Windows.Forms.TextBox();
			this.STLocationLabel = new System.Windows.Forms.Label();
			this.ConnectionTab = new System.Windows.Forms.TabPage();
			this.ConnectionErrorlabel = new System.Windows.Forms.Label();
			this.StoreIDtextBox = new System.Windows.Forms.TextBox();
			this.PasswordTextBox = new System.Windows.Forms.TextBox();
			this.TestConnectionErrorlabel = new System.Windows.Forms.Label();
			this.SaveConnectionSettingsButton = new System.Windows.Forms.Button();
			this.Storelabel = new System.Windows.Forms.Label();
			this.Passwordlabel = new System.Windows.Forms.Label();
			this.TestConnectionButton = new System.Windows.Forms.Button();
			this.MainTabControl = new System.Windows.Forms.TabControl();
			this.SyncTab = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.SendOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.UpdateItemsButton = new System.Windows.Forms.Button();
			this.ReplaceItemsButton = new System.Windows.Forms.Button();
			this.SendItemsErrorLabel = new System.Windows.Forms.Label();
			this.CommitItemsErrorLabel = new System.Windows.Forms.Label();
			this.CommitStocktakeButton = new System.Windows.Forms.Button();
			this.GetStocktakeTransactionsErrorLabel = new System.Windows.Forms.Label();
			this.GetItemsButton = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.DatabasesTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.POSSoftwareSplitContainer)).BeginInit();
			this.POSSoftwareSplitContainer.Panel1.SuspendLayout();
			this.POSSoftwareSplitContainer.Panel2.SuspendLayout();
			this.POSSoftwareSplitContainer.SuspendLayout();
			this.POSSoftwareGroupBox.SuspendLayout();
			this.ConnectionTab.SuspendLayout();
			this.MainTabControl.SuspendLayout();
			this.SyncTab.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SendOptionsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbLog
			// 
			this.lbLog.Location = new System.Drawing.Point(593, 26);
			this.lbLog.Multiline = true;
			this.lbLog.Name = "lbLog";
			this.lbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.lbLog.Size = new System.Drawing.Size(519, 550);
			this.lbLog.TabIndex = 22;
			// 
			// DatabasesTab
			// 
			this.DatabasesTab.Controls.Add(this.DatabaseSettingsErrorLabel);
			this.DatabasesTab.Controls.Add(this.POSSoftwareSplitContainer);
			this.DatabasesTab.Controls.Add(this.POSSoftwareGroupBox);
			this.DatabasesTab.Controls.Add(this.SaveDatabaseSettingsButton);
			this.DatabasesTab.Controls.Add(this.FindDPDBButton);
			this.DatabasesTab.Controls.Add(this.DPDBTextBox);
			this.DatabasesTab.Controls.Add(this.STLocationLabel);
			this.DatabasesTab.Location = new System.Drawing.Point(4, 22);
			this.DatabasesTab.Name = "DatabasesTab";
			this.DatabasesTab.Padding = new System.Windows.Forms.Padding(3);
			this.DatabasesTab.Size = new System.Drawing.Size(538, 524);
			this.DatabasesTab.TabIndex = 2;
			this.DatabasesTab.Text = "Databases";
			this.DatabasesTab.UseVisualStyleBackColor = true;
			// 
			// DatabaseSettingsErrorLabel
			// 
			this.DatabaseSettingsErrorLabel.AutoSize = true;
			this.DatabaseSettingsErrorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DatabaseSettingsErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.DatabaseSettingsErrorLabel.Location = new System.Drawing.Point(125, 401);
			this.DatabaseSettingsErrorLabel.Name = "DatabaseSettingsErrorLabel";
			this.DatabaseSettingsErrorLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.DatabaseSettingsErrorLabel.Size = new System.Drawing.Size(12, 15);
			this.DatabaseSettingsErrorLabel.TabIndex = 49;
			// 
			// POSSoftwareSplitContainer
			// 
			this.POSSoftwareSplitContainer.Location = new System.Drawing.Point(9, 135);
			this.POSSoftwareSplitContainer.Name = "POSSoftwareSplitContainer";
			this.POSSoftwareSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// POSSoftwareSplitContainer.Panel1
			// 
			this.POSSoftwareSplitContainer.Panel1.Controls.Add(this.label2);
			this.POSSoftwareSplitContainer.Panel1.Controls.Add(this.FindRMDBbutton);
			this.POSSoftwareSplitContainer.Panel1.Controls.Add(this.label1);
			this.POSSoftwareSplitContainer.Panel1.Controls.Add(this.RMDBTextBox);
			// 
			// POSSoftwareSplitContainer.Panel2
			// 
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.MicrosoftDBTextBox);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.label7);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.TestMicrosoftConnectionErrorLabel);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.MicrosoftTestConnectionButton);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.label6);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.MicrosoftPasswordTextBox);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.label5);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.MicrosoftUserTextBox);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.label4);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.MicrosoftLocationTextBox);
			this.POSSoftwareSplitContainer.Panel2.Controls.Add(this.label3);
			this.POSSoftwareSplitContainer.Size = new System.Drawing.Size(526, 228);
			this.POSSoftwareSplitContainer.SplitterDistance = 77;
			this.POSSoftwareSplitContainer.TabIndex = 48;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(13, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(132, 13);
			this.label2.TabIndex = 34;
			this.label2.Text = "MYOB Retail Manager";
			// 
			// FindRMDBbutton
			// 
			this.FindRMDBbutton.Location = new System.Drawing.Point(438, 35);
			this.FindRMDBbutton.Name = "FindRMDBbutton";
			this.FindRMDBbutton.Size = new System.Drawing.Size(75, 23);
			this.FindRMDBbutton.TabIndex = 33;
			this.FindRMDBbutton.Text = "Find RMDB";
			this.FindRMDBbutton.UseVisualStyleBackColor = true;
			this.FindRMDBbutton.Click += new System.EventHandler(this.FindRMDBbutton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 13);
			this.label1.TabIndex = 29;
			this.label1.Text = "RM DB Location";
			// 
			// RMDBTextBox
			// 
			this.RMDBTextBox.Location = new System.Drawing.Point(131, 35);
			this.RMDBTextBox.Name = "RMDBTextBox";
			this.RMDBTextBox.Size = new System.Drawing.Size(268, 20);
			this.RMDBTextBox.TabIndex = 30;
			// 
			// MicrosoftDBTextBox
			// 
			this.MicrosoftDBTextBox.Location = new System.Drawing.Point(131, 63);
			this.MicrosoftDBTextBox.Name = "MicrosoftDBTextBox";
			this.MicrosoftDBTextBox.Size = new System.Drawing.Size(127, 20);
			this.MicrosoftDBTextBox.TabIndex = 45;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(13, 67);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(84, 13);
			this.label7.TabIndex = 44;
			this.label7.Text = "Database Name";
			// 
			// TestMicrosoftConnectionErrorLabel
			// 
			this.TestMicrosoftConnectionErrorLabel.AutoSize = true;
			this.TestMicrosoftConnectionErrorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TestMicrosoftConnectionErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.TestMicrosoftConnectionErrorLabel.Location = new System.Drawing.Point(131, 122);
			this.TestMicrosoftConnectionErrorLabel.Name = "TestMicrosoftConnectionErrorLabel";
			this.TestMicrosoftConnectionErrorLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.TestMicrosoftConnectionErrorLabel.Size = new System.Drawing.Size(12, 15);
			this.TestMicrosoftConnectionErrorLabel.TabIndex = 43;
			// 
			// MicrosoftTestConnectionButton
			// 
			this.MicrosoftTestConnectionButton.Location = new System.Drawing.Point(16, 117);
			this.MicrosoftTestConnectionButton.Name = "MicrosoftTestConnectionButton";
			this.MicrosoftTestConnectionButton.Size = new System.Drawing.Size(97, 23);
			this.MicrosoftTestConnectionButton.TabIndex = 42;
			this.MicrosoftTestConnectionButton.Text = "Test Connection";
			this.MicrosoftTestConnectionButton.UseVisualStyleBackColor = true;
			this.MicrosoftTestConnectionButton.Click += new System.EventHandler(this.MicrosoftTestConnectionButton_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(278, 67);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 13);
			this.label6.TabIndex = 40;
			this.label6.Text = "Password";
			// 
			// MicrosoftPasswordTextBox
			// 
			this.MicrosoftPasswordTextBox.Location = new System.Drawing.Point(356, 64);
			this.MicrosoftPasswordTextBox.Name = "MicrosoftPasswordTextBox";
			this.MicrosoftPasswordTextBox.PasswordChar = '*';
			this.MicrosoftPasswordTextBox.Size = new System.Drawing.Size(157, 20);
			this.MicrosoftPasswordTextBox.TabIndex = 41;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(276, 34);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(55, 13);
			this.label5.TabIndex = 38;
			this.label5.Text = "Username";
			// 
			// MicrosoftUserTextBox
			// 
			this.MicrosoftUserTextBox.Location = new System.Drawing.Point(356, 34);
			this.MicrosoftUserTextBox.Name = "MicrosoftUserTextBox";
			this.MicrosoftUserTextBox.Size = new System.Drawing.Size(157, 20);
			this.MicrosoftUserTextBox.TabIndex = 39;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 37);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(69, 13);
			this.label4.TabIndex = 36;
			this.label4.Text = "Server Name";
			// 
			// MicrosoftLocationTextBox
			// 
			this.MicrosoftLocationTextBox.Location = new System.Drawing.Point(131, 34);
			this.MicrosoftLocationTextBox.Name = "MicrosoftLocationTextBox";
			this.MicrosoftLocationTextBox.Size = new System.Drawing.Size(127, 20);
			this.MicrosoftLocationTextBox.TabIndex = 37;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(11, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 13);
			this.label3.TabIndex = 35;
			this.label3.Text = "Microsoft RMS";
			// 
			// POSSoftwareGroupBox
			// 
			this.POSSoftwareGroupBox.Controls.Add(this.MicrosoftRMSRadioButton);
			this.POSSoftwareGroupBox.Controls.Add(this.MYOBRadioButton);
			this.POSSoftwareGroupBox.Location = new System.Drawing.Point(140, 45);
			this.POSSoftwareGroupBox.Name = "POSSoftwareGroupBox";
			this.POSSoftwareGroupBox.Size = new System.Drawing.Size(285, 62);
			this.POSSoftwareGroupBox.TabIndex = 47;
			this.POSSoftwareGroupBox.TabStop = false;
			this.POSSoftwareGroupBox.Text = "POS Software";
			// 
			// MicrosoftRMSRadioButton
			// 
			this.MicrosoftRMSRadioButton.AutoSize = true;
			this.MicrosoftRMSRadioButton.Location = new System.Drawing.Point(30, 42);
			this.MicrosoftRMSRadioButton.Name = "MicrosoftRMSRadioButton";
			this.MicrosoftRMSRadioButton.Size = new System.Drawing.Size(95, 17);
			this.MicrosoftRMSRadioButton.TabIndex = 1;
			this.MicrosoftRMSRadioButton.TabStop = true;
			this.MicrosoftRMSRadioButton.Text = "Microsoft RMS";
			this.MicrosoftRMSRadioButton.UseVisualStyleBackColor = true;
			this.MicrosoftRMSRadioButton.CheckedChanged += new System.EventHandler(this.MicrosoftRMSRadioButton_CheckedChanged);
			// 
			// MYOBRadioButton
			// 
			this.MYOBRadioButton.AutoSize = true;
			this.MYOBRadioButton.Location = new System.Drawing.Point(30, 19);
			this.MYOBRadioButton.Name = "MYOBRadioButton";
			this.MYOBRadioButton.Size = new System.Drawing.Size(131, 17);
			this.MYOBRadioButton.TabIndex = 0;
			this.MYOBRadioButton.TabStop = true;
			this.MYOBRadioButton.Text = "MYOB Retail Manager";
			this.MYOBRadioButton.UseVisualStyleBackColor = true;
			this.MYOBRadioButton.CheckedChanged += new System.EventHandler(this.MYOBRadioButton_CheckedChanged);
			// 
			// SaveDatabaseSettingsButton
			// 
			this.SaveDatabaseSettingsButton.Location = new System.Drawing.Point(25, 396);
			this.SaveDatabaseSettingsButton.Name = "SaveDatabaseSettingsButton";
			this.SaveDatabaseSettingsButton.Size = new System.Drawing.Size(83, 23);
			this.SaveDatabaseSettingsButton.TabIndex = 35;
			this.SaveDatabaseSettingsButton.Text = "Save Settings";
			this.SaveDatabaseSettingsButton.UseVisualStyleBackColor = true;
			this.SaveDatabaseSettingsButton.Click += new System.EventHandler(this.SaveDatabaseSettingsButton_Click);
			// 
			// FindDPDBButton
			// 
			this.FindDPDBButton.Location = new System.Drawing.Point(447, 17);
			this.FindDPDBButton.Name = "FindDPDBButton";
			this.FindDPDBButton.Size = new System.Drawing.Size(75, 23);
			this.FindDPDBButton.TabIndex = 34;
			this.FindDPDBButton.Text = "Find DPDB";
			this.FindDPDBButton.UseVisualStyleBackColor = true;
			this.FindDPDBButton.Click += new System.EventHandler(this.FindDPDBButton_Click);
			// 
			// DPDBTextBox
			// 
			this.DPDBTextBox.Location = new System.Drawing.Point(140, 19);
			this.DPDBTextBox.Name = "DPDBTextBox";
			this.DPDBTextBox.Size = new System.Drawing.Size(268, 20);
			this.DPDBTextBox.TabIndex = 32;
			// 
			// STLocationLabel
			// 
			this.STLocationLabel.AutoSize = true;
			this.STLocationLabel.Location = new System.Drawing.Point(6, 22);
			this.STLocationLabel.Name = "STLocationLabel";
			this.STLocationLabel.Size = new System.Drawing.Size(115, 13);
			this.STLocationLabel.TabIndex = 31;
			this.STLocationLabel.Text = "Stocktakr DB Location";
			// 
			// ConnectionTab
			// 
			this.ConnectionTab.Controls.Add(this.ConnectionErrorlabel);
			this.ConnectionTab.Controls.Add(this.StoreIDtextBox);
			this.ConnectionTab.Controls.Add(this.PasswordTextBox);
			this.ConnectionTab.Controls.Add(this.TestConnectionErrorlabel);
			this.ConnectionTab.Controls.Add(this.SaveConnectionSettingsButton);
			this.ConnectionTab.Controls.Add(this.Storelabel);
			this.ConnectionTab.Controls.Add(this.Passwordlabel);
			this.ConnectionTab.Controls.Add(this.TestConnectionButton);
			this.ConnectionTab.Location = new System.Drawing.Point(4, 22);
			this.ConnectionTab.Name = "ConnectionTab";
			this.ConnectionTab.Padding = new System.Windows.Forms.Padding(3);
			this.ConnectionTab.Size = new System.Drawing.Size(538, 524);
			this.ConnectionTab.TabIndex = 0;
			this.ConnectionTab.Text = "Connection";
			this.ConnectionTab.UseVisualStyleBackColor = true;
			// 
			// ConnectionErrorlabel
			// 
			this.ConnectionErrorlabel.AutoSize = true;
			this.ConnectionErrorlabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ConnectionErrorlabel.ForeColor = System.Drawing.Color.Red;
			this.ConnectionErrorlabel.Location = new System.Drawing.Point(135, 121);
			this.ConnectionErrorlabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.ConnectionErrorlabel.Name = "ConnectionErrorlabel";
			this.ConnectionErrorlabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.ConnectionErrorlabel.Size = new System.Drawing.Size(12, 15);
			this.ConnectionErrorlabel.TabIndex = 35;
			// 
			// StoreIDtextBox
			// 
			this.StoreIDtextBox.Location = new System.Drawing.Point(108, 16);
			this.StoreIDtextBox.Name = "StoreIDtextBox";
			this.StoreIDtextBox.Size = new System.Drawing.Size(386, 20);
			this.StoreIDtextBox.TabIndex = 26;
			// 
			// PasswordTextBox
			// 
			this.PasswordTextBox.Location = new System.Drawing.Point(108, 56);
			this.PasswordTextBox.Name = "PasswordTextBox";
			this.PasswordTextBox.Size = new System.Drawing.Size(386, 20);
			this.PasswordTextBox.TabIndex = 28;
			// 
			// TestConnectionErrorlabel
			// 
			this.TestConnectionErrorlabel.AutoSize = true;
			this.TestConnectionErrorlabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TestConnectionErrorlabel.ForeColor = System.Drawing.Color.Red;
			this.TestConnectionErrorlabel.Location = new System.Drawing.Point(16, 265);
			this.TestConnectionErrorlabel.Name = "TestConnectionErrorlabel";
			this.TestConnectionErrorlabel.Size = new System.Drawing.Size(2, 15);
			this.TestConnectionErrorlabel.TabIndex = 36;
			// 
			// SaveConnectionSettingsButton
			// 
			this.SaveConnectionSettingsButton.Location = new System.Drawing.Point(16, 117);
			this.SaveConnectionSettingsButton.Name = "SaveConnectionSettingsButton";
			this.SaveConnectionSettingsButton.Size = new System.Drawing.Size(87, 23);
			this.SaveConnectionSettingsButton.TabIndex = 34;
			this.SaveConnectionSettingsButton.Text = "Save Settings";
			this.SaveConnectionSettingsButton.UseVisualStyleBackColor = true;
			this.SaveConnectionSettingsButton.Click += new System.EventHandler(this.SaveConnectionSettingsButton_Click);
			// 
			// Storelabel
			// 
			this.Storelabel.AutoSize = true;
			this.Storelabel.Location = new System.Drawing.Point(13, 16);
			this.Storelabel.Name = "Storelabel";
			this.Storelabel.Size = new System.Drawing.Size(49, 13);
			this.Storelabel.TabIndex = 25;
			this.Storelabel.Text = "Store ID:";
			// 
			// Passwordlabel
			// 
			this.Passwordlabel.AutoSize = true;
			this.Passwordlabel.Location = new System.Drawing.Point(13, 56);
			this.Passwordlabel.Name = "Passwordlabel";
			this.Passwordlabel.Size = new System.Drawing.Size(56, 13);
			this.Passwordlabel.TabIndex = 27;
			this.Passwordlabel.Text = "Password:";
			// 
			// TestConnectionButton
			// 
			this.TestConnectionButton.Location = new System.Drawing.Point(16, 210);
			this.TestConnectionButton.Name = "TestConnectionButton";
			this.TestConnectionButton.Size = new System.Drawing.Size(102, 23);
			this.TestConnectionButton.TabIndex = 24;
			this.TestConnectionButton.Text = "Test Connection";
			this.TestConnectionButton.UseVisualStyleBackColor = true;
			this.TestConnectionButton.Click += new System.EventHandler(this.TestConnectionButton_Click);
			// 
			// MainTabControl
			// 
			this.MainTabControl.Controls.Add(this.ConnectionTab);
			this.MainTabControl.Controls.Add(this.DatabasesTab);
			this.MainTabControl.Controls.Add(this.SyncTab);
			this.MainTabControl.Location = new System.Drawing.Point(28, 26);
			this.MainTabControl.Name = "MainTabControl";
			this.MainTabControl.SelectedIndex = 0;
			this.MainTabControl.Size = new System.Drawing.Size(546, 550);
			this.MainTabControl.TabIndex = 39;
			// 
			// SyncTab
			// 
			this.SyncTab.Controls.Add(this.panel2);
			this.SyncTab.Location = new System.Drawing.Point(4, 22);
			this.SyncTab.Name = "SyncTab";
			this.SyncTab.Padding = new System.Windows.Forms.Padding(3);
			this.SyncTab.Size = new System.Drawing.Size(538, 524);
			this.SyncTab.TabIndex = 3;
			this.SyncTab.Text = "Sync";
			this.SyncTab.UseVisualStyleBackColor = true;
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.SendOptionsGroupBox);
			this.panel2.Controls.Add(this.CommitItemsErrorLabel);
			this.panel2.Controls.Add(this.CommitStocktakeButton);
			this.panel2.Controls.Add(this.GetStocktakeTransactionsErrorLabel);
			this.panel2.Controls.Add(this.GetItemsButton);
			this.panel2.Controls.Add(this.label9);
			this.panel2.Location = new System.Drawing.Point(29, 28);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(479, 477);
			this.panel2.TabIndex = 46;
			// 
			// SendOptionsGroupBox
			// 
			this.SendOptionsGroupBox.Controls.Add(this.UpdateItemsButton);
			this.SendOptionsGroupBox.Controls.Add(this.ReplaceItemsButton);
			this.SendOptionsGroupBox.Controls.Add(this.SendItemsErrorLabel);
			this.SendOptionsGroupBox.Location = new System.Drawing.Point(23, 58);
			this.SendOptionsGroupBox.Name = "SendOptionsGroupBox";
			this.SendOptionsGroupBox.Size = new System.Drawing.Size(432, 120);
			this.SendOptionsGroupBox.TabIndex = 48;
			this.SendOptionsGroupBox.TabStop = false;
			this.SendOptionsGroupBox.Text = "1. Send Products to Server";
			// 
			// UpdateItemsButton
			// 
			this.UpdateItemsButton.Location = new System.Drawing.Point(264, 36);
			this.UpdateItemsButton.Name = "UpdateItemsButton";
			this.UpdateItemsButton.Size = new System.Drawing.Size(139, 38);
			this.UpdateItemsButton.TabIndex = 43;
			this.UpdateItemsButton.Text = "Update New Items Only";
			this.UpdateItemsButton.UseVisualStyleBackColor = true;
			this.UpdateItemsButton.Click += new System.EventHandler(this.UpdateItemsButton_Click);
			// 
			// ReplaceItemsButton
			// 
			this.ReplaceItemsButton.Location = new System.Drawing.Point(17, 36);
			this.ReplaceItemsButton.Name = "ReplaceItemsButton";
			this.ReplaceItemsButton.Size = new System.Drawing.Size(139, 38);
			this.ReplaceItemsButton.TabIndex = 1;
			this.ReplaceItemsButton.Text = "Replace All Items";
			this.ReplaceItemsButton.UseVisualStyleBackColor = true;
			this.ReplaceItemsButton.Click += new System.EventHandler(this.ReplaceItemsButton_Click);
			// 
			// SendItemsErrorLabel
			// 
			this.SendItemsErrorLabel.AutoSize = true;
			this.SendItemsErrorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SendItemsErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.SendItemsErrorLabel.Location = new System.Drawing.Point(17, 88);
			this.SendItemsErrorLabel.Name = "SendItemsErrorLabel";
			this.SendItemsErrorLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.SendItemsErrorLabel.Size = new System.Drawing.Size(12, 15);
			this.SendItemsErrorLabel.TabIndex = 42;
			// 
			// CommitItemsErrorLabel
			// 
			this.CommitItemsErrorLabel.AutoSize = true;
			this.CommitItemsErrorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CommitItemsErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.CommitItemsErrorLabel.Location = new System.Drawing.Point(29, 306);
			this.CommitItemsErrorLabel.Name = "CommitItemsErrorLabel";
			this.CommitItemsErrorLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.CommitItemsErrorLabel.Size = new System.Drawing.Size(12, 15);
			this.CommitItemsErrorLabel.TabIndex = 45;
			// 
			// CommitStocktakeButton
			// 
			this.CommitStocktakeButton.Location = new System.Drawing.Point(29, 257);
			this.CommitStocktakeButton.Name = "CommitStocktakeButton";
			this.CommitStocktakeButton.Size = new System.Drawing.Size(193, 35);
			this.CommitStocktakeButton.TabIndex = 44;
			this.CommitStocktakeButton.Text = "3. Write Stocktake Items to POS Database";
			this.CommitStocktakeButton.UseVisualStyleBackColor = true;
			this.CommitStocktakeButton.Click += new System.EventHandler(this.CommitStocktakeButton_Click);
			// 
			// GetStocktakeTransactionsErrorLabel
			// 
			this.GetStocktakeTransactionsErrorLabel.AutoSize = true;
			this.GetStocktakeTransactionsErrorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.GetStocktakeTransactionsErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.GetStocktakeTransactionsErrorLabel.Location = new System.Drawing.Point(228, 206);
			this.GetStocktakeTransactionsErrorLabel.Name = "GetStocktakeTransactionsErrorLabel";
			this.GetStocktakeTransactionsErrorLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.GetStocktakeTransactionsErrorLabel.Size = new System.Drawing.Size(12, 15);
			this.GetStocktakeTransactionsErrorLabel.TabIndex = 43;
			// 
			// GetItemsButton
			// 
			this.GetItemsButton.Location = new System.Drawing.Point(28, 193);
			this.GetItemsButton.Name = "GetItemsButton";
			this.GetItemsButton.Size = new System.Drawing.Size(193, 38);
			this.GetItemsButton.TabIndex = 2;
			this.GetItemsButton.Text = "2. Get StockTake Items from Server";
			this.GetItemsButton.UseVisualStyleBackColor = true;
			this.GetItemsButton.Click += new System.EventHandler(this.GetItemsButton_Click);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(24, 19);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(155, 24);
			this.label9.TabIndex = 0;
			this.label9.Text = "Items in Inventory";
			// 
			// FormMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1124, 590);
			this.Controls.Add(this.MainTabControl);
			this.Controls.Add(this.lbLog);
			this.Name = "FormMain";
			this.Text = "StockTakr POS Client";
			this.DatabasesTab.ResumeLayout(false);
			this.DatabasesTab.PerformLayout();
			this.POSSoftwareSplitContainer.Panel1.ResumeLayout(false);
			this.POSSoftwareSplitContainer.Panel1.PerformLayout();
			this.POSSoftwareSplitContainer.Panel2.ResumeLayout(false);
			this.POSSoftwareSplitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.POSSoftwareSplitContainer)).EndInit();
			this.POSSoftwareSplitContainer.ResumeLayout(false);
			this.POSSoftwareGroupBox.ResumeLayout(false);
			this.POSSoftwareGroupBox.PerformLayout();
			this.ConnectionTab.ResumeLayout(false);
			this.ConnectionTab.PerformLayout();
			this.MainTabControl.ResumeLayout(false);
			this.SyncTab.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.SendOptionsGroupBox.ResumeLayout(false);
			this.SendOptionsGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		
		[STAThread]
		static void Main() 
		{
            try
            {
                Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                bool flag = false;
            }
		}

		public void AddLog(string entry, bool writeToFile)
		{
			DateTime dt = DateTime.Now;
			lbLog.AppendText("[" + dt.ToLongDateString() + " " + dt.ToLongTimeString() + "] " + entry + "\r\n");

			try
			{

				if (writeToFile)
				{
					string logentry = dt.ToLongDateString() + " " + dt.ToLongTimeString() + "\t" + entry + "\r\n";

					string path = System.Environment.CurrentDirectory + "\\log.txt";

					if (!File.Exists(path))
					{
						// Create a reference to a file.
						FileInfo fi = new FileInfo(path);
						// Actually create the file.
						FileStream fs = fi.Create();
						// Modify the file as required, and then close the file.
						fs.Close();
					}

					File.AppendAllText(path, logentry);
				}
			}
			catch (Exception ex)
			{
				lbLog.AppendText("[" + dt.ToLongDateString() + " " + dt.ToLongTimeString() + "] " + ex.ToString() + "\r\n");
			}

		}


		
		public FormMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
				
			//Load Connection settings
			StoreIDtextBox.Text = Properties.Settings.Default.store_id;
			PasswordTextBox.Text = Properties.Settings.Default.password;
			
			RMDBTextBox.Text = Properties.Settings.Default.RMDBLocation;
			DPDBTextBox.Text = Properties.Settings.Default.StocktakrDBLocation;


			if (Properties.Settings.Default.POSSoftware == "MYOB")
			{
				MYOBRadioButton.Checked = true;
				MicrosoftRMSRadioButton.Checked = false;
				ToggleSoftwarePanels(true);

				RMDBTextBox.Text = Properties.Settings.Default.RMDBLocation;		
			}
			else if (Properties.Settings.Default.POSSoftware == "Microsoft")
			{
				MYOBRadioButton.Checked = false;
				MicrosoftRMSRadioButton.Checked = true;
				ToggleSoftwarePanels(false);

				MicrosoftLocationTextBox.Text = Properties.Settings.Default.POSServerLocation;
				MicrosoftDBTextBox.Text = Properties.Settings.Default.POSServerDBName;
				MicrosoftUserTextBox.Text = Properties.Settings.Default.POSServerUser;
				MicrosoftPasswordTextBox.Text = Properties.Settings.Default.POSServerPassword;				
			}
		}   


		
		#region Button Events
	        
		private void ClearErrorMessages()
		{
			ConnectionErrorlabel.Text = "";
			TestConnectionErrorlabel.Text = "";
			
			TestMicrosoftConnectionErrorLabel.Text = "";
			DatabaseSettingsErrorLabel.Text = "";
			SendItemsErrorLabel.Text = "";
			GetStocktakeTransactionsErrorLabel.Text = "";
			CommitItemsErrorLabel.Text = "";
		}		

		private void SaveConnectionSettingsButton_Click(object sender, EventArgs e)
		   {
			  ClearErrorMessages();
			  string error_message = "";
			  bool is_valid = true;

			  if(StoreIDtextBox.Text == "")
			  {
				 error_message = "Please enter the store ID from the website.";
				 is_valid = false;
			  }
			  else if(PasswordTextBox.Text == "")
			  {
				 error_message = "Please enter the password for the store ID from the website.";
				 is_valid = false;
			  }
	            
			  try
			  {
				 if (is_valid)
				 {
	                    
					Properties.Settings.Default.store_id = StoreIDtextBox.Text;
					Properties.Settings.Default.password = PasswordTextBox.Text;                    
					
	                   
					Properties.Settings.Default.Save();
					ConnectionErrorlabel.Text = "Settings saved successfully";
				 }
				 else
				 {
					ConnectionErrorlabel.Text = error_message;
				 }
			  }
			  catch (Exception ex)
			  {
				 ConnectionErrorlabel.Text = ex.ToString();
			  }
		   }		
		private void SaveDatabaseSettingsButton_Click(object sender, EventArgs e)
		{
			ClearErrorMessages();
			string error_message = "";
			bool is_valid = true;

			if (MYOBRadioButton.Checked)
			{
				if (String.IsNullOrEmpty(RMDBTextBox.Text))
				{
					error_message = "Please enter the location of the Retail Manager database.";

				}
				else
				{
					is_valid = true;
					Properties.Settings.Default.POSSoftware = "MYOB";
					Properties.Settings.Default.RMDBLocation = RMDBTextBox.Text;
				}
			}
			else if (MicrosoftRMSRadioButton.Checked)
			{
				if (String.IsNullOrEmpty(MicrosoftLocationTextBox.Text))
				{
					error_message = "Please enter the location of the Microsoft RMS server.";
				}
				else if (String.IsNullOrEmpty(MicrosoftDBTextBox.Text))
				{
					error_message = "Please enter the name of the Microsoft RMS database.";
				}
				else if (String.IsNullOrEmpty(MicrosoftUserTextBox.Text))
				{
					error_message = "Please enter the user name to connect to the Microsoft RMS database.";
				}
				else if (String.IsNullOrEmpty(MicrosoftPasswordTextBox.Text))
				{
					error_message = "Please enter the password to connect to the Microsoft RMS database.";
				}
				else
				{
					is_valid = true;
					Properties.Settings.Default.POSSoftware = "Microsoft";
					Properties.Settings.Default.POSServerLocation = MicrosoftLocationTextBox.Text;
					Properties.Settings.Default.POSServerDBName = MicrosoftDBTextBox.Text;
					Properties.Settings.Default.POSServerUser = MicrosoftUserTextBox.Text;
					Properties.Settings.Default.POSServerPassword = MicrosoftPasswordTextBox.Text;
				}

			}

			if (RMDBTextBox.Text == "")
			{
				error_message = "Please enter the location of the Retail Manager database.";
				is_valid = false;
			}
			else if (DPDBTextBox.Text == "")
			{
				error_message = "Please enter the location of the DocketPlace database.";
				is_valid = false;
			}

			try
			{
				if (is_valid)
				{

					Properties.Settings.Default.RMDBLocation = RMDBTextBox.Text;
					Properties.Settings.Default.StocktakrDBLocation = DPDBTextBox.Text;
					

					Properties.Settings.Default.Save();
					DatabaseSettingsErrorLabel.Text = "Settings saved successfully";
				}
				else
				{
					DatabaseSettingsErrorLabel.Text = error_message;
				}
			}
			catch (Exception ex)
			{
				DatabaseSettingsErrorLabel.Text = ex.ToString();
			}
		}

		private void TestConnectionButton_Click(object sender, EventArgs e)
		{
			ClearErrorMessages();
			try
			{
				ItemResponse newResponse = new POSItemHandler().TestConnection(Convert.ToInt32(StoreIDtextBox.Text), PasswordTextBox.Text);

				if (newResponse.is_error)
				{
					AddLog(newResponse.errorMessage, true);					
				}
				else
				{
					ConnectionErrorlabel.Text = "Connection is solid";
				}
			}		
			catch (System.Net.WebException ex)
			{
				ConnectionErrorlabel.Text = "No internet connection";
			}			
		}

	
		private void FindDPDBButton_Click(object sender, EventArgs e)
		{
			if (DPOpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = DPOpenFileDialog.FileName;
				DPDBTextBox.Text = fileName;
			}
		}

	
		private void MYOBRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (MYOBRadioButton.Checked)
			{
				ToggleSoftwarePanels(true);
			}
		}

		private void MicrosoftRMSRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (MicrosoftRMSRadioButton.Checked)
			{
				ToggleSoftwarePanels(false);
			}
		}

		private void ToggleSoftwarePanels(bool MYOBPicked)
		{
			POSSoftwareSplitContainer.Panel1Collapsed = !MYOBPicked;
			POSSoftwareSplitContainer.Panel2Collapsed = MYOBPicked;
		}

		private void MicrosoftTestConnectionButton_Click(object sender, EventArgs e)
		{
			ClearErrorMessages();
			if (TestSQLConnection(MicrosoftLocationTextBox.Text, MicrosoftDBTextBox.Text, MicrosoftUserTextBox.Text, MicrosoftPasswordTextBox.Text))
			{
				TestMicrosoftConnectionErrorLabel.Text = "Connection is solid";
			}
			else
			{
				TestMicrosoftConnectionErrorLabel.Text = "Error connecting to SQL Server.";
			}
		}


		private void UpdateItemsButton_Click(object sender, EventArgs e)
		{
			SendItems(false);
		}

		private void ReplaceItemsButton_Click(object sender, EventArgs e)
		{

			// Initializes the variables to pass to the MessageBox.Show method.

			string message = "Are you sure ? This action will replace ALL items on the server including stocktake data.";
			string caption = "Confirm ";

			MessageBoxButtons buttons = MessageBoxButtons.YesNo;
			
			DialogResult result;

			// Displays the MessageBox.

			result = MessageBox.Show(message, caption, buttons);

			if (result == System.Windows.Forms.DialogResult.Yes)
			{				// Closes the parent form.
				SendItems(true);
			}
		}

		private void SendItems(bool replaceAll)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				//Check if all fields are filled.
				if ((StoreIDtextBox.Text == "") || (PasswordTextBox.Text == ""))
				{
					SendItemsErrorLabel.Text = "Store ID or Password are empty";
					return;
				}

				int storeID = Convert.ToInt32(StoreIDtextBox.Text);

				List<LocalItem> localItems = new List<LocalItem>();

				switch (Properties.Settings.Default.POSSoftware)
				{
					case "MYOB":
						if (replaceAll)
						{
							localItems = MYOB.GetCompleteItemList();
						}
						else
						{
							localItems = MYOB.GetRecentlyModifiedItemList();
						}

						break;
					case "Microsoft":
						if (replaceAll)
						{
							if (TestSQLConnection(MicrosoftLocationTextBox.Text, MicrosoftDBTextBox.Text, MicrosoftUserTextBox.Text, MicrosoftPasswordTextBox.Text))
						     {
							     localItems = MicrosoftRMS.GetCompleteItemList();
						     }
						}
						else
						{
							if (TestSQLConnection(MicrosoftLocationTextBox.Text, MicrosoftDBTextBox.Text, MicrosoftUserTextBox.Text, MicrosoftPasswordTextBox.Text))
							{
								localItems = MicrosoftRMS.GetRecentlyModifiedItemList();
							}
						}
						
						break;
				}

				if (localItems.Count() == 0)
				{
					AddLog("No items found", true);
				}
				else
				{


					POSItemHandler handler = new POSItemHandler();					

					handler.Timeout = 1000000;

                         var jsonSerializer = new JavaScriptSerializer();
                         jsonSerializer.MaxJsonLength = Int32.MaxValue;

                         string rawData = jsonSerializer.Serialize(localItems);

                         string compressedData = ZipHelper.CompressToGzip(rawData);                             

					ItemResponse newResponse = handler.UpdateOrReplaceItems(storeID, PasswordTextBox.Text, compressedData, replaceAll);

					if (newResponse.is_error)
					{
						AddLog(newResponse.errorMessage, true);
						SendItemsErrorLabel.Text = newResponse.errorMessage;
					}
					else
					{
						SendItemsErrorLabel.Text = "Success";
						AddLog(localItems.Count() + " Items sent to server.", true);
					}
				}
			}
			catch (Exception ex)
			{
				AddLog(ex.ToString(), true);
				SendItemsErrorLabel.Text = "An error has occurred. See log";
			}

			Cursor.Current = Cursors.Default;
		}

		private void GetItemsButton_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			// Initializes the variables to pass to the MessageBox.Show method.

			string message = "Are you sure ? This action will download ALL stocktake transactions on the server and then delete them.";
			string caption = "Confirm ";

			MessageBoxButtons buttons = MessageBoxButtons.YesNo;

			DialogResult result;

			// Displays the MessageBox.

			result = MessageBox.Show(message, caption, buttons);

			if (result == System.Windows.Forms.DialogResult.Yes)
			{
				if (!Helpers.TestDPDBConnection())
				{
					GetStocktakeTransactionsErrorLabel.Text = "Error see log";
					AddLog("Unable to connect to  the Docketplace Stocktakr database", true);
				}
				else
				{
					try
					{
						//Check if all fields are filled.
						if ((StoreIDtextBox.Text == "") || (PasswordTextBox.Text == ""))
						{
							GetStocktakeTransactionsErrorLabel.Text = "Store ID or Password are empty";
							return;
						}

						int storeID = Convert.ToInt32(StoreIDtextBox.Text);

						POSItemHandler handler = new POSItemHandler();

						ItemResponse newResponse = handler.GetStocktakeTransactions(storeID, PasswordTextBox.Text);

						if (!newResponse.is_error)
						{

							if (newResponse.localStocktakeTransactions == null)
							{
								AddLog("No Stocktake transactions", true);
							}
							else
							{
								Helpers.DownloadStockTakeTransactions(newResponse.localStocktakeTransactions);
                                        var deleteResponse = handler.DeleteStocktakeTransactions(storeID, PasswordTextBox.Text);
								AddLog(newResponse.localStocktakeTransactions.Count() + " Stocktake Transactions downloaded", true);
							}
						}
						else
						{
							AddLog(newResponse.errorMessage, true);
							GetStocktakeTransactionsErrorLabel.Text = newResponse.errorMessage;
						}
					}
					catch (Exception ex)
					{
						AddLog(ex.ToString(), true);
						GetStocktakeTransactionsErrorLabel.Text = "An error has occurred. See log";
					}
				}
			}
			Cursor.Current = Cursors.WaitCursor;
		}


		private bool TestSQLConnection(string location, string DBname, string user, string password)
		{
			string connectionString = MicrosoftRMS.MakeConnectionString(location, DBname, user, password);
			SqlConnection conn = new SqlConnection(connectionString);

			try
			{
				conn.Open();
				return true;
			}
			catch (Exception ex)
			{
				lbLog.AppendText(ex.ToString() + "\r\n");
				return false;
			}
			finally
			{
				conn.Close();
			}
		}

		private void FindRMDBbutton_Click(object sender, EventArgs e)
		{
			if (RMOpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = RMOpenFileDialog.FileName;
				RMDBTextBox.Text = fileName;
			}
		}

		private void CommitStocktakeButton_Click(object sender, EventArgs e)
		{
			// Initializes the variables to pass to the MessageBox.Show method.

			string message = "Are you sure ? This action will move all stocktake transactions from the temporary database to the POS database.";
			string caption = "Confirm ";

			MessageBoxButtons buttons = MessageBoxButtons.YesNo;
			
			DialogResult result;

			// Displays the MessageBox.

			result = MessageBox.Show(message, caption, buttons);

			if (result == System.Windows.Forms.DialogResult.Yes)
			{				// Closes the parent form.
				Cursor.Current = Cursors.WaitCursor;				
				try
				{
                         int itemCount = 0;
					switch (Properties.Settings.Default.POSSoftware)
					{
						case "MYOB":						
							itemCount = MYOB.CommitStocktakeToPOSDatabase();
							CommitItemsErrorLabel.Text = "Success";
							AddLog(itemCount.ToString() + " Items stocktaked", true);								
							break;
						case "Microsoft":
                                   //17chars only due to limit in RMS db.
                                   string code = DateTime.Now.ToString("yyyy-mm-dd_hh:mm");
                                   
                                   itemCount = MicrosoftRMS.CommitStocktakeToPOSDatabase(code);
                                   
                                   AddLog(itemCount.ToString() + " Items stocktaked", true);

                                   CommitItemsErrorLabel.Text = "Stocktake successful. Lookup code for the Physical Inventory is: " + code;
							break;
					}				
				}
				catch (Exception ex)
				{
					AddLog(ex.ToString(), true);
					SendItemsErrorLabel.Text = "An error has occurred. See log";
				}
			}
			Cursor.Current = Cursors.Default;
		}

		#endregion
	}	
}		