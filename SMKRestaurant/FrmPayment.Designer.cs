namespace SMKRestaurant.Forms
{
    partial class FrmPayment
    {
        private System.ComponentModel.IContainer components = null;

        private ComboBox cmbOrder;
        private ComboBox cmbPaymentType;
        private Label lblTotal;
        private TextBox txtCash;
        private Label lblChange;
        private Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            cmbOrder = new ComboBox();
            cmbPaymentType = new ComboBox();
            lblTotal = new Label();
            txtCash = new TextBox();
            lblChange = new Label();
            btnSave = new Button();

            SuspendLayout();

            // cmbOrder
            cmbOrder.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrder.Location = new Point(30, 30);
            cmbOrder.Size = new Size(300, 27);
            cmbOrder.SelectedIndexChanged += cmbOrder_SelectedIndexChanged;

            // cmbPaymentType
            cmbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentType.Location = new Point(30, 75);
            cmbPaymentType.Size = new Size(300, 27);

            // lblTotal
            lblTotal.Location = new Point(30, 120);
            lblTotal.Size = new Size(300, 30);
            lblTotal.Text = "0";

            // txtCash
            txtCash.Location = new Point(30, 165);
            txtCash.Size = new Size(300, 27);
            txtCash.PlaceholderText = "Nominal Cash";

            // lblChange
            lblChange.Location = new Point(30, 210);
            lblChange.Size = new Size(300, 30);
            lblChange.Text = "0";

            // btnSave
            btnSave.Location = new Point(30, 255);
            btnSave.Size = new Size(300, 40);
            btnSave.Text = "Bayar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;

            // FrmPayment
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(370, 340);
            Controls.Add(cmbOrder);
            Controls.Add(cmbPaymentType);
            Controls.Add(lblTotal);
            Controls.Add(txtCash);
            Controls.Add(lblChange);
            Controls.Add(btnSave);
            Name = "FrmPayment";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Payment";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
