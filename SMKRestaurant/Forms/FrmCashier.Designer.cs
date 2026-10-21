using System.Drawing;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    partial class FrmCashier
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblWelcome;
        private Button btnOrder;
        private Button btnPayment;
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
            btnPayment = new Button();
            btnLogout = new Button();

            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(80, 20);
            lblTitle.Text = "Cashier Navigation";

            // lblWelcome
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblWelcome.Location = new Point(40, 70);
            lblWelcome.Text = "Welcome, [Cashier Name]";

            // btnOrder
            btnOrder.Location = new Point(90, 110);
            btnOrder.Size = new Size(200, 35);
            btnOrder.Text = "Order";
            btnOrder.UseVisualStyleBackColor = true;
            btnOrder.Click += btnOrder_Click;

            // btnPayment
            btnPayment.Location = new Point(90, 155);
            btnPayment.Size = new Size(200, 35);
            btnPayment.Text = "Payment";
            btnPayment.UseVisualStyleBackColor = true;
            btnPayment.Click += btnPayment_Click;

            // btnLogout
            btnLogout.Location = new Point(90, 200);
            btnLogout.Size = new Size(200, 35);
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;

            // FrmCashier
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(380, 270);
            Controls.Add(lblTitle);
            Controls.Add(lblWelcome);
            Controls.Add(btnOrder);
            Controls.Add(btnPayment);
            Controls.Add(btnLogout);
            Name = "FrmCashier";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Cashier";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}