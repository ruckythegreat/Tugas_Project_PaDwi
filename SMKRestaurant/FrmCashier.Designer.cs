namespace SMKRestaurant.Forms
{
    partial class FrmCashier
    {
        private System.ComponentModel.IContainer components = null;

        private Button btnOrder;
        private Button btnPayment;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnOrder = new Button();
            btnPayment = new Button();

            SuspendLayout();

            // btnOrder
            btnOrder.Location = new Point(40, 45);
            btnOrder.Size = new Size(220, 45);
            btnOrder.Text = "Order";
            btnOrder.UseVisualStyleBackColor = true;
            btnOrder.Click += btnOrder_Click;

            // btnPayment
            btnPayment.Location = new Point(40, 105);
            btnPayment.Size = new Size(220, 45);
            btnPayment.Text = "Payment";
            btnPayment.UseVisualStyleBackColor = true;
            btnPayment.Click += btnPayment_Click;

            // FrmCashier
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 200);
            Controls.Add(btnOrder);
            Controls.Add(btnPayment);
            Name = "FrmCashier";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Cashier";

            ResumeLayout(false);
        }
    }
}