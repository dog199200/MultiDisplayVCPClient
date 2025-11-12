using SuchByte.MacroDeck.GUI.CustomControls;

namespace MultiDisplayVCPClient.GUI
{
    partial class PluginConfig
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
        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            formLayout = new System.Windows.Forms.TableLayoutPanel();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            btnOk = new ButtonPrimary();
            btnCleanUp = new ButtonPrimary();
            btnNewConnection = new ButtonPrimary();
            connectionsPanel = new System.Windows.Forms.FlowLayoutPanel();

            // --- NEW: Add a panel for left-aligned buttons ---
            leftButtonPanel = new System.Windows.Forms.FlowLayoutPanel();

            formLayout.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();

            // --- NEW: Add leftButtonPanel ---
            leftButtonPanel.SuspendLayout();

            SuspendLayout();
            // 
            // toolTip1
            // 
            toolTip1.AutomaticDelay = 300;
            toolTip1.AutoPopDelay = int.MaxValue;
            toolTip1.InitialDelay = 300;
            toolTip1.IsBalloon = true;
            toolTip1.ReshowDelay = 0;
            // 
            // formLayout
            // 
            // --- MODIFIED: Changed to 2 columns ---
            formLayout.ColumnCount = 2;
            formLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            formLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            // --- END MODIFICATION ---

            formLayout.Controls.Add(flowLayoutPanel1, 1, 1);
            formLayout.Controls.Add(connectionsPanel, 0, 0);

            // --- NEW: Add leftButtonPanel to (0,1) ---
            formLayout.Controls.Add(leftButtonPanel, 0, 1);

            // --- MODIFIED: Make connectionsPanel span 2 columns ---
            formLayout.SetColumnSpan(connectionsPanel, 2);
            // --- END MODIFICATION ---

            formLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            formLayout.Location = new System.Drawing.Point(1, 1);
            formLayout.Name = "formLayout";
            formLayout.RowCount = 2;
            formLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            formLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            formLayout.Size = new System.Drawing.Size(480, 398);
            formLayout.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            // --- MODIFIED: Only contains OK button ---
            flowLayoutPanel1.Controls.Add(btnOk);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new System.Drawing.Point(394, 361);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(83, 34);
            flowLayoutPanel1.TabIndex = 11;
            // 
            // btnOk
            // 
            btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOk.BorderRadius = 8;
            btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOk.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnOk.ForeColor = System.Drawing.Color.White;
            btnOk.HoverColor = System.Drawing.Color.FromArgb(0, 89, 184);
            btnOk.Icon = null;
            btnOk.Location = new System.Drawing.Point(5, 3);
            btnOk.Name = "btnOk";
            btnOk.Progress = 0;
            btnOk.ProgressColor = System.Drawing.Color.FromArgb(0, 46, 94);
            btnOk.Size = new System.Drawing.Size(75, 25);
            btnOk.TabIndex = 7;
            btnOk.Text = "Ok";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.UseWindowsAccentColor = true;
            btnOk.Click += BtnOk_Click;
            // 
            // btnCleanUp
            // 
            btnCleanUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnCleanUp.BorderRadius = 8;
            btnCleanUp.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCleanUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCleanUp.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnCleanUp.ForeColor = System.Drawing.Color.White;
            btnCleanUp.HoverColor = System.Drawing.Color.FromArgb(0, 89, 184);
            btnCleanUp.Icon = null;
            btnCleanUp.Location = new System.Drawing.Point(3, 3);
            btnCleanUp.Name = "btnCleanUp";
            btnCleanUp.Progress = 0;
            btnCleanUp.ProgressColor = System.Drawing.Color.FromArgb(0, 46, 94);
            btnCleanUp.Size = new System.Drawing.Size(125, 25);
            btnCleanUp.TabIndex = 8;
            btnCleanUp.Text = "Clean Up Variables";
            btnCleanUp.UseVisualStyleBackColor = false;
            btnCleanUp.UseWindowsAccentColor = false;
            btnCleanUp.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            // 
            // btnNewConnection
            // 
            btnNewConnection.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnNewConnection.BorderRadius = 8;
            btnNewConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            btnNewConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNewConnection.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnNewConnection.ForeColor = System.Drawing.Color.White;
            btnNewConnection.HoverColor = System.Drawing.Color.FromArgb(0, 89, 184);
            btnNewConnection.Icon = null;
            btnNewConnection.Location = new System.Drawing.Point(134, 3);
            btnNewConnection.Name = "btnNewConnection";
            btnNewConnection.Progress = 0;
            btnNewConnection.ProgressColor = System.Drawing.Color.FromArgb(0, 46, 94);
            btnNewConnection.Size = new System.Drawing.Size(125, 25);
            btnNewConnection.TabIndex = 9;
            btnNewConnection.Text = "New Connection";
            btnNewConnection.UseVisualStyleBackColor = false;
            btnNewConnection.UseWindowsAccentColor = true;
            btnNewConnection.Click += BtnAdd_Click;
            // 
            // connectionsPanel
            // 
            connectionsPanel.AutoScroll = true;
            // --- MODIFIED: Span 2 columns ---
            formLayout.SetColumnSpan(this.connectionsPanel, 2);
            connectionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            connectionsPanel.Location = new System.Drawing.Point(3, 3);
            connectionsPanel.Name = "connectionsPanel";
            connectionsPanel.Padding = new System.Windows.Forms.Padding(10);
            connectionsPanel.Size = new System.Drawing.Size(474, 352);
            connectionsPanel.TabIndex = 12;
            // 
            // --- NEW: leftButtonPanel definition ---
            // 
            leftButtonPanel.Controls.Add(btnCleanUp);
            leftButtonPanel.Controls.Add(btnNewConnection);
            leftButtonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            leftButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            leftButtonPanel.Location = new System.Drawing.Point(3, 361);
            leftButtonPanel.Name = "leftButtonPanel";
            leftButtonPanel.Size = new System.Drawing.Size(385, 34);
            leftButtonPanel.TabIndex = 13;
            // 
            // PluginConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(482, 400);
            Controls.Add(formLayout);
            Location = new System.Drawing.Point(0, 0);
            Name = "PluginConfig";
            Text = "VCP Server Configuration";
            Controls.SetChildIndex(formLayout, 0);
            formLayout.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);

            // --- NEW: Add leftButtonPanel ---
            leftButtonPanel.ResumeLayout(false);

            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel formLayout;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ButtonPrimary btnOk;
        private ButtonPrimary btnCleanUp;
        private ButtonPrimary btnNewConnection;
        private System.Windows.Forms.FlowLayoutPanel connectionsPanel;

        // --- NEW: Add leftButtonPanel ---
        private System.Windows.Forms.FlowLayoutPanel leftButtonPanel;
    }
}