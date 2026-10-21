
using System;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    public partial class FrmAdmin : Form
    {
        private readonly string _employeeId;
        private readonly string _adminName;

        public FrmAdmin(string empId = "", string name = "")
        {
            InitializeComponent();

            _employeeId = empId;
            _adminName = name;

            // Tampilkan nama admin pada label sambutan
            lblWelcome.Text = string.IsNullOrWhiteSpace(_adminName)
                ? "Welcome, Admin"
                : $"Welcome, {_adminName}";
        }

        private void btnOrder_Click(object? sender, EventArgs e)
        {
            using (FrmOrder form = new FrmOrder(_employeeId))
            {
                form.ShowDialog(this);
            }
        }

        private void btnManageMenu_Click(object? sender, EventArgs e)
        {
            using (FrmMenu form = new FrmMenu())
            {
                form.ShowDialog(this);
            }
        }

        private void btnManageMember_Click(object? sender, EventArgs e)
        {
            using (FrmMember form = new FrmMember())
            {
                form.ShowDialog(this);
            }
        }

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Apakah kamu yakin ingin logout?",
                "Konfirmasi Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}
