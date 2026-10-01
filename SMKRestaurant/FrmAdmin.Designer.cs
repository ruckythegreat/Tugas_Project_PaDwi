namespace SMKRestaurant.Forms
{
    partial class FrmAdmin
    {
        private System.ComponentModel.IContainer components = null;

        private Button btnMenu;
        private Button btnMember;
        private Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnMenu = new Button();
            btnMember = new Button();
            btnLogout = new Button();

            SuspendLayout();

            // btnMenu
            btnMenu.Location = new Point(40, 40);
            btnMenu.Size = new Size(220, 40);
            btnMenu.Text = "Menu";
            btnMenu.UseVisualStyleBackColor = true;
            btnMenu.Click += btnMenu_Click;

            // btnMember
            btnMember.Location = new Point(40, 95);
            btnMember.Size = new Size(220, 40);
            btnMember.Text = "Member";
            btnMember.UseVisualStyleBackColor = true;
            btnMember.Click += btnMember_Click;

            // btnLogout
            btnLogout.Location = new Point(40, 150);
            btnLogout.Size = new Size(220, 40);
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;

            // FrmAdmin
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 230);
            Controls.Add(btnMenu);
            Controls.Add(btnMember);
            Controls.Add(btnLogout);
            Name = "FrmAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Admin";

            ResumeLayout(false);
        }
    }
}