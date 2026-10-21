using System.Drawing;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    partial class FrmOrder
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle, lblMenuName, lblQty, lblCarbo, lblProtein, lblTotal;
        private TextBox txtMenuName, txtQty;
        private Button btnAdd, btnDelete, btnOrderProcess;
        private DataGridView dgvMenu, dgvCart;
        private PictureBox picPhoto;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            dgvMenu = new DataGridView();
            picPhoto = new PictureBox();
            lblMenuName = new Label();
            txtMenuName = new TextBox();
            lblQty = new Label();
            txtQty = new TextBox();
            btnAdd = new Button();
            btnDelete = new Button();
            dgvCart = new DataGridView();
            lblCarbo = new Label();
            lblProtein = new Label();
            lblTotal = new Label();
            btnOrderProcess = new Button();

            ((System.ComponentModel.ISupportInitialize)(dgvMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(picPhoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dgvCart)).BeginInit();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(320, 10);
            lblTitle.Text = "Form Order";

            // dgvMenu
            dgvMenu.AllowUserToAddRows = false;
            dgvMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMenu.Location = new Point(40, 50);
            dgvMenu.ReadOnly = true;
            dgvMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMenu.Size = new Size(700, 150);
            dgvMenu.CellClick += new DataGridViewCellEventHandler(dgvMenu_CellClick);

            // picPhoto
            picPhoto.BorderStyle = BorderStyle.FixedSingle;
            picPhoto.Location = new Point(180, 210);
            picPhoto.Size = new Size(150, 100);
            picPhoto.SizeMode = PictureBoxSizeMode.Zoom;

            // lblMenuName
            lblMenuName.AutoSize = true;
            lblMenuName.Location = new Point(350, 220);
            lblMenuName.Text = "Menu Name";

            // txtMenuName
            txtMenuName.Location = new Point(430, 217);
            txtMenuName.ReadOnly = true;
            txtMenuName.Size = new Size(150, 23);

            // lblQty
            lblQty.AutoSize = true;
            lblQty.Location = new Point(350, 260);
            lblQty.Text = "Qty";

            // txtQty
            txtQty.Location = new Point(430, 257);
            txtQty.Size = new Size(150, 23);

            // btnAdd
            btnAdd.Location = new Point(350, 290);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += new System.EventHandler(btnAdd_Click);

            // btnDelete
            btnDelete.Location = new Point(480, 290);
            btnDelete.Size = new Size(100, 30);
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += new System.EventHandler(btnDelete_Click);

            // dgvCart
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.Location = new Point(40, 330);
            dgvCart.ReadOnly = true;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.Size = new Size(700, 150);

            // lblCarbo
            lblCarbo.AutoSize = true;
            lblCarbo.Location = new Point(40, 490);
            lblCarbo.Text = "Carbo: 0";

            // lblProtein
            lblProtein.AutoSize = true;
            lblProtein.Location = new Point(120, 490);
            lblProtein.Text = "Protein: 0";

            // btnOrderProcess
            btnOrderProcess.Location = new Point(350, 510);
            btnOrderProcess.Size = new Size(100, 35);
            btnOrderProcess.Text = "Order";
            btnOrderProcess.UseVisualStyleBackColor = true;
            btnOrderProcess.Click += new System.EventHandler(btnOrder_Click);

            // lblTotal
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTotal.Location = new Point(620, 490);
            lblTotal.Text = "Total: 0";

            // FrmOrder
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 560);
            Controls.Add(lblTitle);
            Controls.Add(dgvMenu);
            Controls.Add(picPhoto);
            Controls.Add(lblMenuName);
            Controls.Add(txtMenuName);
            Controls.Add(lblQty);
            Controls.Add(txtQty);
            Controls.Add(btnAdd);
            Controls.Add(btnDelete);
            Controls.Add(dgvCart);
            Controls.Add(lblCarbo);
            Controls.Add(lblProtein);
            Controls.Add(btnOrderProcess);
            Controls.Add(lblTotal);
            Name = "FrmOrder";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SMK Restaurant - Order";

            ((System.ComponentModel.ISupportInitialize)(dgvMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(picPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dgvCart)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}