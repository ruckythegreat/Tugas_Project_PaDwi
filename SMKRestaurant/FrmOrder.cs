using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;
using SMKRestaurant.Models;
using System.Data;

namespace SMKRestaurant.Forms
{
    public partial class FrmOrder : Form
    {
        private List<OrderItem> orderItems = new List<OrderItem>();
        private string employeeId = "";

        public FrmOrder(string empId = "")
        {
            InitializeComponent();
            this.employeeId = empId;
        }

        private void FrmOrder_Load(object sender, EventArgs e)
        {
            LoadMemberCombo();
            LoadMenuData();
        }

        private void LoadMemberCombo()
        {
            string sql = "SELECT Id, Name FROM MsMember";
            using (SqlConnection conn = Database.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbMember.DataSource = dt;
                cmbMember.DisplayMember = "Name";
                cmbMember.ValueMember = "Id";
                cmbMember.SelectedIndex = -1;
            }
        }

        private void LoadMenuData()
        {
            string sql = "SELECT Id, Name, Price, Carbo, Protein FROM MsMenu";
            using (SqlConnection conn = Database.GetConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dgvMenu != null) dgvMenu.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvMenu.CurrentRow == null)
            {
                MessageBox.Show("Pilih menu terlebih dahulu!");
                return;
            }

            int menuId = Convert.ToInt32(dgvMenu.CurrentRow.Cells["Id"].Value);
            int qty = (int)nudQty.Value;

            AddMenu(menuId, qty);
        }

        private void AddMenu(int menuId, int qty)
        {
            if (qty <= 0) return;

            OrderItem? existing = orderItems.FirstOrDefault(x => x.MenuId == menuId);

            if (existing != null)
            {
                existing.Qty = qty;
            }
            else
            {
                OrderItem? item = GetMenuById(menuId);
                if (item != null)
                {
                    item.Qty = qty;
                    orderItems.Add(item);
                }
            }

            RefreshOrder();
        }

        private OrderItem? GetMenuById(int menuId)
        {
            string sql = "SELECT Id, Name, Price, Carbo, Protein FROM MsMenu WHERE Id = @Id";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", menuId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrderItem
                        {
                            MenuId = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString() ?? "",
                            Price = Convert.ToInt32(reader["Price"]),
                            Carbo = Convert.ToInt32(reader["Carbo"]),
                            Protein = Convert.ToInt32(reader["Protein"]),
                            Qty = 1
                        };
                    }
                }
            }
            return null;
        }

        private void RefreshOrder()
        {
            dgvOrder.DataSource = null;
            dgvOrder.DataSource = orderItems;

            int totalPrice = orderItems.Sum(x => x.Price * x.Qty);
            int totalCarbo = orderItems.Sum(x => x.Carbo * x.Qty);
            int totalProtein = orderItems.Sum(x => x.Protein * x.Qty);

            lblTotalPrice.Text = totalPrice.ToString("N0");
            lblTotalCarbo.Text = totalCarbo.ToString();
            lblTotalProtein.Text = totalProtein.ToString();
        }

private string GenerateOrderId()
{
    string datePrefix = DateTime.Now.ToString("yyyyMMdd");
    
    // Mengambil nomor urut tertinggi hari ini
    string sql = @"
        SELECT ISNULL(MAX(CAST(RIGHT(Id, 4) AS INT)), 0) 
        FROM OrderHeader 
        WHERE CAST(Date AS DATE) = CAST(GETDATE() AS DATE)";

    int lastNumber = 0;

    using (SqlConnection conn = Database.GetConnection())
    using (SqlCommand cmd = new SqlCommand(sql, conn))
    {
        conn.Open();
        object result = cmd.ExecuteScalar();
        if (result != null && result != DBNull.Value)
        {
            lastNumber = Convert.ToInt32(result);
        }
    }

    int nextNumber = lastNumber + 1;
    return datePrefix + nextNumber.ToString("D4");
}
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (orderItems.Count == 0)
            {
                MessageBox.Show("Silakan pilih menu terlebih dahulu.");
                return;
            }

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                MessageBox.Show("Employee ID belum ditentukan. Silakan login ulang!");
                return;
            }

            if (cmbMember.SelectedValue == null)
            {
                MessageBox.Show("Silakan pilih member.");
                return;
            }

            int memberId = Convert.ToInt32(cmbMember.SelectedValue);
            string orderId = GenerateOrderId();

            string sqlHeader = @"
                INSERT INTO OrderHeader (Id, EmployeeId, MemberId, Date)
                VALUES (@Id, @EmployeeId, @MemberId, @Date)";

            string sqlDetail = @"
                INSERT INTO OrderDetail (OrderId, MenuId, Qty, Status)
                VALUES (@OrderId, @MenuId, @Qty, @Status)";

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmdHeader = new SqlCommand(sqlHeader, conn, transaction))
                        {
                            cmdHeader.Parameters.AddWithValue("@Id", orderId);
                            cmdHeader.Parameters.AddWithValue("@EmployeeId", employeeId);
                            cmdHeader.Parameters.AddWithValue("@MemberId", memberId);
                            cmdHeader.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                            cmdHeader.ExecuteNonQuery();
                        }

                        foreach (OrderItem item in orderItems)
                        {
                            using (SqlCommand cmdDetail = new SqlCommand(sqlDetail, conn, transaction))
                            {
                                cmdDetail.Parameters.AddWithValue("@OrderId", orderId);
                                cmdDetail.Parameters.AddWithValue("@MenuId", item.MenuId);
                                cmdDetail.Parameters.AddWithValue("@Qty", item.Qty);
                                cmdDetail.Parameters.AddWithValue("@Status", "Unpaid");
                                cmdDetail.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Order berhasil disimpan.\nOrder ID: " + orderId);

                        orderItems.Clear();
                        RefreshOrder();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Order gagal disimpan: " + ex.Message);
                    }
                }
            }
        }
    }
}