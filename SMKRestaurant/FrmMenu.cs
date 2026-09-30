using Microsoft.Data.SqlClient;
using System.Data;
using SMKRestaurant.Helpers;

namespace SMKRestaurant.Forms
{
    public partial class FrmMember : Form
    {
        // Store the selected member ID
        private int selectedId = 0;

        public FrmMember()
        {
            InitializeComponent();

            // Connect DataGridView event
            dgvMember.CellClick += dgvMember_CellClick;

            // Load member data when form opens
            LoadMember();

            // Hide Save and Cancel initially
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        // Load all member data
        private void LoadMember()
        {
            string sql = "SELECT * FROM MsMember";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                // Temporary storage for query results
                DataTable dt = new DataTable();

                // Execute query and fill DataTable
                da.Fill(dt);

                // Display data in DataGridView
                dgvMember.DataSource = dt;
            }
        }

        // Add new member
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Reset selected ID
            selectedId = 0;

            // Clear form
            ClearForm();

            // Show Save and Cancel
            btnSave.Visible = true;
            btnCancel.Visible = true;

            txtName.Focus();
        }

        // Save member
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check required fields
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtHandphone.Text))
            {
                MessageBox.Show("Semua data wajib diisi!");
                return;
            }

            // SQL INSERT query
            string sql = @"
                INSERT INTO MsMember
                (Name, Email, Handphone, JoinDate)
                VALUES
                (@Name, @Email, @Handphone, @JoinDate)";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Handphone", txtHandphone.Text.Trim());
                cmd.Parameters.AddWithValue("@JoinDate", dtpJoinDate.Value.Date);

                // Open database connection
                conn.Open();

                // Execute INSERT query
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Data berhasil disimpan.");

            // Refresh DataGridView
            LoadMember();

            // Clear form
            ClearForm();

            // Reset selected ID
            selectedId = 0;

            // Hide Save and Cancel
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        // Edit member
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Check if a member has been selected
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih member yang ingin diedit!");
                return;
            }

            // Show Save and Cancel
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        // Delete member
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvMember.CurrentRow == null)
            {
                MessageBox.Show("Pilih data terlebih dahulu.");
                return;
            }

            // Get selected member ID
            int id = Convert.ToInt32(
                dgvMember.CurrentRow.Cells["Id"].Value);

            // Ask for confirmation
            DialogResult result = MessageBox.Show(
                "Apakah yakin ingin menghapus data?",
                "Konfirmasi",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                string sql = "DELETE FROM MsMember WHERE Id = @Id";

                using (SqlConnection conn = Database.GetConnection())
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Add ID parameter
                    cmd.Parameters.AddWithValue("@Id", id);

                    // Open database connection
                    conn.Open();

                    // Execute DELETE query
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Data berhasil dihapus.");

                // Refresh DataGridView
                LoadMember();

                // Clear form
                ClearForm();

                // Reset selected ID
                selectedId = 0;
            }
        }

        // Cancel Add/Edit
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            ClearForm();

            // Reset selected ID
            selectedId = 0;

            // Hide Save and Cancel
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        // Select member from DataGridView
        private void dgvMember_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            // Ignore header row
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvMember.Rows[e.RowIndex];

            // Store selected member ID
            selectedId = Convert.ToInt32(
                row.Cells["Id"].Value);

            // Load selected data into form
            txtName.Text =
                row.Cells["Name"].Value?.ToString() ?? "";

            txtEmail.Text =
                row.Cells["Email"].Value?.ToString() ?? "";

            txtHandphone.Text =
                row.Cells["Handphone"].Value?.ToString() ?? "";

            if (row.Cells["JoinDate"].Value != DBNull.Value)
            {
                dtpJoinDate.Value =
                    Convert.ToDateTime(row.Cells["JoinDate"].Value);
            }
        }

        // Clear all input fields
        private void ClearForm()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtHandphone.Clear();

            // Reset date
            dtpJoinDate.Value = DateTime.Now;

            txtName.Focus();
        }
    }
}