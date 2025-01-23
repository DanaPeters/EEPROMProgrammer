namespace EEPROMProgrammer
{
    partial class LoadSketchForm
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
            this.dialogTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dialogTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
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
            this.dialogTableLayoutPanel.Location = new System.Drawing.Point(36, 69);
            this.dialogTableLayoutPanel.Margin = new System.Windows.Forms.Padding(6);
            this.dialogTableLayoutPanel.Name = "dialogTableLayoutPanel";
            this.dialogTableLayoutPanel.RowCount = 1;
            this.dialogTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.dialogTableLayoutPanel.Size = new System.Drawing.Size(162, 29);
            this.dialogTableLayoutPanel.TabIndex = 4;
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 42);
            this.label1.TabIndex = 5;
            this.label1.Text = "A programming sketch was not detected.\r\n\r\nLoad the sketch on the Arduino Mega?";
            // 
            // LoadSketchForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(234, 104);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dialogTableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadSketchForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Sketch";
            this.dialogTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel dialogTableLayoutPanel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
    }
}