using System.Drawing;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    partial class FrmAdmin
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblWelcome;
        private Button btnOrder;
        private Button btnManageMenu;
        private Button btnManageMember;
        private Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblWelcome = new Label();
            btnOrder = new Button();
            btnManageMenu = new Button();
            btnManageMember = new Button();
            btnLogout = new Button();

            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.Location = new Point(90, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(200, 30);
            lblTitle.Text = "Admin Navigation";

            // lblWelcome
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblWelcome.Location = new Point(40, 70);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(160, 19);
            lblWelcome.Text = "Welcome, [Admin Name]";

            // btnOrder
            btnOrder.Location = new Point(90, 110);
            btnOrder.Size = new Size(200, 35);
            btnOrder.Text = "Order";
            btnOrder.UseVisualStyleBackColor = true;
            btnOrder.Click += btnOrder_Click;

            // btnManageMenu
            btnManageMenu.Location = new Point(90, 155);
            btnManageMenu.Size = new Size(200, 35);
            btnManageMenu.Text = "Manage Menu";
            btnManageMenu.UseVisualStyleBackColor = true;
            btnManageMenu.Click += btnManageMenu_Click;

            // btnManageMember
            btnManageMember.Location = new Point(90, 200);
            btnManageMember.Size = new Size(200, 35);
            btnManageMember.Text = "Manage Member";
            btnManageMember.UseVisualStyleBackColor = true;
            btnManageMember.Click += btnManageMember_Click;

            // btnLogout
            btnLogout.Location = new Point(90, 245);
            btnLogout.Size = new Size(200, 35);
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;

            // FrmAdmin
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 320);
            Controls.Add(lblTitle);
            Controls.Add(lblWelcome);
            Controls.Add(btnOrder);
            Controls.Add(btnManageMenu);
            Controls.Add(btnManageMember);
            Controls.Add(btnLogout);
            Name = "FrmAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Admin";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}