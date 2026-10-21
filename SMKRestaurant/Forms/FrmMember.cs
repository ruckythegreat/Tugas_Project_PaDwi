using SMKRestaurant.Data;
using SMKRestaurant.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    public partial class FrmMember : Form
    {
        private readonly MemberRepository _members = new();
        private int selectedId = 0;

        public FrmMember()
        {
            InitializeComponent();
            LoadMember();

            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private void LoadMember()
        {
            try
            {
                dgvMember.DataSource = _members.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data member:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetEditMode(true);
            txtName.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih member yang ingin diedit terlebih dahulu!");
                return;
            }
            SetEditMode(true);
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih data terlebih dahulu.");
                return;
            }

            if (MessageBox.Show("Apakah yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _members.Delete(selectedId);
                    MessageBox.Show("Data berhasil dihapus.");
                    LoadMember();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus member:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtHandphone.Text))
            {
                MessageBox.Show("Semua data wajib diisi!");
                return;
            }

            var input = new MemberInput
            {
                Name = txtName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Handphone = txtHandphone.Text.Trim(),
                JoinDate = dtpJoinDate.Value.Date
            };

            try
            {
                if (selectedId == 0) _members.Insert(input);
                else _members.Update(selectedId, input);

                MessageBox.Show(selectedId == 0 ? "Data berhasil ditambah." : "Data berhasil diperbarui.");
                LoadMember();
                ClearForm();
                SetEditMode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan member:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetEditMode(false);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            foreach (DataGridViewRow row in dgvMember.Rows)
            {
                if (row.Cells["Name"].Value != null && row.Cells["Name"].Value.ToString().ToLower().Contains(keyword))
                    row.Visible = true;
                else
                    row.Visible = false;
            }
        }

        private void dgvMember_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvMember.Rows[e.RowIndex];

            selectedId = Convert.ToInt32(row.Cells["Id"].Value);
            txtMemberId.Text = selectedId.ToString();
            txtName.Text = row.Cells["Name"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
            txtHandphone.Text = row.Cells["Handphone"].Value?.ToString() ?? "";

            if (row.Cells["JoinDate"].Value != DBNull.Value)
                dtpJoinDate.Value = Convert.ToDateTime(row.Cells["JoinDate"].Value);
        }

        private void ClearForm()
        {
            selectedId = 0;
            txtMemberId.Clear();
            txtName.Clear();
            txtEmail.Clear();
            txtHandphone.Clear();
            dtpJoinDate.Value = DateTime.Now;
        }

        private void SetEditMode(bool isEditing)
        {
            btnSave.Visible = isEditing;
            btnCancel.Visible = isEditing;
            btnInsert.Visible = !isEditing;
            btnUpdate.Visible = !isEditing;
            btnDelete.Visible = !isEditing;
        }
    }
}