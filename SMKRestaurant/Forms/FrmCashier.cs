using System;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    public partial class FrmCashier : Form
    {
        private readonly string employeeId;
        private readonly string cashierName;

        public FrmCashier(string empId = "", string name = "")
        {
            InitializeComponent();
            employeeId = empId;
            cashierName = name;
            lblWelcome.Text = $"Welcome, {cashierName}";
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            // Membuka Form Order saat tombol Order ditekan
            FrmOrder form = new FrmOrder(employeeId);
            form.ShowDialog();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            // Membuka Form Payment saat tombol Payment ditekan
            FrmPayment form = new FrmPayment();
            form.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}