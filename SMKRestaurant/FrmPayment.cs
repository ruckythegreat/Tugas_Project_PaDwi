using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;

namespace SMKRestaurant.Forms
{
    public partial class FrmPayment : Form
    {
        public FrmPayment()
        {
            InitializeComponent();

            LoadUnpaidOrder();

            cmbPaymentType.Items.Clear();
            cmbPaymentType.Items.Add("cash");
            cmbPaymentType.Items.Add("qris");
            cmbPaymentType.Items.Add("debit");

            // Event handler saat order dipilih
            cmbOrder.SelectedIndexChanged += cmbOrder_SelectedIndexChanged;
        }

        private void LoadUnpaidOrder()
        {
            string sql = @"
                SELECT DISTINCT oh.Id
                FROM OrderHeader oh
                INNER JOIN OrderDetail od ON oh.Id = od.OrderId
                WHERE od.Status = 'Unpaid'";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    cmbOrder.Items.Clear();
                    while (reader.Read())
                    {
                        cmbOrder.Items.Add(reader["Id"].ToString());
                    }
                }
            }
        }

        // DITAMBAHKAN: Tampilkan total harga saat order dipilih dari ComboBox
        private void cmbOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOrder.SelectedItem != null)
            {
                string orderId = cmbOrder.SelectedItem.ToString() ?? "";
                int total = GetOrderTotal(orderId);
                lblTotal.Text = total.ToString("N0"); // Tampilkan ke label total
            }
            else
            {
                lblTotal.Text = "0";
            }
        }

        private int GetOrderTotal(string orderId)
        {
            string sql = @"
                SELECT SUM(m.Price * od.Qty)
                FROM OrderDetail od
                INNER JOIN MsMenu m ON od.MenuId = m.Id
                WHERE od.OrderId = @OrderId";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbOrder.SelectedIndex == -1)
            {
                MessageBox.Show("Pilih order terlebih dahulu.");
                return;
            }

            if (cmbPaymentType.SelectedItem == null)
            {
                MessageBox.Show("Pilih tipe pembayaran.");
                return;
            }

            string paymentType = cmbPaymentType.SelectedItem.ToString() ?? "";
            string orderId = cmbOrder.SelectedItem.ToString() ?? "";
            int total = GetOrderTotal(orderId);

            if (paymentType == "cash")
            {
                if (!int.TryParse(txtCash.Text, out int cash))
                {
                    MessageBox.Show("Nominal cash tidak valid.");
                    return;
                }

                if (cash < total)
                {
                    MessageBox.Show("Uang pembayaran kurang.");
                    return;
                }

                int change = cash - total;
                lblChange.Text = change.ToString("N0");
            }

            PayOrder(orderId, paymentType);
        }

        private void PayOrder(string orderId, string paymentType)
        {
            string sqlHeader = @"
                UPDATE OrderHeader
                SET PaymentType = @PaymentType
                WHERE Id = @Id";

            string sqlDetail = @"
                UPDATE OrderDetail
                SET Status = 'Paid'
                WHERE OrderId = @Id";

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sqlHeader, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@PaymentType", paymentType);
                            cmd.Parameters.AddWithValue("@Id", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(sqlDetail, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Pembayaran berhasil.");

                        LoadUnpaidOrder();
                        cmbOrder.SelectedIndex = -1;
                        cmbPaymentType.SelectedIndex = -1;
                        txtCash.Clear();
                        lblChange.Text = "0";
                        lblTotal.Text = "0";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Pembayaran gagal: " + ex.Message);
                    }
                }
            }
        }
    }
}