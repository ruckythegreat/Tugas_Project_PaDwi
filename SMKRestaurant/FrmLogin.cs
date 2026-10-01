using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;

namespace SMKRestaurant.Forms
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Semua field wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"
                SELECT Id, Name, Position 
                FROM MsEmployee 
                WHERE Email = @Email AND Password = @Password";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string employeeId = reader["Id"].ToString() ?? "";
                            string position = reader["Position"].ToString() ?? "";

                            this.Hide();

                            if (position.Equals("admin", StringComparison.OrdinalIgnoreCase))
                            {
                                FrmAdmin formAdmin = new FrmAdmin();
                                formAdmin.FormClosed += (s, args) => this.Close();
                                formAdmin.Show();
                            }
                            else if (position.Equals("cashier", StringComparison.OrdinalIgnoreCase))
                            {
                                FrmCashier formCashier = new FrmCashier(employeeId);
                                formCashier.FormClosed += (s, args) => this.Close();
                                formCashier.Show();
                            }
                            else
                            {
                                MessageBox.Show("Position tidak dikenal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Maaf, Email atau Password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Koneksi Database Gagal: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}