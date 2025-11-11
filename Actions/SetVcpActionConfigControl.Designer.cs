namespace MultiDisplayVCPClient.Actions
{
    partial class SetVcpActionConfigControl
    {
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblConnection = new System.Windows.Forms.Label();
            this.comboConnections = new SuchByte.MacroDeck.GUI.CustomControls.RoundedComboBox();
            this.lblMonitor = new System.Windows.Forms.Label();
            this.comboMonitors = new SuchByte.MacroDeck.GUI.CustomControls.RoundedComboBox();
            this.lblSetting = new System.Windows.Forms.Label();
            this.comboSettings = new SuchByte.MacroDeck.GUI.CustomControls.RoundedComboBox();
            this.lblValue = new System.Windows.Forms.Label();
            this.numericValue = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lblConnection
            // 
            this.lblConnection.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblConnection.Location = new System.Drawing.Point(3, 20);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(120, 29);
            this.lblConnection.TabIndex = 0;
            this.lblConnection.Text = "Connection:";
            this.lblConnection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboConnections
            // 
            this.comboConnections.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.comboConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboConnections.Enabled = false;
            this.comboConnections.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.comboConnections.Icon = null;
            this.comboConnections.Location = new System.Drawing.Point(129, 20);
            this.comboConnections.Name = "comboConnections";
            this.comboConnections.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.comboConnections.SelectedIndex = -1;
            this.comboConnections.SelectedItem = null;
            this.comboConnections.Size = new System.Drawing.Size(320, 29);
            this.comboConnections.TabIndex = 1;
            this.comboConnections.SelectedIndexChanged += new System.EventHandler(this.comboConnections_SelectedIndexChanged);
            this.comboConnections.Enter += new System.EventHandler(this.comboConnections_Enter);

            // 
            // lblMonitor
            // 
            this.lblMonitor.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMonitor.Location = new System.Drawing.Point(3, 60);
            this.lblMonitor.Name = "lblMonitor";
            this.lblMonitor.Size = new System.Drawing.Size(120, 29);
            this.lblMonitor.TabIndex = 2;
            this.lblMonitor.Text = "Monitor:";
            this.lblMonitor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboMonitors
            // 
            this.comboMonitors.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.comboMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMonitors.Enabled = false;
            this.comboMonitors.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.comboMonitors.Icon = null;
            this.comboMonitors.Location = new System.Drawing.Point(129, 60);
            this.comboMonitors.Name = "comboMonitors";
            this.comboMonitors.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.comboMonitors.SelectedIndex = -1;
            this.comboMonitors.SelectedItem = null;
            this.comboMonitors.Size = new System.Drawing.Size(320, 29);
            this.comboMonitors.TabIndex = 3;
            this.comboMonitors.SelectedIndexChanged += new System.EventHandler(this.comboMonitors_SelectedIndexChanged);
            this.comboMonitors.Enter += new System.EventHandler(this.comboMonitors_Enter);
            // 
            // lblSetting
            // 
            this.lblSetting.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSetting.Location = new System.Drawing.Point(3, 100);
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(120, 29);
            this.lblSetting.TabIndex = 4;
            this.lblSetting.Text = "Setting:";
            this.lblSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboSettings
            // 
            this.comboSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.comboSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSettings.Enabled = false;
            this.comboSettings.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.comboSettings.Icon = null;
            this.comboSettings.Location = new System.Drawing.Point(129, 100);
            this.comboSettings.Name = "comboSettings";
            this.comboSettings.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);
            this.comboSettings.SelectedIndex = -1;
            this.comboSettings.SelectedItem = null;
            this.comboSettings.Size = new System.Drawing.Size(320, 29);
            this.comboSettings.TabIndex = 5;
            this.comboSettings.SelectedIndexChanged += new System.EventHandler(this.comboSettings_SelectedIndexChanged);
            this.comboSettings.Enter += new System.EventHandler(this.comboSettings_Enter);
            // 
            // lblValue
            // 
            this.lblValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblValue.Location = new System.Drawing.Point(3, 140);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(120, 29);
            this.lblValue.TabIndex = 6;
            this.lblValue.Text = "Value:";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericValue
            // 
            this.numericValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.numericValue.Enabled = false;
            this.numericValue.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.numericValue.ForeColor = System.Drawing.Color.White;
            this.numericValue.Location = new System.Drawing.Point(129, 142);
            this.numericValue.Name = "numericValue";
            this.numericValue.Size = new System.Drawing.Size(120, 27);
            this.numericValue.TabIndex = 7;
            this.numericValue.ValueChanged += new System.EventHandler(this.Value_Changed);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.toolTip1.ForeColor = System.Drawing.Color.White;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "VCP Info";
            // 
            // SetVcpActionConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numericValue);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.comboSettings);
            this.Controls.Add(this.lblSetting);
            this.Controls.Add(this.comboMonitors);
            this.Controls.Add(this.lblMonitor);
            this.Controls.Add(this.comboConnections);
            this.Controls.Add(this.lblConnection);
            this.Name = "SetVcpActionConfigControl";
            this.Size = new System.Drawing.Size(500, 200);
            this.Load += new System.EventHandler(this.SetVcpActionConfigControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericValue)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblConnection;
        private SuchByte.MacroDeck.GUI.CustomControls.RoundedComboBox comboConnections;
        private System.Windows.Forms.Label lblMonitor;
        private SuchByte.MacroDeck.GUI.CustomControls.RoundedComboBox comboMonitors;
        private System.Windows.Forms.Label lblSetting;
        private SuchByte.MacroDeck.GUI.CustomControls.RoundedComboBox comboSettings;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.NumericUpDown numericValue;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}