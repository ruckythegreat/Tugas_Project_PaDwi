using System.Drawing;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    partial class FrmPayment
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle, lblOrderId, lblTotalAmount, lblPaymentType, lblCardNumber, lblBankName;
        private ComboBox cbOrderId, cbPaymentType, cbBankName;
        private TextBox txtCardNumber;
        private DataGridView dgvOrderDetail;
        private Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblOrderId = new Label();
            cbOrderId = new ComboBox();
            dgvOrderDetail = new DataGridView();
            lblTotalAmount = new Label();
            lblPaymentType = new Label();
            cbPaymentType = new ComboBox();
            lblCardNumber = new Label();
            txtCardNumber = new TextBox();
            lblBankName = new Label();
            cbBankName = new ComboBox();
            btnSave = new Button();

            ((System.ComponentModel.ISupportInitialize)(dgvOrderDetail)).BeginInit();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(220, 20);
            lblTitle.Text = "Form Payment";

            // lblOrderId
            lblOrderId.AutoSize = true;
            lblOrderId.Location = new Point(150, 70);
            lblOrderId.Text = "OrderId";

            // cbOrderId
            cbOrderId.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOrderId.Location = new Point(220, 67);
            cbOrderId.Size = new Size(150, 23);
            cbOrderId.SelectedIndexChanged += new System.EventHandler(cbOrderId_SelectedIndexChanged);

            // dgvOrderDetail
            dgvOrderDetail.AllowUserToAddRows = false;
            dgvOrderDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrderDetail.Location = new Point(40, 110);
            dgvOrderDetail.ReadOnly = true;
            dgvOrderDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrderDetail.Size = new Size(500, 150);

            // lblTotalAmount
            lblTotalAmount.AutoSize = true;
            lblTotalAmount.Font = new Font("Segoe UI", 10F);
            lblTotalAmount.Location = new Point(400, 270);
            lblTotalAmount.Text = "Total : 0";

            // lblPaymentType
            lblPaymentType.AutoSize = true;
            lblPaymentType.Location = new Point(140, 310);
            lblPaymentType.Text = "Payment Type";

            // cbPaymentType
            cbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPaymentType.Location = new Point(240, 307);
            cbPaymentType.Size = new Size(150, 23);
            cbPaymentType.SelectedIndexChanged += new System.EventHandler(cbPaymentType_SelectedIndexChanged);

            // lblCardNumber
            lblCardNumber.AutoSize = true;
            lblCardNumber.Location = new Point(140, 345);
            lblCardNumber.Text = "Card Number";

            // txtCardNumber
            txtCardNumber.Location = new Point(240, 342);
            txtCardNumber.Size = new Size(150, 23);

            // lblBankName
            lblBankName.AutoSize = true;
            lblBankName.Location = new Point(140, 380);
            lblBankName.Text = "Bank Name";

            // cbBankName
            cbBankName.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBankName.Location = new Point(240, 377);
            cbBankName.Size = new Size(150, 23);

            // btnSave
            btnSave.Location = new Point(240, 415);
            btnSave.Size = new Size(100, 30);
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new System.EventHandler(btnSave_Click);

            // FrmPayment
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 500);
            Controls.Add(lblTitle);
            Controls.Add(lblOrderId);
            Controls.Add(cbOrderId);
            Controls.Add(dgvOrderDetail);
            Controls.Add(lblTotalAmount);
            Controls.Add(lblPaymentType);
            Controls.Add(cbPaymentType);
            Controls.Add(lblCardNumber);
            Controls.Add(txtCardNumber);
            Controls.Add(lblBankName);
            Controls.Add(cbBankName);
            Controls.Add(btnSave);
            Name = "FrmPayment";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Payment";

            ((System.ComponentModel.ISupportInitialize)(dgvOrderDetail)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}