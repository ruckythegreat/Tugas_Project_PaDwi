namespace SMKRestaurant.Forms
{
    partial class FrmMember
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtHandphone;
        private System.Windows.Forms.DateTimePicker dtpJoinDate;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        private System.Windows.Forms.DataGridView dgvMember;

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
            txtName = new TextBox();
            txtEmail = new TextBox();
            txtHandphone = new TextBox();
            dtpJoinDate = new DateTimePicker();

            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnSave = new Button();
            btnCancel = new Button();

            dgvMember = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvMember).BeginInit();
            SuspendLayout();

            // txtName
            txtName.Location = new Point(120, 20);
            txtName.Size = new Size(220, 27);

            // txtEmail
            txtEmail.Location = new Point(120, 55);
            txtEmail.Size = new Size(220, 27);

            // txtHandphone
            txtHandphone.Location = new Point(120, 90);
            txtHandphone.Size = new Size(220, 27);

            // dtpJoinDate
            dtpJoinDate.Location = new Point(120, 125);
            dtpJoinDate.Size = new Size(220, 27);

            // btnAdd
            btnAdd.Text = "Tambah";
            btnAdd.Location = new Point(370, 20);
            btnAdd.Size = new Size(90, 35);
            btnAdd.Click += btnAdd_Click;

            // btnEdit
            btnEdit.Text = "Edit";
            btnEdit.Location = new Point(470, 20);
            btnEdit.Size = new Size(90, 35);
            btnEdit.Click += btnEdit_Click;

            // btnDelete
            btnDelete.Text = "Hapus";
            btnDelete.Location = new Point(570, 20);
            btnDelete.Size = new Size(90, 35);
            btnDelete.Click += btnDelete_Click;

            // btnSave
            btnSave.Text = "Simpan";
            btnSave.Location = new Point(370, 65);
            btnSave.Size = new Size(90, 35);
            btnSave.Click += btnSave_Click;

            // btnCancel
            btnCancel.Text = "Batal";
            btnCancel.Location = new Point(470, 65);
            btnCancel.Size = new Size(90, 35);
            btnCancel.Click += btnCancel_Click;

            // dgvMember
            dgvMember.Location = new Point(20, 180);
            dgvMember.Size = new Size(800, 350);
            dgvMember.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvMember.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvMember.MultiSelect = false;
            dgvMember.ReadOnly = true;
            dgvMember.AllowUserToAddRows = false;
            dgvMember.CellClick += dgvMember_CellClick;

            // FrmMember
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;

            ClientSize = new Size(850, 560);

            Controls.Add(txtName);
            Controls.Add(txtEmail);
            Controls.Add(txtHandphone);
            Controls.Add(dtpJoinDate);

            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            Controls.Add(dgvMember);

            Name = "FrmMember";
            Text = "Member Management";
            StartPosition = FormStartPosition.CenterScreen;

            ((System.ComponentModel.ISupportInitialize)dgvMember).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}