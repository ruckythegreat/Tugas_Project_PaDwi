using Microsoft.Data.SqlClient;
using SMKRestaurant.Shared.Data;
using SMKRestaurantAPI.Models;

namespace SMKRestaurantAPI.Data
{
    /// <summary>
    /// Akses data MsMenu — satu-satunya tempat SQL untuk menu di API.
    /// </summary>
    public class MenuRepository : IMenuRepository
    {
        private const string SelectAllSql = @"
            SELECT
                Name,
                ISNULL(Carbo, 0) AS Carbo,
                ISNULL(Protein, 0) AS Protein,
                ISNULL(Price, 0) AS Price
            FROM MsMenu";

        public async Task<IReadOnlyList<MenuDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var menus = new List<MenuDto>();

            await using var conn = DbConnectionFactory.Create();
            await using var cmd = new SqlCommand(SelectAllSql, conn);

            await conn.OpenAsync(cancellationToken);
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var name = reader["Name"] != DBNull.Value
                    ? reader["Name"].ToString() ?? ""
                    : "";

                var carbo = Convert.ToInt32(reader["Carbo"]);
                var protein = Convert.ToInt32(reader["Protein"]);
                var price = Convert.ToInt32(reader["Price"]);

                menus.Add(new MenuDto
                {
                    Name = name,
                    Description = $"Carbo : {carbo}, Protein {protein}",
                    Price = price
                });
            }

            return menus;
        }
    }
}
