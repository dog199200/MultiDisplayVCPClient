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
            repeatingLayout = new System.Windows.Forms.TableLayoutPanel();
            btnAdd = new ButtonPrimary();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            btnOk = new ButtonPrimary();

            // --- ADD THIS ---
            btnCleanUp = new ButtonPrimary();
            // --- END ADD ---

            formLayout.SuspendLayout();
            repeatingLayout.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
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
            formLayout.AutoSize = true;
            formLayout.ColumnCount = 1;
            formLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            formLayout.Controls.Add(repeatingLayout, 0, 0);
            formLayout.Controls.Add(btnAdd, 0, 1);
            formLayout.Controls.Add(flowLayoutPanel1, 0, 2);
            formLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            formLayout.Location = new System.Drawing.Point(1, 1);
            formLayout.MaximumSize = new System.Drawing.Size(0, 600);
            formLayout.Name = "formLayout";
            formLayout.RowCount = 3;
            formLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F)); // Row 0
            formLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));  // Row 1
            formLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));  // Row 2
            formLayout.TabIndex = 7;
            // 
            // repeatingLayout
            // 
            repeatingLayout.AutoScroll = true;
            repeatingLayout.AutoSize = true;
            repeatingLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            repeatingLayout.ColumnCount = 2;
            repeatingLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            repeatingLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            repeatingLayout.Dock = System.Windows.Forms.DockStyle.Top;
            repeatingLayout.Location = new System.Drawing.Point(3, 3); // Changed Location
            repeatingLayout.MaximumSize = new System.Drawing.Size(0, 600);
            repeatingLayout.Name = "repeatingLayout";
            repeatingLayout.Padding = new System.Windows.Forms.Padding(3);
            repeatingLayout.RowCount = 0;
            repeatingLayout.TabIndex = 7;
            repeatingLayout.CellPaint += RepeatingLayout_CellPaint;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnAdd.BorderRadius = 8;
            btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(184)))));
            btnAdd.Icon = null;
            btnAdd.Location = new System.Drawing.Point(382, 321); // Location will be auto-managed by layout
            btnAdd.Name = "btnAdd";
            btnAdd.Progress = 0;
            btnAdd.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            btnAdd.Size = new System.Drawing.Size(95, 25);
            btnAdd.TabIndex = 8;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.UseWindowsAccentColor = true;
            btnAdd.Click += BtnAdd_Click;
            // 
            // flowLayoutPanel1
            // 
            // --- MODIFICATION: Added btnCleanUp ---
            flowLayoutPanel1.Controls.Add(btnOk);
            flowLayoutPanel1.Controls.Add(btnCleanUp);
            // --- END MODIFICATION ---
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new System.Drawing.Point(3, 361); // Location will be auto-managed
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(474, 34);
            flowLayoutPanel1.TabIndex = 11;
            // 
            // btnOk
            // 
            btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnOk.BorderRadius = 8;
            btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOk.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnOk.ForeColor = System.Drawing.Color.White;
            btnOk.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(184)))));
            btnOk.Icon = null;
            btnOk.Location = new System.Drawing.Point(396, 3);
            btnOk.Name = "btnOk";
            btnOk.Progress = 0;
            btnOk.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            btnOk.Size = new System.Drawing.Size(75, 25);
            btnOk.TabIndex = 7;
            btnOk.Text = "Ok";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.UseWindowsAccentColor = true;
            btnOk.Click += BtnOk_Click;
            // 
            // --- ADD THIS ---
            // btnCleanUp
            // 
            btnCleanUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnCleanUp.BorderRadius = 8;
            btnCleanUp.Cursor = System.Windows.Forms.Cursors.Hand;
            btnCleanUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCleanUp.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnCleanUp.ForeColor = System.Drawing.Color.White;
            btnCleanUp.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(184)))));
            btnCleanUp.Icon = null;
            btnCleanUp.Location = new System.Drawing.Point(265, 3);
            btnCleanUp.Name = "btnCleanUp";
            btnCleanUp.Progress = 0;
            btnCleanUp.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(46)))), ((int)(((byte)(94)))));
            btnCleanUp.Size = new System.Drawing.Size(125, 25);
            btnCleanUp.TabIndex = 8;
            btnCleanUp.Text = "Clean Up Variables";
            btnCleanUp.UseVisualStyleBackColor = false;
            btnCleanUp.UseWindowsAccentColor = false; // Use a neutral color
            btnCleanUp.BackColor = System.Drawing.Color.FromArgb(65, 65, 65);
            // --- END ADD ---
            // 
            // PluginConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoScroll = true;
            AutoScrollMinSize = new System.Drawing.Size(0, 150);
            AutoSize = true;
            ClientSize = new System.Drawing.Size(482, 400);
            Controls.Add(formLayout);
            Location = new System.Drawing.Point(0, 0);
            MaximumSize = new System.Drawing.Size(1000, 400);
            Name = "PluginConfig";
            Text = "VCP Server Configuration";
            Controls.SetChildIndex(formLayout, 0);
            formLayout.ResumeLayout(false);
            formLayout.PerformLayout();
            repeatingLayout.ResumeLayout(false);
            repeatingLayout.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TableLayoutPanel formLayout;
        private System.Windows.Forms.TableLayoutPanel repeatingLayout;
        private ButtonPrimary btnAdd;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ButtonPrimary btnOk;

        // --- ADD THIS ---
        private ButtonPrimary btnCleanUp;
        // --- END ADD ---
    }
}