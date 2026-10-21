
using SMKRestaurant.Data;
using System;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    public partial class FrmLogin : Form
    {
        private readonly EmployeeRepository _employees = new();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            // Validasi input
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Semua field wajib diisi!",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            try
            {
                // Verifikasi akun melalui EmployeeRepository
                var employee = _employees.Authenticate(email, password);

                if (employee == null)
                {
                    MessageBox.Show(
                        "Maaf, Email atau Password salah!",
                        "Login Gagal",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }

                // Tentukan form berdasarkan posisi pegawai
                Form? nextForm = null;

                if (string.Equals(
                    employee.Position,
                    "admin",
                    StringComparison.OrdinalIgnoreCase))
                {
                    nextForm = new FrmAdmin(
                        employee.Id,
                        employee.Name
                    );
                }
                else if (string.Equals(
                    employee.Position,
                    "cashier",
                    StringComparison.OrdinalIgnoreCase))
                {
                    nextForm = new FrmCashier(
                        employee.Id,
                        employee.Name
                    );
                }

                // Posisi tidak dikenali
                if (nextForm == null)
                {
                    MessageBox.Show(
                        "Position tidak dikenal.",
                        "Login Gagal",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                // Tampilkan form tujuan
                Hide();

                nextForm.FormClosed += (_, _) => Close();
                nextForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Login gagal karena terjadi kesalahan:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
