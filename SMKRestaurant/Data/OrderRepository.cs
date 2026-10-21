
using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;
using SMKRestaurant.Models;
using System;
using System.Collections.Generic;

namespace SMKRestaurant.Data
{
    public class OrderRepository
    {
        // =====================================================
        // MENGAMBIL DAFTAR MEMBER
        // =====================================================
        public IReadOnlyList<MemberOption> GetMembers()
        {
            const string sql = @"
                SELECT Id, Name
                FROM dbo.MsMember
                ORDER BY Name;";

            var members = new List<MemberOption>();

            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                members.Add(new MemberOption
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"]?.ToString() ?? ""
                });
            }

            return members;
        }

        // =====================================================
        // MENGAMBIL ORDER ID YANG BELUM DIBAYAR
        // =====================================================
        public IReadOnlyList<string> GetUnpaidOrderIds()
        {
            const string sql = @"
                SELECT DISTINCT oh.Id
                FROM dbo.OrderHeader AS oh
                INNER JOIN dbo.OrderDetail AS od
                    ON oh.Id = od.OrderId
                WHERE od.Status = 'Unpaid'
                ORDER BY oh.Id DESC;";

            var ids = new List<string>();

            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string id = reader["Id"]?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(id))
                {
                    ids.Add(id);
                }
            }

            return ids;
        }

        // =====================================================
        // MENGHITUNG TOTAL PESANAN
        // =====================================================
        public int GetOrderTotal(string orderId)
        {
            const string sql = @"
                SELECT COALESCE(SUM(m.Price * od.Qty), 0)
                FROM dbo.OrderDetail AS od
                INNER JOIN dbo.MsMenu AS m
                    ON od.MenuId = m.Id
                WHERE od.OrderId = @OrderId;";

            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@OrderId", orderId);

            conn.Open();

            object? result = cmd.ExecuteScalar();

            return result == null || result == DBNull.Value
                ? 0
                : Convert.ToInt32(result);
        }

        // =====================================================
        // MENYIMPAN ORDER DALAM SATU TRANSAKSI
        // =====================================================
        public string SaveOrder(
            string employeeId,
            int memberId,
            IReadOnlyList<OrderItem> items)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                throw new ArgumentException(
                    "Employee ID tidak boleh kosong.",
                    nameof(employeeId)
                );
            }

            if (memberId <= 0)
            {
                throw new ArgumentException(
                    "Pilih member yang valid.",
                    nameof(memberId)
                );
            }

            if (items == null || items.Count == 0)
            {
                throw new ArgumentException(
                    "Keranjang pesanan tidak boleh kosong.",
                    nameof(items)
                );
            }

            foreach (var item in items)
            {
                if (item.MenuId <= 0 || item.Qty <= 0)
                {
                    throw new ArgumentException(
                        "Menu ID dan jumlah pesanan harus valid."
                    );
                }
            }

            using var conn = Database.GetConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                // Validasi Employee ID
                const string checkEmployeeSql = @"
                    SELECT COUNT(1)
                    FROM dbo.MsEmployee
                    WHERE Id = @EmployeeId;";

                using (var cmd = new SqlCommand(
                    checkEmployeeSql,
                    conn,
                    transaction))
                {
                    cmd.Parameters.AddWithValue(
                        "@EmployeeId",
                        employeeId
                    );

                    int employeeExists =
                        Convert.ToInt32(cmd.ExecuteScalar());

                    if (employeeExists == 0)
                    {
                        throw new InvalidOperationException(
                            $"Employee ID '{employeeId}' " +
                            "tidak ditemukan di MsEmployee."
                        );
                    }
                }

                // Validasi Member ID untuk mencegah FOREIGN KEY error
                const string checkMemberSql = @"
                    SELECT COUNT(1)
                    FROM dbo.MsMember
                    WHERE Id = @MemberId;";

                using (var cmd = new SqlCommand(
                    checkMemberSql,
                    conn,
                    transaction))
                {
                    cmd.Parameters.AddWithValue(
                        "@MemberId",
                        memberId
                    );

                    int memberExists =
                        Convert.ToInt32(cmd.ExecuteScalar());

                    if (memberExists == 0)
                    {
                        throw new InvalidOperationException(
                            $"Member ID {memberId} tidak ditemukan. " +
                            "Silakan pilih member yang tersedia."
                        );
                    }
                }

                // Pastikan seluruh menu masih tersedia
                const string checkMenuSql = @"
                    SELECT COUNT(1)
                    FROM dbo.MsMenu
                    WHERE Id = @MenuId;";

                foreach (var item in items)
                {
                    using var cmd = new SqlCommand(
                        checkMenuSql,
                        conn,
                        transaction
                    );

                    cmd.Parameters.AddWithValue(
                        "@MenuId",
                        item.MenuId
                    );

                    int menuExists =
                        Convert.ToInt32(cmd.ExecuteScalar());

                    if (menuExists == 0)
                    {
                        throw new InvalidOperationException(
                            $"Menu ID {item.MenuId} tidak ditemukan."
                        );
                    }
                }

                // Generate Order ID di dalam transaksi
                string orderId = GenerateOrderId(
                    conn,
                    transaction
                );

                // Simpan header pesanan
                const string sqlHeader = @"
                    INSERT INTO dbo.OrderHeader
                        (Id, EmployeeId, MemberId, Date)
                    VALUES
                        (@Id, @EmployeeId, @MemberId, @Date);";

                using (var cmdHeader = new SqlCommand(
                    sqlHeader,
                    conn,
                    transaction))
                {
                    cmdHeader.Parameters.AddWithValue(
                        "@Id",
                        orderId
                    );

                    cmdHeader.Parameters.AddWithValue(
                        "@EmployeeId",
                        employeeId
                    );

                    cmdHeader.Parameters.AddWithValue(
                        "@MemberId",
                        memberId
                    );

                    cmdHeader.Parameters.AddWithValue(
                        "@Date",
                        DateTime.Now.Date
                    );

                    cmdHeader.ExecuteNonQuery();
                }

                // Simpan detail pesanan
                const string sqlDetail = @"
                    INSERT INTO dbo.OrderDetail
                        (OrderId, MenuId, Qty, Status)
                    VALUES
                        (@OrderId, @MenuId, @Qty, @Status);";

                foreach (var item in items)
                {
                    using var cmdDetail = new SqlCommand(
                        sqlDetail,
                        conn,
                        transaction
                    );

                    cmdDetail.Parameters.AddWithValue(
                        "@OrderId",
                        orderId
                    );

                    cmdDetail.Parameters.AddWithValue(
                        "@MenuId",
                        item.MenuId
                    );

                    cmdDetail.Parameters.AddWithValue(
                        "@Qty",
                        item.Qty
                    );

                    cmdDetail.Parameters.AddWithValue(
                        "@Status",
                        "Unpaid"
                    );

                    cmdDetail.ExecuteNonQuery();
                }

                transaction.Commit();

                return orderId;
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // Jangan mengganti error utama jika rollback gagal.
                }

                throw;
            }
        }

        // =====================================================
        // MEMBAYAR ORDER
        // =====================================================
        public void PayOrder(
            string orderId,
            string paymentType)
        {
            using var conn = Database.GetConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                const string sqlHeader = @"
                    UPDATE dbo.OrderHeader
                    SET PaymentType = @PaymentType
                    WHERE Id = @Id;";

                using (var cmd = new SqlCommand(
                    sqlHeader,
                    conn,
                    transaction))
                {
                    cmd.Parameters.AddWithValue(
                        "@PaymentType",
                        paymentType
                    );

                    cmd.Parameters.AddWithValue("@Id", orderId);

                    int affected = cmd.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        throw new InvalidOperationException(
                            "Order ID tidak ditemukan."
                        );
                    }
                }

                const string sqlDetail = @"
                    UPDATE dbo.OrderDetail
                    SET Status = 'Paid'
                    WHERE OrderId = @Id;";

                using (var cmd = new SqlCommand(
                    sqlDetail,
                    conn,
                    transaction))
                {
                    cmd.Parameters.AddWithValue("@Id", orderId);

                    int affected = cmd.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        throw new InvalidOperationException(
                            "Detail pesanan tidak ditemukan."
                        );
                    }
                }

                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // Pertahankan error utama.
                }

                throw;
            }
        }

        // =====================================================
        // GENERATE ORDER ID
        // Format: yyyyMMdd + 4 digit sequence
        // =====================================================
        private static string GenerateOrderId(
            SqlConnection conn,
            SqlTransaction transaction)
        {
            string datePrefix = DateTime.Now.ToString("yyyyMMdd");

            const string sql = @"
                SELECT ISNULL(
                    MAX(TRY_CONVERT(INT, RIGHT(Id, 4))),
                    0
                )
                FROM dbo.OrderHeader WITH (UPDLOCK, HOLDLOCK)
                WHERE CAST([Date] AS DATE) = CAST(GETDATE() AS DATE);";

            using var cmd = new SqlCommand(
                sql,
                conn,
                transaction
            );

            object? result = cmd.ExecuteScalar();

            int lastNumber =
                result == null || result == DBNull.Value
                    ? 0
                    : Convert.ToInt32(result);

            if (lastNumber >= 9999)
            {
                throw new InvalidOperationException(
                    "Nomor urut pesanan untuk hari ini sudah penuh."
                );
            }

            return datePrefix + (lastNumber + 1).ToString("D4");
        }
    }

    // =========================================================
    // MODEL PILIHAN MEMBER
    // =========================================================
    public sealed class MemberOption
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public override string ToString()
        {
            return $"{Id} - {Name}";
        }
    }
}
