namespace EEPROMProgrammer
{
    partial class AddressForm
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
            this.startAddressNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.endAddressLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dialogTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.startAddressNumericUpDown)).BeginInit();
            this.dialogTableLayoutPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startAddressNumericUpDown
            // 
            this.startAddressNumericUpDown.Hexadecimal = true;
            this.startAddressNumericUpDown.Location = new System.Drawing.Point(13, 28);
            this.startAddressNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.startAddressNumericUpDown.Name = "startAddressNumericUpDown";
            this.startAddressNumericUpDown.Size = new System.Drawing.Size(62, 20);
            this.startAddressNumericUpDown.TabIndex = 0;
            this.startAddressNumericUpDown.ValueChanged += new System.EventHandler(this.startAddressNumericUpDown_ValueChanged);
            // 
            // endAddressLabel
            // 
            this.endAddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.endAddressLabel.Location = new System.Drawing.Point(103, 28);
            this.endAddressLabel.Name = "endAddressLabel";
            this.endAddressLabel.Size = new System.Drawing.Size(57, 21);
            this.endAddressLabel.TabIndex = 2;
            this.endAddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "to";
            // 
            // dialogTableLayoutPanel
            // 
            this.dialogTableLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dialogTableLayoutPanel.AutoSize = true;
            this.dialogTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dialogTableLayoutPanel.ColumnCount = 2;
            this.dialogTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.dialogTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.dialogTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
            this.dialogTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
            this.dialogTableLayoutPanel.Location = new System.Drawing.Point(18, 89);
            this.dialogTableLayoutPanel.Margin = new System.Windows.Forms.Padding(6);
            this.dialogTableLayoutPanel.Name = "dialogTableLayoutPanel";
            this.dialogTableLayoutPanel.RowCount = 1;
            this.dialogTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.dialogTableLayoutPanel.Size = new System.Drawing.Size(162, 29);
            this.dialogTableLayoutPanel.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(3, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(84, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.startAddressNumericUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.endAddressLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(173, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Address";
            // 
            // AddressForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(198, 124);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dialogTableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Binary File";
            ((System.ComponentModel.ISupportInitialize)(this.startAddressNumericUpDown)).EndInit();
            this.dialogTableLayoutPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown startAddressNumericUpDown;
        private System.Windows.Forms.Label endAddressLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel dialogTableLayoutPanel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}