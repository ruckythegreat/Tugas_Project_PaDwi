namespace SMKRestaurant.Forms
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();

            SuspendLayout();

            // txtEmail
            txtEmail.Location = new Point(40, 40);
            txtEmail.Size = new Size(280, 27);
            txtEmail.PlaceholderText = "Email";

            // txtPassword
            txtPassword.Location = new Point(40, 85);
            txtPassword.Size = new Size(280, 27);
            txtPassword.PlaceholderText = "Password";
            txtPassword.UseSystemPasswordChar = true;

            // btnLogin
            btnLogin.Location = new Point(40, 130);
            btnLogin.Size = new Size(280, 35);
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;

            // FrmLogin
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(360, 210);
            Controls.Add(txtEmail);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Login";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}