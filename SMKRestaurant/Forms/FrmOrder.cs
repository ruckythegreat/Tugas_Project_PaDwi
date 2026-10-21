
using SMKRestaurant.Data;
using SMKRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    public partial class FrmOrder : Form
    {
        private readonly string _employeeId;
        private readonly MenuRepository _menus = new();
        private readonly OrderRepository _orders = new();

        private DataTable dtCart = new();

        public FrmOrder(string empId = "")
        {
            InitializeComponent();

            _employeeId = empId;

            SetupCartTable();
            LoadMenus();
        }

        // =====================================================
        // SETUP KERANJANG
        // =====================================================
        private void SetupCartTable()
        {
            dtCart = new DataTable();

            dtCart.Columns.Add("MenuId", typeof(int));
            dtCart.Columns.Add("Name", typeof(string));
            dtCart.Columns.Add("Qty", typeof(int));
            dtCart.Columns.Add("Carbo", typeof(int));
            dtCart.Columns.Add("Protein", typeof(int));
            dtCart.Columns.Add("Price", typeof(int));
            dtCart.Columns.Add("Total", typeof(int));

            dgvCart.DataSource = dtCart;

            if (dgvCart.Columns["MenuId"] != null)
            {
                dgvCart.Columns["MenuId"]!.Visible = false;
            }

            dgvCart.AllowUserToAddRows = false;
            dgvCart.ReadOnly = true;
            dgvCart.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
        }

        // =====================================================
        // LOAD MENU
        // =====================================================
        private void LoadMenus()
        {
            try
            {
                dgvMenu.DataSource = _menus.GetAll();

                dgvMenu.ReadOnly = true;
                dgvMenu.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvMenu.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memuat daftar menu:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // PILIH MENU DARI TABEL
        // =====================================================
        private void dgvMenu_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dgvMenu.Rows[e.RowIndex];

            txtMenuName.Text =
                row.Cells["Name"].Value?.ToString() ?? "";

            txtMenuName.Tag = row.Cells["Id"].Value;

            try
            {
                string photoName =
                    row.Cells["Photo"].Value?.ToString() ?? "";

                string photoPath = System.IO.Path.Combine(
                    Application.StartupPath,
                    "Images",
                    photoName
                );

                if (System.IO.File.Exists(photoPath))
                {
                    using var stream = new System.IO.FileStream(
                        photoPath,
                        System.IO.FileMode.Open,
                        System.IO.FileAccess.Read
                    );

                    using var image = Image.FromStream(stream);

                    picPhoto.Image = new Bitmap(image);
                }
                else
                {
                    picPhoto.Image = null;
                }
            }
            catch
            {
                picPhoto.Image = null;
            }
        }

        // =====================================================
        // TAMBAH MENU KE KERANJANG
        // =====================================================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMenuName.Text) ||
                txtMenuName.Tag == null)
            {
                MessageBox.Show(
                    "Pilih menu dari tabel di atas terlebih dahulu!"
                );

                return;
            }

            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show(
                    "Jumlah (Qty) harus berupa angka lebih dari 0!"
                );

                return;
            }

            if (!int.TryParse(
                Convert.ToString(txtMenuName.Tag),
                out int menuId))
            {
                MessageBox.Show("Menu ID tidak valid.");
                return;
            }

            DataGridViewRow? selectedRow =
                dgvMenu.CurrentRow;

            if (selectedRow == null)
            {
                MessageBox.Show("Pilih menu dari tabel terlebih dahulu.");
                return;
            }

            try
            {
                int price = Convert.ToInt32(
                    selectedRow.Cells["Price"].Value
                );

                int carbo = Convert.ToInt32(
                    selectedRow.Cells["Carbo"].Value
                );

                int protein = Convert.ToInt32(
                    selectedRow.Cells["Protein"].Value
                );

                bool exists = false;

                foreach (DataRow row in dtCart.Rows)
                {
                    if (Convert.ToInt32(row["MenuId"]) == menuId)
                    {
                        int newQty =
                            Convert.ToInt32(row["Qty"]) + qty;

                        row["Qty"] = newQty;
                        row["Total"] = newQty * price;

                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    dtCart.Rows.Add(
                        menuId,
                        txtMenuName.Text,
                        qty,
                        carbo,
                        protein,
                        price,
                        price * qty
                    );
                }

                UpdateTotals();

                txtMenuName.Clear();
                txtMenuName.Tag = null;
                txtQty.Clear();
                picPhoto.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menambahkan menu:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // HAPUS MENU DARI KERANJANG
        // =====================================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null ||
                dgvCart.CurrentRow.IsNewRow)
            {
                MessageBox.Show(
                    "Pilih menu di keranjang yang ingin dihapus!"
                );

                return;
            }

            if (dgvCart.CurrentRow.DataBoundItem is DataRowView rowView)
            {
                rowView.Row.Delete();
            }

            UpdateTotals();
        }

        // =====================================================
        // HITUNG TOTAL
        // =====================================================
        private void UpdateTotals()
        {
            int totalCarbo = 0;
            int totalProtein = 0;
            int totalAmount = 0;

            foreach (DataRow row in dtCart.Rows)
            {
                totalCarbo +=
                    Convert.ToInt32(row["Carbo"]) *
                    Convert.ToInt32(row["Qty"]);

                totalProtein +=
                    Convert.ToInt32(row["Protein"]) *
                    Convert.ToInt32(row["Qty"]);

                totalAmount +=
                    Convert.ToInt32(row["Total"]);
            }

            lblCarbo.Text = $"Carbo: {totalCarbo}";
            lblProtein.Text = $"Protein: {totalProtein}";
            lblTotal.Text = $"Total: {totalAmount:N0}";
        }

        // =====================================================
        // DIALOG PEMILIHAN MEMBER
        // =====================================================
        private int? SelectMemberId()
        {
            IReadOnlyList<MemberOption> members =
                _orders.GetMembers();

            if (members.Count == 0)
            {
                MessageBox.Show(
                    "Belum ada member di database.\n" +
                    "Tambahkan member terlebih dahulu melalui menu Manage Member.",
                    "Member Tidak Tersedia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return null;
            }

            using Form dialog = new Form();

            dialog.Text = "Pilih Member";
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.ClientSize = new Size(390, 145);
            dialog.MaximizeBox = false;
            dialog.MinimizeBox = false;
            dialog.ShowInTaskbar = false;

            Label label = new Label
            {
                Text = "Pilih member untuk pesanan ini:",
                Location = new Point(15, 15),
                AutoSize = true
            };

            ComboBox cbMembers = new ComboBox
            {
                Location = new Point(15, 40),
                Width = 360,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            foreach (MemberOption member in members)
            {
                cbMembers.Items.Add(member);
            }

            if (cbMembers.Items.Count > 0)
            {
                cbMembers.SelectedIndex = 0;
            }

            Button btnSelect = new Button
            {
                Text = "Pilih",
                Location = new Point(195, 90),
                Size = new Size(85, 30),
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Batal",
                Location = new Point(290, 90),
                Size = new Size(85, 30),
                DialogResult = DialogResult.Cancel
            };

            dialog.Controls.Add(label);
            dialog.Controls.Add(cbMembers);
            dialog.Controls.Add(btnSelect);
            dialog.Controls.Add(btnCancel);

            dialog.AcceptButton = btnSelect;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return null;
            }

            if (cbMembers.SelectedItem is MemberOption selectedMember)
            {
                return selectedMember.Id;
            }

            return null;
        }

        // =====================================================
        // SIMPAN ORDER
        // =====================================================
        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (dtCart.Rows.Count == 0)
            {
                MessageBox.Show("Keranjang kosong!");
                return;
            }

            if (string.IsNullOrWhiteSpace(_employeeId))
            {
                MessageBox.Show(
                    "Employee ID tidak tersedia. Silakan login kembali.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            try
            {
                // Ambil Member ID dari data database yang valid
                int? memberId = SelectMemberId();

                if (!memberId.HasValue)
                {
                    // User membatalkan pemilihan member
                    return;
                }

                var orderItems = new List<OrderItem>();

                foreach (DataRow row in dtCart.Rows)
                {
                    orderItems.Add(new OrderItem
                    {
                        MenuId = Convert.ToInt32(row["MenuId"]),
                        Qty = Convert.ToInt32(row["Qty"])
                    });
                }

                // Tidak lagi menggunakan Member ID hardcode = 1
                string orderId = _orders.SaveOrder(
                    _employeeId,
                    memberId.Value,
                    orderItems
                );

                MessageBox.Show(
                    $"Pesanan berhasil dibuat!\nOrder ID: {orderId}",
                    "Berhasil",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                dtCart.Clear();
                UpdateTotals();

                txtMenuName.Clear();
                txtMenuName.Tag = null;
                txtQty.Clear();
                picPhoto.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menyimpan pesanan:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
