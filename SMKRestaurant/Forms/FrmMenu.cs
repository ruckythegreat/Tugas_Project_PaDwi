using SMKRestaurant.Data;
using SMKRestaurant.Models;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    public partial class FrmMenu : Form
    {
        private readonly MenuRepository _menus = new();
        private int selectedId = 0;

        public FrmMenu()
        {
            InitializeComponent();
            LoadMenu();

            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        private void LoadMenu()
        {
            try
            {
                dgvMenu.DataSource = _menus.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data menu:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- TOMBOL AKSI ---
        private void btnInsert_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetEditMode(true);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih menu yang ingin diedit terlebih dahulu.");
                return;
            }
            SetEditMode(true);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Pilih menu yang ingin dihapus terlebih dahulu.");
                return;
            }

            var result = MessageBox.Show("Yakin ingin menghapus menu ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    _menus.Delete(selectedId);
                    MessageBox.Show("Menu berhasil dihapus.");
                    ClearForm();
                    LoadMenu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menghapus menu:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || !int.TryParse(txtPrice.Text, out int price) ||
                !int.TryParse(txtCarbo.Text, out int carbo) || !int.TryParse(txtProtein.Text, out int protein))
            {
                MessageBox.Show("Pastikan semua form terisi dengan format yang benar (Harga, Carbo, Protein berupa angka).");
                return;
            }

            var input = new MenuInput
            {
                Name = txtName.Text.Trim(),
                Price = price,
                Photo = txtPhoto.Text.Trim(),
                Carbo = carbo,
                Protein = protein
            };

            try
            {
                if (selectedId == 0) _menus.Insert(input);
                else _menus.Update(selectedId, input);

                MessageBox.Show("Data menu berhasil disimpan.");
                ClearForm();
                LoadMenu();
                SetEditMode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan data:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetEditMode(false);
        }

        // --- FITUR TAMBAHAN ---
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPhoto.Text = Path.GetFileName(ofd.FileName);
                    try { picPhoto.Image = Image.FromFile(ofd.FileName); }
                    catch { picPhoto.Image = null; }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Logika pencarian sederhana via DataGridView
            string keyword = txtSearch.Text.ToLower();
            foreach (DataGridViewRow row in dgvMenu.Rows)
            {
                if (row.Cells["Name"].Value != null && row.Cells["Name"].Value.ToString().ToLower().Contains(keyword))
                {
                    row.Visible = true;
                }
                else
                {
                    // Pastikan dgv tidak dlm mode CurrencyManager jika menyembunyikan row
                    // Jika error, gunakan filter DataSource langsung (contoh LINQ)
                    row.Visible = false; 
                }
            }
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvMenu.Rows[e.RowIndex];

            selectedId = Convert.ToInt32(row.Cells["Id"].Value);
            txtMenuId.Text = selectedId.ToString();
            txtName.Text = row.Cells["Name"].Value?.ToString() ?? "";
            txtPrice.Text = row.Cells["Price"].Value?.ToString() ?? "";
            txtPhoto.Text = row.Cells["Photo"].Value?.ToString() ?? "";
            txtCarbo.Text = row.Cells["Carbo"].Value?.ToString() ?? "";
            txtProtein.Text = row.Cells["Protein"].Value?.ToString() ?? "";

            // Coba memuat gambar jika ada file fisik di folder
            try { picPhoto.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Images", txtPhoto.Text)); }
            catch { picPhoto.Image = null; }
        }

        private void ClearForm()
        {
            selectedId = 0;
            txtMenuId.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtPhoto.Clear();
            txtCarbo.Clear();
            txtProtein.Clear();
            picPhoto.Image = null;
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

    // --- BAGIAN UI (Biasanya di FrmMenu.Designer.cs) ---
    partial class FrmMenu
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle, lblMenuId, lblName, lblPrice, lblPhoto, lblCarbo, lblProtein;
        private TextBox txtSearch, txtMenuId, txtName, txtPrice, txtPhoto, txtCarbo, txtProtein;
        private Button btnSearch, btnBrowse, btnInsert, btnUpdate, btnDelete, btnSave, btnCancel;
        private DataGridView dgvMenu;
        private PictureBox picPhoto;

        private void InitializeComponent()
        {
            this.Text = "Form Manage Menu";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            lblTitle = new Label { Text = "Form Manage Menu", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(250, 20), AutoSize = true };

            // Search
            txtSearch = new TextBox { Location = new Point(500, 60), Width = 180 };
            btnSearch = new Button { Text = "Search", Location = new Point(690, 58), Width = 70 };
            btnSearch.Click += btnSearch_Click;

            // DataGridView
            dgvMenu = new DataGridView { Location = new Point(40, 100), Size = new Size(720, 200), ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgvMenu.CellClick += dgvMenu_CellClick;

            // Inputs (Left Side)
            int labelX = 40, txtX = 120, startY = 320, gap = 30;
            
            lblMenuId = new Label { Text = "MenuId", Location = new Point(labelX, startY), AutoSize = true };
            txtMenuId = new TextBox { Location = new Point(txtX, startY), Width = 200, ReadOnly = true }; // Readonly

            lblName = new Label { Text = "Name", Location = new Point(labelX, startY += gap), AutoSize = true };
            txtName = new TextBox { Location = new Point(txtX, startY), Width = 200 };

            lblPrice = new Label { Text = "Price", Location = new Point(labelX, startY += gap), AutoSize = true };
            txtPrice = new TextBox { Location = new Point(txtX, startY), Width = 200 };

            lblPhoto = new Label { Text = "Photo", Location = new Point(labelX, startY += gap), AutoSize = true };
            txtPhoto = new TextBox { Location = new Point(txtX, startY), Width = 150 };
            btnBrowse = new Button { Text = "...", Location = new Point(txtX + 160, startY), Width = 40 };
            btnBrowse.Click += btnBrowse_Click;

            lblCarbo = new Label { Text = "Carbo", Location = new Point(labelX, startY += gap), AutoSize = true };
            txtCarbo = new TextBox { Location = new Point(txtX, startY), Width = 200 };

            lblProtein = new Label { Text = "Protein", Location = new Point(labelX, startY += gap), AutoSize = true };
            txtProtein = new TextBox { Location = new Point(txtX, startY), Width = 200 };

            // Photo Preview (Right Side)
            picPhoto = new PictureBox { Location = new Point(450, 320), Size = new Size(200, 150), BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom };

            // Bottom Buttons
            btnInsert = new Button { Text = "Insert", Location = new Point(250, 500), Width = 90 };
            btnInsert.Click += btnInsert_Click;

            btnUpdate = new Button { Text = "Update", Location = new Point(350, 500), Width = 90 };
            btnUpdate.Click += btnUpdate_Click;

            btnDelete = new Button { Text = "Delete", Location = new Point(450, 500), Width = 90 };
            btnDelete.Click += btnDelete_Click;

            btnSave = new Button { Text = "Save", Location = new Point(550, 500), Width = 90 };
            btnSave.Click += btnSave_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(650, 500), Width = 90 };
            btnCancel.Click += btnCancel_Click;

            // Add Controls
            this.Controls.Add(lblTitle); this.Controls.Add(txtSearch); this.Controls.Add(btnSearch); this.Controls.Add(dgvMenu);
            this.Controls.Add(lblMenuId); this.Controls.Add(txtMenuId); this.Controls.Add(lblName); this.Controls.Add(txtName);
            this.Controls.Add(lblPrice); this.Controls.Add(txtPrice); this.Controls.Add(lblPhoto); this.Controls.Add(txtPhoto);
            this.Controls.Add(btnBrowse); this.Controls.Add(lblCarbo); this.Controls.Add(txtCarbo); this.Controls.Add(lblProtein);
            this.Controls.Add(txtProtein); this.Controls.Add(picPhoto);
            this.Controls.Add(btnInsert); this.Controls.Add(btnUpdate); this.Controls.Add(btnDelete); this.Controls.Add(btnSave); this.Controls.Add(btnCancel);
        }
    }
}