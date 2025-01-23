namespace EEPROMProgrammer
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.versionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.stopToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.eepromStartAddressComboBox = new System.Windows.Forms.ComboBox();
            this.eepromSizeComboBox = new System.Windows.Forms.ComboBox();
            this.writeButton = new System.Windows.Forms.Button();
            this.stateTextLabel = new System.Windows.Forms.Label();
            this.stateColorLabel = new System.Windows.Forms.Label();
            this.pollTimer = new System.Windows.Forms.Timer(this.components);
            this.eepromEndAddressLabel = new System.Windows.Forms.Label();
            this.readButton = new System.Windows.Forms.Button();
            this.verifyButton = new System.Windows.Forms.Button();
            this.deviceGroupBox = new System.Windows.Forms.GroupBox();
            this.addressGroupBox = new System.Windows.Forms.GroupBox();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.deviceGroupBox.SuspendLayout();
            this.addressGroupBox.SuspendLayout();
            this.statusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionToolStripStatusLabel,
            this.stopToolStripDropDownButton,
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 243);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(314, 26);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 6;
            // 
            // versionToolStripStatusLabel
            // 
            this.versionToolStripStatusLabel.Name = "versionToolStripStatusLabel";
            this.versionToolStripStatusLabel.Size = new System.Drawing.Size(0, 21);
            // 
            // stopToolStripDropDownButton
            // 
            this.stopToolStripDropDownButton.AutoSize = false;
            this.stopToolStripDropDownButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.stopToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stopToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("stopToolStripDropDownButton.Image")));
            this.stopToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopToolStripDropDownButton.Name = "stopToolStripDropDownButton";
            this.stopToolStripDropDownButton.ShowDropDownArrow = false;
            this.stopToolStripDropDownButton.Size = new System.Drawing.Size(50, 24);
            this.stopToolStripDropDownButton.Text = "Stop";
            this.stopToolStripDropDownButton.Visible = false;
            this.stopToolStripDropDownButton.Click += new System.EventHandler(this.stopToolStripDropDownButton_Click);
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.AutoSize = false;
            this.toolStripProgressBar.Margin = new System.Windows.Forms.Padding(12, 3, 1, 3);
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(230, 20);
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBar.Visible = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(70, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "to";
            // 
            // eepromStartAddressComboBox
            // 
            this.eepromStartAddressComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eepromStartAddressComboBox.FormattingEnabled = true;
            this.eepromStartAddressComboBox.Location = new System.Drawing.Point(10, 29);
            this.eepromStartAddressComboBox.Name = "eepromStartAddressComboBox";
            this.eepromStartAddressComboBox.Size = new System.Drawing.Size(56, 21);
            this.eepromStartAddressComboBox.TabIndex = 0;
            this.eepromStartAddressComboBox.SelectedIndexChanged += new System.EventHandler(this.eepromStartAddressComboBox_SelectedIndexChanged);
            // 
            // eepromSizeComboBox
            // 
            this.eepromSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eepromSizeComboBox.FormattingEnabled = true;
            this.eepromSizeComboBox.Items.AddRange(new object[] {
            "2864 (8K)",
            "28128 (16K)",
            "28256 (32K)",
            "28512 (64K)",
            "2764 (8K)",
            "27128 (16K)",
            "27256 (32K)",
            "27512 (64K)"});
            this.eepromSizeComboBox.Location = new System.Drawing.Point(13, 29);
            this.eepromSizeComboBox.Name = "eepromSizeComboBox";
            this.eepromSizeComboBox.Size = new System.Drawing.Size(96, 21);
            this.eepromSizeComboBox.TabIndex = 0;
            this.eepromSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.eepromSizeComboBox_SelectedIndexChanged);
            // 
            // writeButton
            // 
            this.writeButton.BackColor = System.Drawing.SystemColors.Control;
            this.writeButton.Enabled = false;
            this.writeButton.Image = ((System.Drawing.Image)(resources.GetObject("writeButton.Image")));
            this.writeButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.writeButton.Location = new System.Drawing.Point(12, 153);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(80, 80);
            this.writeButton.TabIndex = 3;
            this.writeButton.Text = "Write";
            this.writeButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.writeButton.UseVisualStyleBackColor = false;
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            // 
            // stateTextLabel
            // 
            this.stateTextLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.stateTextLabel.Location = new System.Drawing.Point(43, 23);
            this.stateTextLabel.Name = "stateTextLabel";
            this.stateTextLabel.Size = new System.Drawing.Size(236, 22);
            this.stateTextLabel.TabIndex = 1;
            this.stateTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stateColorLabel
            // 
            this.stateColorLabel.BackColor = System.Drawing.Color.Red;
            this.stateColorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stateColorLabel.Location = new System.Drawing.Point(13, 26);
            this.stateColorLabel.Name = "stateColorLabel";
            this.stateColorLabel.Size = new System.Drawing.Size(16, 16);
            this.stateColorLabel.TabIndex = 0;
            // 
            // pollTimer
            // 
            this.pollTimer.Enabled = true;
            this.pollTimer.Interval = 250;
            this.pollTimer.Tick += new System.EventHandler(this.pollTimer_Tick);
            // 
            // eepromEndAddressLabel
            // 
            this.eepromEndAddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.eepromEndAddressLabel.Location = new System.Drawing.Point(90, 29);
            this.eepromEndAddressLabel.Name = "eepromEndAddressLabel";
            this.eepromEndAddressLabel.Size = new System.Drawing.Size(57, 21);
            this.eepromEndAddressLabel.TabIndex = 2;
            this.eepromEndAddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // readButton
            // 
            this.readButton.Enabled = false;
            this.readButton.Image = ((System.Drawing.Image)(resources.GetObject("readButton.Image")));
            this.readButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.readButton.Location = new System.Drawing.Point(222, 153);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(80, 80);
            this.readButton.TabIndex = 5;
            this.readButton.Text = "Read";
            this.readButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.readButton.UseVisualStyleBackColor = false;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // verifyButton
            // 
            this.verifyButton.Enabled = false;
            this.verifyButton.Image = ((System.Drawing.Image)(resources.GetObject("verifyButton.Image")));
            this.verifyButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.verifyButton.Location = new System.Drawing.Point(117, 153);
            this.verifyButton.Name = "verifyButton";
            this.verifyButton.Size = new System.Drawing.Size(80, 80);
            this.verifyButton.TabIndex = 4;
            this.verifyButton.Text = "Verify";
            this.verifyButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.verifyButton.UseVisualStyleBackColor = false;
            this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
            // 
            // deviceGroupBox
            // 
            this.deviceGroupBox.Controls.Add(this.eepromSizeComboBox);
            this.deviceGroupBox.Location = new System.Drawing.Point(12, 79);
            this.deviceGroupBox.Name = "deviceGroupBox";
            this.deviceGroupBox.Size = new System.Drawing.Size(123, 68);
            this.deviceGroupBox.TabIndex = 1;
            this.deviceGroupBox.TabStop = false;
            this.deviceGroupBox.Text = "Device";
            // 
            // addressGroupBox
            // 
            this.addressGroupBox.Controls.Add(this.eepromStartAddressComboBox);
            this.addressGroupBox.Controls.Add(this.label4);
            this.addressGroupBox.Controls.Add(this.eepromEndAddressLabel);
            this.addressGroupBox.Location = new System.Drawing.Point(142, 79);
            this.addressGroupBox.Name = "addressGroupBox";
            this.addressGroupBox.Size = new System.Drawing.Size(160, 68);
            this.addressGroupBox.TabIndex = 2;
            this.addressGroupBox.TabStop = false;
            this.addressGroupBox.Text = "Address";
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Controls.Add(this.stateColorLabel);
            this.statusGroupBox.Controls.Add(this.stateTextLabel);
            this.statusGroupBox.Location = new System.Drawing.Point(12, 12);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(290, 61);
            this.statusGroupBox.TabIndex = 0;
            this.statusGroupBox.TabStop = false;
            this.statusGroupBox.Text = "Status";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 269);
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.addressGroupBox);
            this.Controls.Add(this.deviceGroupBox);
            this.Controls.Add(this.verifyButton);
            this.Controls.Add(this.readButton);
            this.Controls.Add(this.writeButton);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "EEPROM Programmer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.deviceGroupBox.ResumeLayout(false);
            this.addressGroupBox.ResumeLayout(false);
            this.addressGroupBox.PerformLayout();
            this.statusGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox eepromSizeComboBox;
        private System.Windows.Forms.ComboBox eepromStartAddressComboBox;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.Label stateTextLabel;
        private System.Windows.Forms.Label stateColorLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer pollTimer;
        private System.Windows.Forms.Label eepromEndAddressLabel;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Button verifyButton;
        private System.Windows.Forms.ToolStripDropDownButton stopToolStripDropDownButton;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.GroupBox addressGroupBox;
        private System.Windows.Forms.GroupBox deviceGroupBox;
        private System.Windows.Forms.ToolStripStatusLabel versionToolStripStatusLabel;
    }
}

