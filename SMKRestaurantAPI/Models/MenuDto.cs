namespace SMKRestaurantAPI.Models
{
    /// <summary>
    /// DTO menu yang dikirim ke client.
    /// </summary>
    public class MenuDto
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Price { get; set; }
    }
}
