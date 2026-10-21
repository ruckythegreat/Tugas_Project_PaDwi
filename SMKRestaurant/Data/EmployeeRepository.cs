using Microsoft.Data.SqlClient;
using SMKRestaurant.Helpers;
using SMKRestaurant.Models;

namespace SMKRestaurant.Data
{
    public class EmployeeRepository
    {
        public EmployeeLoginResult? Authenticate(string email, string password)
        {
            const string sql = @"
                SELECT Id, Name, Position
                FROM MsEmployee
                WHERE Email = @Email AND Password = @Password";

            using var conn = Database.GetConnection();
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new EmployeeLoginResult
            {
                Id = reader["Id"].ToString() ?? "",
                Name = reader["Name"].ToString() ?? "",
                Position = reader["Position"].ToString() ?? ""
            };
        }
    }
}
