using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;
using System.Data;

namespace SMKRestaurant.Forms
{
    public class FrmMenu : Form
    {
        private int selectedId = 0;

        private TextBox txtName;
        private TextBox txtPrice;
        private TextBox txtPhoto;
        private TextBox txtCarbo;
        private TextBox txtProtein;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnCancel;

        private DataGridView dgvMenu;

        public FrmMenu()
        {
            InitializeComponent();
            LoadMenu();

            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private void InitializeComponent()
        {
            this.Text = "Menu Management";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblName = new Label();
            lblName.Text = "Nama Menu";
            lblName.Location = new Point(20, 20);
            lblName.AutoSize = true;

            txtName = new TextBox();
            txtName.Location = new Point(120, 17);
            txtName.Width = 200;

            Label lblPrice = new Label();
            lblPrice.Text = "Harga";
            lblPrice.Location = new Point(20, 60);
            lblPrice.AutoSize = true;

            txtPrice = new TextBox();
            txtPrice.Location = new Point(120, 57);
            txtPrice.Width = 200;

            Label lblPhoto = new Label();
            lblPhoto.Text = "Photo";
            lblPhoto.Location = new Point(20, 100);
            lblPhoto.AutoSize = true;

            txtPhoto = new TextBox();
            txtPhoto.Location = new Point(120, 97);
            txtPhoto.Width = 200;

            Label lblCarbo = new Label();
            lblCarbo.Text = "Carbo";
            lblCarbo.Location = new Point(350, 20);
            lblCarbo.AutoSize = true;

            txtCarbo = new TextBox();
            txtCarbo.Location = new Point(430, 17);
            txtCarbo.Width = 150;

            Label lblProtein = new Label();
            lblProtein.Text = "Protein";
            lblProtein.Location = new Point(350, 60);
            lblProtein.AutoSize = true;

            txtProtein = new TextBox();
            txtProtein.Location = new Point(430, 57);
            txtProtein.Width = 150;

            btnAdd = new Button();
            btnAdd.Text = "Tambah";
            btnAdd.Location = new Point(350, 100);
            btnAdd.Click += btnAdd_Click;

            btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(440, 100);
            btnEdit.Click += btnEdit_Click;

            btnDelete = new Button();
            btnDelete.Text = "Hapus";
            btnDelete.Location = new Point(530, 100);
            btnDelete.Click += btnDelete_Click;

            btnSave = new Button();
            btnSave.Text = "Simpan";
            btnSave.Location = new Point(620, 100);
            btnSave.Click += btnSave_Click;

            btnCancel = new Button();
            btnCancel.Text = "Batal";
            btnCancel.Location = new Point(710, 100);
            btnCancel.Click += btnCancel_Click;

            dgvMenu = new DataGridView();
            dgvMenu.Location = new Point(20, 160);
            dgvMenu.Size = new Size(840, 360);
            dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenu.MultiSelect = false;
            dgvMenu.ReadOnly = true;
            dgvMenu.AllowUserToAddRows = false;
            dgvMenu.CellClick += dgvMenu_CellClick;

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);

            this.Controls.Add(lblPrice);
            this.Controls.Add(txtPrice);

            this.Controls.Add(lblPhoto);
            this.Controls.Add(txtPhoto);

            this.Controls.Add(lblCarbo);
            this.Controls.Add(txtCarbo);

            this.Controls.Add(lblProtein);
            this.Controls.Add(txtProtein);

            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.Controls.Add(dgvMenu);
        }

        private void LoadMenu()
        {
            try
            {
                using SqlConnection conn = Database.GetConnection();

                string sql = @"
                    SELECT Id, Name, Price, Photo, Carbo, Protein
                    FROM MsMenu
                    ORDER BY Id";

                using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvMenu.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal mengambil data menu:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            ClearForm();

            btnSave.Visible = true;
            btnCancel.Visible = true;

            btnAdd.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih menu yang ingin diedit terlebih dahulu.");
                return;
            }

            btnSave.Visible = true;
            btnCancel.Visible = true;

            btnAdd.Visible = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Nama menu harus diisi.");
                return;
            }

            if (!int.TryParse(txtPrice.Text, out int price))
            {
                MessageBox.Show("Harga harus berupa angka.");
                return;
            }

            if (!int.TryParse(txtCarbo.Text, out int carbo))
            {
                MessageBox.Show("Carbo harus berupa angka.");
                return;
            }

            if (!int.TryParse(txtProtein.Text, out int protein))
            {
                MessageBox.Show("Protein harus berupa angka.");
                return;
            }

            try
            {
                using SqlConnection conn = Database.GetConnection();
                conn.Open();

                string sql;

                if (selectedId == 0)
                {
                    sql = @"
                        INSERT INTO MsMenu
                        (Name, Price, Photo, Carbo, Protein)
                        VALUES
                        (@Name, @Price, @Photo, @Carbo, @Protein)";
                }
                else
                {
                    sql = @"
                        UPDATE MsMenu
                        SET
                            Name = @Name,
                            Price = @Price,
                            Photo = @Photo,
                            Carbo = @Carbo,
                            Protein = @Protein
                        WHERE Id = @Id";
                }

                using SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Photo", txtPhoto.Text);
                cmd.Parameters.AddWithValue("@Carbo", carbo);
                cmd.Parameters.AddWithValue("@Protein", protein);

                if (selectedId != 0)
                {
                    cmd.Parameters.AddWithValue("@Id", selectedId);
                }

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data menu berhasil disimpan.");

                ClearForm();
                LoadMenu();

                btnSave.Visible = false;
                btnCancel.Visible = false;

                btnAdd.Visible = true;
                btnEdit.Visible = true;
                btnDelete.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menyimpan data:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih menu yang ingin dihapus terlebih dahulu.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Yakin ingin menghapus menu ini?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using SqlConnection conn = Database.GetConnection();
                conn.Open();

                string sql = "DELETE FROM MsMenu WHERE Id = @Id";

                using SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", selectedId);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Menu berhasil dihapus.");

                ClearForm();
                LoadMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menghapus menu:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            ClearForm();

            btnSave.Visible = false;
            btnCancel.Visible = false;

            btnAdd.Visible = true;
            btnEdit.Visible = true;
            btnDelete.Visible = true;
        }

        private void dgvMenu_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dgvMenu.Rows[e.RowIndex];

            selectedId = Convert.ToInt32(row.Cells["Id"].Value);

            txtName.Text = row.Cells["Name"].Value?.ToString() ?? "";
            txtPrice.Text = row.Cells["Price"].Value?.ToString() ?? "";
            txtPhoto.Text = row.Cells["Photo"].Value?.ToString() ?? "";
            txtCarbo.Text = row.Cells["Carbo"].Value?.ToString() ?? "";
            txtProtein.Text = row.Cells["Protein"].Value?.ToString() ?? "";
        }

        private void ClearForm()
        {
            selectedId = 0;

            txtName.Clear();
            txtPrice.Clear();
            txtPhoto.Clear();
            txtCarbo.Clear();
            txtProtein.Clear();
        }
    }
}