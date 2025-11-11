using SuchByte.MacroDeck.GUI.CustomControls;

namespace MultiDisplayVCPClient.GUI
{
    partial class VcpConnectionToggler
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_client != null)
                {
                    _client.ConnectionStateChanged -= OnConnectionStateChanged;
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.btnToggle = new SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pbRefresh = new System.Windows.Forms.PictureBox();

            // --- ADD THIS ---
            this.separatorPanel = new System.Windows.Forms.Panel();
            // --- END ADD ---

            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblName.ForeColor = System.Drawing.Color.White;
            this.lblName.Location = new System.Drawing.Point(3, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(134, 28);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Connection";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnToggle
            // 
            this.btnToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggle.BorderRadius = 8;
            this.btnToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnToggle.ForeColor = System.Drawing.Color.White;
            this.btnToggle.HoverColor = System.Drawing.Color.Empty;
            this.btnToggle.Icon = null;
            this.btnToggle.Location = new System.Drawing.Point(220, 3);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Progress = 0;
            this.btnToggle.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            this.btnToggle.Size = new System.Drawing.Size(80, 28);
            this.btnToggle.TabIndex = 1;
            this.btnToggle.Text = "Connect";
            this.btnToggle.UseWindowsAccentColor = true;
            this.btnToggle.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(143, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(71, 23);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Offline";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbRefresh
            // 
            this.pbRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.pbRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbRefresh.Image = global::MultiDisplayVCPClient.Properties.Resources.MCC_Refresh;
            this.pbRefresh.Location = new System.Drawing.Point(306, 3);
            this.pbRefresh.Name = "pbRefresh";
            this.pbRefresh.Padding = new System.Windows.Forms.Padding(1);
            this.pbRefresh.Size = new System.Drawing.Size(28, 28);
            this.pbRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbRefresh.TabIndex = 2;
            this.pbRefresh.TabStop = false;
            // 
            // --- ADD THIS ---
            // separatorPanel
            // 
            this.separatorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.separatorPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.separatorPanel.Location = new System.Drawing.Point(0, 34);
            this.separatorPanel.Name = "separatorPanel";
            this.separatorPanel.Size = new System.Drawing.Size(337, 1);
            this.separatorPanel.TabIndex = 3;
            // --- END ADD ---
            // 
            // VcpConnectionToggler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = false;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));

            // --- ADD THIS ---
            this.Controls.Add(this.separatorPanel);
            // --- END ADD ---

            this.Controls.Add(this.pbRefresh);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnToggle);
            this.Controls.Add(this.lblName);
            this.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.Name = "VcpConnectionToggler";

            // --- MODIFIED ---
            this.Size = new System.Drawing.Size(337, 37); // Was 35
            // --- END MODIFIED ---

            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private ButtonPrimary btnToggle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox pbRefresh;

        // --- ADD THIS ---
        private System.Windows.Forms.Panel separatorPanel;
        // --- END ADD ---
    }
}