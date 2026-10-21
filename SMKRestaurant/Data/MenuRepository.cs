using System.Data;
using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;
using SMKRestaurant.Models;

namespace SMKRestaurant.Data
{
    public class MenuRepository
    {
        public DataTable GetAll()
        {
            const string sql = @"
                SELECT Id, Name, Price, Photo, Carbo, Protein
                FROM MsMenu
                ORDER BY Id";

            using var conn = Database.GetConnection();
            using var adapter = new SqlDataAdapter(sql, conn);

            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetAllForOrder()
        {
            const string sql = "SELECT Id, Name, Price, Carbo, Protein, Photo FROM MsMenu";

            using var conn = Database.GetConnection();
            using var adapter = new SqlDataAdapter(sql, conn);

            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable Search(string keyword)
        {
            const string sql = @"
                SELECT Id, Name, Price, Photo, Carbo, Protein
                FROM MsMenu
                WHERE Name LIKE @Keyword
                   OR CAST(Id AS NVARCHAR(20)) LIKE @Keyword
                ORDER BY Id";

            using var conn = Database.GetConnection();
            using var adapter = new SqlDataAdapter(sql, conn);
            adapter.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public OrderItem? GetById(int menuId)
        {
            const string sql = @"
                SELECT Id, Name, Price, Carbo, Protein, ISNULL(Photo, '') AS Photo
                FROM MsMenu
                WHERE Id = @Id";

            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", menuId);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            return new OrderItem
            {
                MenuId = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? "",
                Price = Convert.ToInt32(reader["Price"]),
                Carbo = Convert.ToInt32(reader["Carbo"]),
                Protein = Convert.ToInt32(reader["Protein"]),
                Photo = reader["Photo"].ToString() ?? "",
                Qty = 1
            };
        }

        public void Insert(MenuInput menu)
        {
            const string sql = @"
                INSERT INTO MsMenu (Name, Price, Photo, Carbo, Protein)
                VALUES (@Name, @Price, @Photo, @Carbo, @Protein)";

            ExecuteSave(sql, menu, id: null);
        }

        public void Update(int id, MenuInput menu)
        {
            const string sql = @"
                UPDATE MsMenu
                SET Name = @Name,
                    Price = @Price,
                    Photo = @Photo,
                    Carbo = @Carbo,
                    Protein = @Protein
                WHERE Id = @Id";

            ExecuteSave(sql, menu, id);
        }

        public void Delete(int id)
        {
            const string sql = "DELETE FROM MsMenu WHERE Id = @Id";

            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        private static void ExecuteSave(string sql, MenuInput menu, int? id)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Name", menu.Name);
            cmd.Parameters.AddWithValue("@Price", menu.Price);
            cmd.Parameters.AddWithValue("@Photo", menu.Photo);
            cmd.Parameters.AddWithValue("@Carbo", menu.Carbo);
            cmd.Parameters.AddWithValue("@Protein", menu.Protein);

            if (id.HasValue)
                cmd.Parameters.AddWithValue("@Id", id.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
