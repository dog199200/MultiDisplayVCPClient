namespace MultiDisplayVCPClient.GUI
{
    partial class ConnectionEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        private SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary btnOk;
        private VcpConnectionConfigurator _configurator;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this._configurator = new MultiDisplayVCPClient.GUI.VcpConnectionConfigurator();
            this.btnOk = new SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary();
            this.SuspendLayout();
            // 
            // _configurator
            // 
            this._configurator.AutoSize = true;
            this._configurator.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._configurator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this._configurator.Location = new System.Drawing.Point(12, 12);
            this._configurator.Name = "_configurator";
            this._configurator.Padding = new System.Windows.Forms.Padding(5);
            this._configurator.Settings = null;
            this._configurator.Size = new System.Drawing.Size(340, 130);
            this._configurator.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.BorderRadius = 8;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.HoverColor = System.Drawing.Color.Empty;
            this.btnOk.Icon = null;
            this.btnOk.Location = new System.Drawing.Point(277, 148);
            this.btnOk.Name = "btnOk";
            this.btnOk.Progress = 0;
            this.btnOk.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseWindowsAccentColor = true;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // ConnectionEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 185);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this._configurator);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ConnectionEditorForm";
            this.Text = "VCP Server Configuration";
            this.Controls.SetChildIndex(this._configurator, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}