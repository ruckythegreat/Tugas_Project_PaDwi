using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SMKRestaurantAPI.Models;

namespace SMKRestaurantAPI.Controllers
{
    [ApiController]
    [Route("menu")]
    public class MenuController : ControllerBase
    {
        // SQL Server connection
        private readonly string connectionString =
            @"Server=Naptune\SQLEXPRESS;
              Database=SMKRestaurant;
              Trusted_Connection=True;
              TrustServerCertificate=True;";

        [HttpGet]
        public IActionResult GetMenu()
        {
            // Check Authorization header
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return BadRequest(new
                {
                    message = "Authorization required"
                });
            }

            // Store menu data
            List<MenuResponse> menus =
                new List<MenuResponse>();

            // SQL query
            string sql = @"
                SELECT
                    Name,
                    ISNULL(Carbo, 0) AS Carbo,
                    ISNULL(Protein, 0) AS Protein,
                    ISNULL(Price, 0) AS Price
                FROM MsMenu";

            // Connect to SQL Server
            using (SqlConnection conn =
                new SqlConnection(connectionString))
            using (SqlCommand cmd =
                new SqlCommand(sql, conn))
            {
                // Open database connection
                conn.Open();

                // Execute query
                using (SqlDataReader reader =
                    cmd.ExecuteReader())
                {
                    // Read every menu
                    while (reader.Read())
                    {
                        string name =
                            reader["Name"] != DBNull.Value
                                ? reader["Name"].ToString()!
                                : "";

                        int carbo =
                            Convert.ToInt32(reader["Carbo"]);

                        int protein =
                            Convert.ToInt32(reader["Protein"]);

                        int price =
                            Convert.ToInt32(reader["Price"]);

                        // Add menu to response
                        menus.Add(new MenuResponse
                        {
                            Name = name,
                            Description =
                                $"Carbo : {carbo}, Protein {protein}",
                            Price = price
                        });
                    }
                }
            }

            // Return HTTP 200
            return Ok(menus);
        }
    }
}