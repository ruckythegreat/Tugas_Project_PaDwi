using System.Data;
using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;
using SMKRestaurant.Models;

namespace SMKRestaurant.Data
{
    public class MemberRepository
    {
        public DataTable GetAll()
        {
            const string sql = "SELECT Id, Name, Email, Handphone, JoinDate FROM MsMember";

            using var conn = Database.GetConnection();
            using var adapter = new SqlDataAdapter(sql, conn);

            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetLookup()
        {
            const string sql = "SELECT Id, Name FROM MsMember";

            using var conn = Database.GetConnection();
            using var adapter = new SqlDataAdapter(sql, conn);

            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable Search(string keyword)
        {
            const string sql = @"
                SELECT Id, Name, Email, Handphone, JoinDate
                FROM MsMember
                WHERE Name LIKE @Keyword
                   OR Email LIKE @Keyword
                   OR Handphone LIKE @Keyword
                   OR CAST(Id AS NVARCHAR(20)) LIKE @Keyword
                ORDER BY Id";

            using var conn = Database.GetConnection();
            using var adapter = new SqlDataAdapter(sql, conn);
            adapter.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public void Insert(MemberInput member)
        {
            const string sql = @"
                INSERT INTO MsMember (Name, Email, Handphone, JoinDate)
                VALUES (@Name, @Email, @Handphone, @JoinDate)";

            ExecuteSave(sql, member, id: null);
        }

        public void Update(int id, MemberInput member)
        {
            const string sql = @"
                UPDATE MsMember
                SET Name = @Name,
                    Email = @Email,
                    Handphone = @Handphone,
                    JoinDate = @JoinDate
                WHERE Id = @Id";

            ExecuteSave(sql, member, id);
        }

        public void Delete(int id)
        {
            const string sql = "DELETE FROM MsMember WHERE Id = @Id";

            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private static void ExecuteSave(string sql, MemberInput member, int? id)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Name", member.Name);
            cmd.Parameters.AddWithValue("@Email", member.Email);
            cmd.Parameters.AddWithValue("@Handphone", member.Handphone);
            cmd.Parameters.AddWithValue("@JoinDate", member.JoinDate.Date);

            if (id.HasValue)
                cmd.Parameters.AddWithValue("@Id", id.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
