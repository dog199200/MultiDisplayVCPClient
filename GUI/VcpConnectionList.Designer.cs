using System.Windows.Forms;
using SuchByte.MacroDeck.GUI.CustomControls;

namespace MultiDisplayVCPClient.GUI
{
    partial class VcpConnectionList
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnManage = new SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.mainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSize = false;

            // --- MODIFICATION 1 ---
            // Removed the border from the inner panel
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // --- END MODIFICATION ---

            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(334, 1);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // btnManage
            // 
            this.btnManage.BorderRadius = 8;
            this.btnManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnManage.ForeColor = System.Drawing.Color.White;
            this.btnManage.HoverColor = System.Drawing.Color.Empty;
            this.btnManage.Icon = null;
            this.btnManage.Location = new System.Drawing.Point(8, 10);
            this.btnManage.Margin = new System.Windows.Forms.Padding(8, 3, 8, 8);
            this.btnManage.Name = "btnManage";
            this.btnManage.Progress = 0;
            this.btnManage.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            this.btnManage.Size = new System.Drawing.Size(324, 29);
            this.btnManage.TabIndex = 1;
            this.btnManage.Text = "Manage";
            this.btnManage.UseWindowsAccentColor = true;
            this.btnManage.UseVisualStyleBackColor = true;
            // 
            // mainLayout
            // 
            this.mainLayout.AutoSize = true;
            this.mainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            // --- MODIFICATION 2 ---
            // Added the border to the outer panel
            this.mainLayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // --- END MODIFICATION ---

            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.mainLayout.Controls.Add(this.btnManage, 0, 1);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainLayout.Size = new System.Drawing.Size(340, 48);
            this.mainLayout.TabIndex = 1;
            // 
            // VcpConnectionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(340, 48);
            this.Controls.Add(this.mainLayout);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "VcpConnectionList";
            this.mainLayout.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private SuchByte.MacroDeck.GUI.CustomControls.ButtonPrimary btnManage;
    }
}