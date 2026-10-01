namespace SMKRestaurant.Forms
{
    public partial class FrmCashier : Form
    {
        private string employeeId;

        public FrmCashier(string empId = "")
        {
            InitializeComponent();
            this.employeeId = empId;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            FrmOrder form = new FrmOrder(employeeId);
            form.ShowDialog();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            FrmPayment form = new FrmPayment();
            form.ShowDialog();
        }
    }
}