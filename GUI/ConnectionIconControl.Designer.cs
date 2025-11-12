namespace MultiDisplayVCPClient.GUI
{
    partial class ConnectionIconControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnIcon;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblName;

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
            btnIcon = new Button();
            btnDelete = new Button();
            lblName = new Label();
            SuspendLayout();
            // 
            // btnIcon
            // 
            btnIcon.BackColor = Color.FromArgb(65, 65, 65);
            btnIcon.Cursor = Cursors.Hand;
            btnIcon.FlatAppearance.BorderColor = Color.FromArgb(85, 85, 85);
            btnIcon.FlatStyle = FlatStyle.Flat;
            btnIcon.Image = Properties.Resources.MCC_Offline;
            btnIcon.Location = new Point(13, 5);
            btnIcon.Name = "btnIcon";
            btnIcon.Size = new Size(70, 70);
            btnIcon.TabIndex = 0;
            btnIcon.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Maroon;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(72, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(20, 19);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "X";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // lblName
            // 
            lblName.Cursor = Cursors.Hand;
            lblName.Font = new Font("Tahoma", 9F);
            lblName.ForeColor = Color.White;
            lblName.Location = new Point(0, 78);
            lblName.Name = "lblName";
            lblName.Size = new Size(95, 22);
            lblName.TabIndex = 2;
            lblName.Text = "Connection";
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ConnectionIconControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(btnDelete);
            Controls.Add(lblName);
            Controls.Add(btnIcon);
            Name = "ConnectionIconControl";
            Size = new Size(95, 100);
            ResumeLayout(false);
        }
        #endregion
    }
}