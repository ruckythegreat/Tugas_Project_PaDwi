using Microsoft.Data.SqlClient;
using System.Data;
using SMKRestaurant.Helpers;

namespace SMKRestaurant.Forms
{
    public partial class FrmMember : Form
    {
        private int selectedId = 0;

        public FrmMember()
        {
            InitializeComponent();
            dgvMember.CellClick += dgvMember_CellClick;
            LoadMember();

            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private void LoadMember()
        {
            string sql = "SELECT Id, Name, Email, Handphone, JoinDate FROM MsMember";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvMember.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            selectedId = 0;
            ClearForm();

            btnSave.Visible = true;
            btnCancel.Visible = true;
            txtName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih member yang ingin diedit terlebih dahulu!");
                return;
            }

            btnSave.Visible = true;
            btnCancel.Visible = true;
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtHandphone.Text))
            {
                MessageBox.Show("Semua data wajib diisi!");
                return;
            }

            string sql;
            if (selectedId == 0)
            {
                // INSERT DATA BARU
                sql = @"
                    INSERT INTO MsMember (Name, Email, Handphone, JoinDate)
                    VALUES (@Name, @Email, @Handphone, @JoinDate)";
            }
            else
            {
                // UPDATE DATA LAMA
                sql = @"
                    UPDATE MsMember 
                    SET Name = @Name, Email = @Email, Handphone = @Handphone, JoinDate = @JoinDate
                    WHERE Id = @Id";
            }

            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Handphone", txtHandphone.Text.Trim());
                cmd.Parameters.AddWithValue("@JoinDate", dtpJoinDate.Value.Date);

                if (selectedId != 0)
                {
                    cmd.Parameters.AddWithValue("@Id", selectedId);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show(selectedId == 0 ? "Data berhasil ditambah." : "Data berhasil diperbarui.");

            LoadMember();
            ClearForm();
            selectedId = 0;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih data terlebih dahulu.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Apakah yakin ingin menghapus data ini?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string sql = "DELETE FROM MsMember WHERE Id = @Id";

                using (SqlConnection conn = Database.GetConnection())
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", selectedId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Data berhasil dihapus.");
                LoadMember();
                ClearForm();
                selectedId = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            selectedId = 0;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private void dgvMember_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvMember.Rows[e.RowIndex];
            selectedId = Convert.ToInt32(row.Cells["Id"].Value);

            txtName.Text = row.Cells["Name"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
            txtHandphone.Text = row.Cells["Handphone"].Value?.ToString() ?? "";

            if (row.Cells["JoinDate"].Value != DBNull.Value)
            {
                dtpJoinDate.Value = Convert.ToDateTime(row.Cells["JoinDate"].Value);
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtHandphone.Clear();
            dtpJoinDate.Value = DateTime.Now;
        }
    }
}