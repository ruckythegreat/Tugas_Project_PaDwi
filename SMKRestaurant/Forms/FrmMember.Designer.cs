using System.Drawing;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    partial class FrmMember
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle, lblMemberId, lblName, lblEmail, lblHandphone, lblJoinDate;
        private TextBox txtSearch, txtMemberId, txtName, txtEmail, txtHandphone;
        private DateTimePicker dtpJoinDate;
        private Button btnSearch, btnInsert, btnUpdate, btnDelete, btnSave, btnCancel;
        private DataGridView dgvMember;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "Form Manage Member";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            lblTitle = new Label { Text = "Form Manage Member", Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(250, 20), AutoSize = true };

            // DataGridView
            dgvMember = new DataGridView { Location = new Point(40, 70), Size = new Size(700, 250), ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgvMember.CellClick += dgvMember_CellClick;

            // Search (Bottom Left)
            txtSearch = new TextBox { Location = new Point(40, 340), Width = 180 };
            btnSearch = new Button { Text = "Search", Location = new Point(230, 338), Width = 70 };
            btnSearch.Click += btnSearch_Click;

            // Inputs (Bottom Right)
            int labelX = 400, txtX = 480, startY = 340, gap = 30;

            lblMemberId = new Label { Text = "MemberId", Location = new Point(labelX, startY), AutoSize = true };
            txtMemberId = new TextBox { Location = new Point(txtX, startY), Width = 200, ReadOnly = true }; // Readonly

            lblName = new Label { Text = "Name", Location = new Point(labelX, startY += gap), AutoSize = true };
            txtName = new TextBox { Location = new Point(txtX, startY), Width = 200 };

            lblEmail = new Label { Text = "Email", Location = new Point(labelX, startY += gap), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(txtX, startY), Width = 200 };

            lblHandphone = new Label { Text = "Handphone", Location = new Point(labelX, startY += gap), AutoSize = true };
            txtHandphone = new TextBox { Location = new Point(txtX, startY), Width = 200 };

            lblJoinDate = new Label { Text = "Join Date", Location = new Point(labelX, startY += gap), AutoSize = true };
            dtpJoinDate = new DateTimePicker { Location = new Point(txtX, startY), Width = 200, Format = DateTimePickerFormat.Short };

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
            this.Controls.Add(lblTitle); this.Controls.Add(dgvMember); this.Controls.Add(txtSearch); this.Controls.Add(btnSearch);
            this.Controls.Add(lblMemberId); this.Controls.Add(txtMemberId); this.Controls.Add(lblName); this.Controls.Add(txtName);
            this.Controls.Add(lblEmail); this.Controls.Add(txtEmail); this.Controls.Add(lblHandphone); this.Controls.Add(txtHandphone);
            this.Controls.Add(lblJoinDate); this.Controls.Add(dtpJoinDate);
            this.Controls.Add(btnInsert); this.Controls.Add(btnUpdate); this.Controls.Add(btnDelete); this.Controls.Add(btnSave); this.Controls.Add(btnCancel);
        }
    }
}