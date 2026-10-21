
using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SMKRestaurant.Forms
{
    public partial class FrmPayment : Form
    {
        private decimal currentTotal = 0;
        private bool isLoadingOrders = false;

        public FrmPayment()
        {
            InitializeComponent();

            // Pilihan metode pembayaran
            cbPaymentType.Items.Clear();
            cbPaymentType.Items.AddRange(new object[]
            {
                "Cash",
                "Credit Card",
                "Debit Card"
            });

            // Pilihan bank
            cbBankName.Items.Clear();
            cbBankName.Items.AddRange(new object[]
            {
                "BCA",
                "BNI",
                "BRI",
                "Mandiri"
            });

            if (cbPaymentType.Items.Count > 0)
            {
                cbPaymentType.SelectedIndex = 0;
            }

            UpdatePaymentFields();
            ClearOrderDetails();
            LoadUnpaidOrders();
        }

        // =====================================================
        // LOAD ORDER YANG BELUM DIBAYAR
        // =====================================================
        private void LoadUnpaidOrders()
        {
            List<string> orderIds = new List<string>();

            try
            {
                isLoadingOrders = true;

                string sql = @"
                    SELECT DISTINCT oh.Id
                    FROM dbo.OrderHeader AS oh
                    INNER JOIN dbo.OrderDetail AS od
                        ON od.OrderId = oh.Id
                    WHERE od.Status = 'Unpaid'
                    ORDER BY oh.Id DESC;";

                using (SqlConnection conn = Database.GetConnection())
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = Convert.ToString(reader["Id"]) ?? "";

                            if (!string.IsNullOrWhiteSpace(id))
                            {
                                orderIds.Add(id);
                            }
                        }
                    }
                }

                // Reset ComboBox sebelum mengisi data baru
                cbOrderId.DataSource = null;
                cbOrderId.Items.Clear();
                cbOrderId.DataSource = orderIds;

                if (orderIds.Count > 0)
                {
                    cbOrderId.SelectedIndex = 0;
                }
                else
                {
                    cbOrderId.SelectedIndex = -1;
                }

                isLoadingOrders = false;

                if (orderIds.Count > 0)
                {
                    LoadOrderDetails(orderIds[0]);
                }
                else
                {
                    ClearOrderDetails();
                }
            }
            catch (Exception ex)
            {
                ClearOrderDetails();

                MessageBox.Show(
                    "Gagal memuat Order ID:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                isLoadingOrders = false;
            }
        }

        // =====================================================
        // EVENT KETIKA ORDER DIPILIH
        // =====================================================
        private void cbOrderId_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (isLoadingOrders)
            {
                return;
            }

            string orderId = cbOrderId.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(orderId))
            {
                ClearOrderDetails();
                return;
            }

            LoadOrderDetails(orderId);
        }

        // =====================================================
        // LOAD DETAIL PESANAN
        // =====================================================
        private void LoadOrderDetails(string orderId)
        {
            try
            {
                string sql = @"
                    SELECT
                        m.Name AS MenuName,
                        od.Qty AS Qty,
                        m.Price AS Price,
                        (m.Price * od.Qty) AS Total
                    FROM dbo.OrderDetail AS od
                    INNER JOIN dbo.MsMenu AS m
                        ON od.MenuId = m.Id
                    WHERE od.OrderId = @OrderId
                    ORDER BY m.Name;";

                DataTable dt = new DataTable();

                using (SqlConnection conn = Database.GetConnection())
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@OrderId",
                        NormalizeOrderId(orderId)
                    );

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }

                dgvOrderDetail.DataSource = dt;

                currentTotal = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Total"] != DBNull.Value)
                    {
                        currentTotal += Convert.ToDecimal(row["Total"]);
                    }
                }

                lblTotalAmount.Text =
                    $"Total : {currentTotal:N0}";

                // Format harga agar mudah dibaca
                if (dgvOrderDetail.Columns["Price"] != null)
                {
                    dgvOrderDetail.Columns["Price"]!
                        .DefaultCellStyle.Format = "N0";
                }

                if (dgvOrderDetail.Columns["Total"] != null)
                {
                    dgvOrderDetail.Columns["Total"]!
                        .DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                ClearOrderDetails();

                MessageBox.Show(
                    "Gagal memuat detail pesanan:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // MEMASTIKAN TIPE ID SESUAI DENGAN BENTUK ORDER ID
        // =====================================================
        private static object NormalizeOrderId(string orderId)
        {
            if (int.TryParse(orderId, out int numericId))
            {
                return numericId;
            }

            return orderId;
        }

        // =====================================================
        // ATUR FIELD PEMBAYARAN
        // =====================================================
        private void cbPaymentType_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            UpdatePaymentFields();
        }

        private void UpdatePaymentFields()
        {
            string paymentType =
                cbPaymentType.SelectedItem?.ToString() ?? "";

            bool isCard =
                !string.IsNullOrWhiteSpace(paymentType) &&
                !paymentType.Equals(
                    "Cash",
                    StringComparison.OrdinalIgnoreCase
                );

            txtCardNumber.Enabled = isCard;
            cbBankName.Enabled = isCard;

            if (!isCard)
            {
                txtCardNumber.Clear();
                cbBankName.SelectedIndex = -1;
            }
        }

        // =====================================================
        // CEK APAKAH KOLOM OPSIONAL TERSEDIA
        // =====================================================
        private static bool ColumnExists(
            SqlConnection conn,
            SqlTransaction transaction,
            string columnName)
        {
            string sql = @"
                SELECT CASE
                    WHEN COL_LENGTH(
                        N'dbo.OrderHeader',
                        @ColumnName
                    ) IS NOT NULL THEN 1
                    ELSE 0
                END;";

            using (SqlCommand cmd =
                new SqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.Add(
                    "@ColumnName",
                    SqlDbType.NVarChar,
                    128
                ).Value = columnName;

                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        // =====================================================
        // SIMPAN PEMBAYARAN
        // =====================================================
        private bool SavePaymentToDatabase(
            string orderId,
            string paymentType,
            string cardNumber,
            string bankName)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Kolom kartu/bank hanya digunakan apabila tersedia
                        bool hasCardNumber = ColumnExists(
                            conn,
                            transaction,
                            "CardNumber"
                        );

                        bool hasBankName = ColumnExists(
                            conn,
                            transaction,
                            "BankName"
                        );

                        // Update header pesanan
                        StringBuilder sqlHeader = new StringBuilder(
                            "UPDATE dbo.OrderHeader " +
                            "SET PaymentType = @PaymentType"
                        );

                        if (hasCardNumber)
                        {
                            sqlHeader.Append(
                                ", CardNumber = @CardNumber"
                            );
                        }

                        if (hasBankName)
                        {
                            sqlHeader.Append(
                                ", BankName = @BankName"
                            );
                        }

                        sqlHeader.Append(" WHERE Id = @OrderId;");

                        using (SqlCommand cmd = new SqlCommand(
                            sqlHeader.ToString(),
                            conn,
                            transaction))
                        {
                            cmd.Parameters.Add(
                                "@PaymentType",
                                SqlDbType.NVarChar,
                                50
                            ).Value = paymentType;

                            cmd.Parameters.AddWithValue(
                                "@OrderId",
                                NormalizeOrderId(orderId)
                            );

                            if (hasCardNumber)
                            {
                                cmd.Parameters.Add(
                                    "@CardNumber",
                                    SqlDbType.NVarChar,
                                    50
                                ).Value = cardNumber;
                            }

                            if (hasBankName)
                            {
                                cmd.Parameters.Add(
                                    "@BankName",
                                    SqlDbType.NVarChar,
                                    100
                                ).Value = bankName;
                            }

                            int affectedRows = cmd.ExecuteNonQuery();

                            if (affectedRows == 0)
                            {
                                throw new InvalidOperationException(
                                    "Order ID tidak ditemukan."
                                );
                            }
                        }

                        // Tandai semua detail pesanan sebagai sudah dibayar
                        string sqlDetail = @"
                            UPDATE dbo.OrderDetail
                            SET Status = 'Paid'
                            WHERE OrderId = @OrderId;";

                        using (SqlCommand cmd = new SqlCommand(
                            sqlDetail,
                            conn,
                            transaction))
                        {
                            cmd.Parameters.AddWithValue(
                                "@OrderId",
                                NormalizeOrderId(orderId)
                            );

                            int affectedRows = cmd.ExecuteNonQuery();

                            if (affectedRows == 0)
                            {
                                throw new InvalidOperationException(
                                    "Detail pesanan tidak ditemukan."
                                );
                            }
                        }

                        // Simpan kedua perubahan secara bersamaan
                        transaction.Commit();

                        // True jika kedua kolom kartu dan bank tersedia
                        return hasCardNumber && hasBankName;
                    }
                    catch
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // Pertahankan error utama apabila rollback gagal
                        }

                        throw;
                    }
                }
            }
        }

        // =====================================================
        // TOMBOL SIMPAN
        // =====================================================
        private void btnSave_Click(object? sender, EventArgs e)
        {
            string orderId =
                cbOrderId.SelectedItem?.ToString() ?? "";

            string paymentType =
                cbPaymentType.SelectedItem?.ToString() ?? "";

            string cardNumber = txtCardNumber.Text.Trim();

            string bankName =
                cbBankName.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(orderId))
            {
                MessageBox.Show(
                    "Pilih Order ID terlebih dahulu.",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(paymentType))
            {
                MessageBox.Show(
                    "Pilih metode pembayaran terlebih dahulu.",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            bool isCash = paymentType.Equals(
                "Cash",
                StringComparison.OrdinalIgnoreCase
            );

            if (!isCash &&
                (string.IsNullOrWhiteSpace(cardNumber) ||
                 string.IsNullOrWhiteSpace(bankName)))
            {
                MessageBox.Show(
                    "Card Number dan Bank Name harus diisi " +
                    "untuk pembayaran non-tunai.",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (currentTotal <= 0)
            {
                MessageBox.Show(
                    "Total pesanan tidak valid atau detail pesanan kosong.",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            try
            {
                bool cardInfoSaved = SavePaymentToDatabase(
                    orderId,
                    paymentType,
                    cardNumber,
                    bankName
                );

                if (!isCash && !cardInfoSaved)
                {
                    MessageBox.Show(
                        "Pembayaran berhasil disimpan dan status " +
                        "pesanan telah diperbarui.\n\n" +
                        "Namun, nomor kartu dan nama bank tidak " +
                        "sepenuhnya tersimpan karena kolom " +
                        "CardNumber atau BankName belum tersedia " +
                        "di dbo.OrderHeader.",
                        "Pembayaran Berhasil",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Pembayaran berhasil disimpan!",
                        "Berhasil",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                ClearOrderDetails();
                LoadUnpaidOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menyimpan pembayaran:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // RESET DETAIL PESANAN
        // =====================================================
        private void ClearOrderDetails()
        {
            currentTotal = 0;

            dgvOrderDetail.DataSource = null;
            lblTotalAmount.Text = "Total : 0";
        }
    }
}
