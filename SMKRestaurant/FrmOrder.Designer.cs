namespace SMKRestaurant.Forms
{
    partial class FrmOrder
    {
        private System.ComponentModel.IContainer components = null;

        private ComboBox cmbMember;
        private DataGridView dgvMenu;
        private NumericUpDown nudQty;
        private Button btnAdd;
        private DataGridView dgvOrder;
        private Button btnSave;
        private Label lblTotalPrice;
        private Label lblTotalCarbo;
        private Label lblTotalProtein;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            cmbMember = new ComboBox();
            dgvMenu = new DataGridView();
            nudQty = new NumericUpDown();
            btnAdd = new Button();
            dgvOrder = new DataGridView();
            btnSave = new Button();
            lblTotalPrice = new Label();
            lblTotalCarbo = new Label();
            lblTotalProtein = new Label();

            ((System.ComponentModel.ISupportInitialize)dgvMenu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrder).BeginInit();

            SuspendLayout();

            // cmbMember
            cmbMember.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMember.Location = new Point(20, 20);
            cmbMember.Size = new Size(250, 27);

            // dgvMenu
            dgvMenu.Location = new Point(20, 65);
            dgvMenu.Size = new Size(500, 230);
            dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenu.MultiSelect = false;
            dgvMenu.ReadOnly = true;

            // nudQty
            nudQty.Location = new Point(540, 65);
            nudQty.Minimum = 1;
            nudQty.Maximum = 100;
            nudQty.Value = 1;
            nudQty.Size = new Size(100, 27);

            // btnAdd
            btnAdd.Location = new Point(540, 105);
            btnAdd.Size = new Size(100, 35);
            btnAdd.Text = "Tambah";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;

            // dgvOrder
            dgvOrder.Location = new Point(20, 320);
            dgvOrder.Size = new Size(620, 200);
            dgvOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrder.ReadOnly = true;

            // lblTotalPrice
            lblTotalPrice.Location = new Point(20, 540);
            lblTotalPrice.Size = new Size(180, 30);
            lblTotalPrice.Text = "0";

            // lblTotalCarbo
            lblTotalCarbo.Location = new Point(220, 540);
            lblTotalCarbo.Size = new Size(180, 30);
            lblTotalCarbo.Text = "0";

            // lblTotalProtein
            lblTotalProtein.Location = new Point(420, 540);
            lblTotalProtein.Size = new Size(180, 30);
            lblTotalProtein.Text = "0";

            // btnSave
            btnSave.Location = new Point(540, 580);
            btnSave.Size = new Size(100, 40);
            btnSave.Text = "Simpan";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;

            // FrmOrder
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 640);
            Controls.Add(cmbMember);
            Controls.Add(dgvMenu);
            Controls.Add(nudQty);
            Controls.Add(btnAdd);
            Controls.Add(dgvOrder);
            Controls.Add(lblTotalPrice);
            Controls.Add(lblTotalCarbo);
            Controls.Add(lblTotalProtein);
            Controls.Add(btnSave);
            Name = "FrmOrder";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Order";
            Load += FrmOrder_Load;

            ((System.ComponentModel.ISupportInitialize)dgvMenu).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrder).EndInit();

            ResumeLayout(false);
        }
    }
}