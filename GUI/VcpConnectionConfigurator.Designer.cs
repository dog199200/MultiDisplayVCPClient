namespace MultiDisplayVCPClient.GUI
{
    partial class VcpConnectionConfigurator
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox();
            this.lblIpAddress = new System.Windows.Forms.Label();
            this.txtIpAddress = new SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblName.ForeColor = System.Drawing.Color.White;
            this.lblName.Location = new System.Drawing.Point(3, 10);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 23);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.txtName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtName.Icon = null;
            this.txtName.Location = new System.Drawing.Point(109, 10);
            this.txtName.Multiline = false;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.txtName.PasswordChar = false;
            this.txtName.PlaceHolderColor = System.Drawing.Color.Gray;
            this.txtName.PlaceHolderText = "My Server";
            this.txtName.ReadOnly = false;
            this.txtName.SelectionStart = 0;
            this.txtName.Size = new System.Drawing.Size(228, 25);
            this.txtName.TabIndex = 1;
            this.txtName.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblIpAddress
            // 
            this.lblIpAddress.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblIpAddress.ForeColor = System.Drawing.Color.White;
            this.lblIpAddress.Location = new System.Drawing.Point(3, 40);
            this.lblIpAddress.Name = "lblIpAddress";
            this.lblIpAddress.Size = new System.Drawing.Size(100, 23);
            this.lblIpAddress.TabIndex = 2;
            this.lblIpAddress.Text = "IP Address:";
            this.lblIpAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIpAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.txtIpAddress.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtIpAddress.Icon = null;
            this.txtIpAddress.Location = new System.Drawing.Point(109, 40);
            this.txtIpAddress.Multiline = false;
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.txtIpAddress.PasswordChar = false;
            this.txtIpAddress.PlaceHolderColor = System.Drawing.Color.Gray;
            // 
            // --- MODIFIED ---
            //
            this.txtIpAddress.PlaceHolderText = "127.0.0.1 or localhost";
            //
            // --- END MODIFIED ---
            //
            this.txtIpAddress.ReadOnly = false;
            this.txtIpAddress.SelectionStart = 0;
            this.txtIpAddress.Size = new System.Drawing.Size(228, 25);
            this.txtIpAddress.TabIndex = 3;
            this.txtIpAddress.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblPort
            // 
            this.lblPort.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPort.ForeColor = System.Drawing.Color.White;
            this.lblPort.Location = new System.Drawing.Point(3, 70);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(100, 23);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.txtPort.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPort.Icon = null;
            this.txtPort.Location = new System.Drawing.Point(109, 70);
            this.txtPort.Multiline = false;
            this.txtPort.Name = "txtPort";
            this.txtPort.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.txtPort.PasswordChar = false;
            this.txtPort.PlaceHolderColor = System.Drawing.Color.Gray;
            this.txtPort.PlaceHolderText = "21000";
            this.txtPort.ReadOnly = false;
            this.txtPort.SelectionStart = 0;
            this.txtPort.Size = new System.Drawing.Size(228, 25);
            this.txtPort.TabIndex = 5;
            this.txtPort.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // lblPassword
            // 
            this.lblPassword.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPassword.ForeColor = System.Drawing.Color.White;
            this.lblPassword.Location = new System.Drawing.Point(3, 100);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(100, 23);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.txtPassword.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPassword.Icon = null;
            this.txtPassword.Location = new System.Drawing.Point(109, 100);
            this.txtPassword.Multiline = false;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.txtPassword.PasswordChar = true;
            this.txtPassword.PlaceHolderColor = System.Drawing.Color.Gray;
            this.txtPassword.PlaceHolderText = "";
            this.txtPassword.ReadOnly = false;
            this.txtPassword.SelectionStart = 0;
            this.txtPassword.Size = new System.Drawing.Size(228, 25);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // VcpConnectionConfigurator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtIpAddress);
            this.Controls.Add(this.lblIpAddress);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Name = "VcpConnectionConfigurator";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(340, 130);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox txtName;
        private System.Windows.Forms.Label lblIpAddress;
        private SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox txtIpAddress;
        private System.Windows.Forms.Label lblPort;
        private SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox txtPort;
        private System.Windows.Forms.Label lblPassword;
        private SuchByte.MacroDeck.GUI.CustomControls.RoundedTextBox txtPassword;
    }
}