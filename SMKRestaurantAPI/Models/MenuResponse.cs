namespace SMKRestaurantAPI.Models
{
    // Model for menu API response
    public class MenuResponse
    {
        // Menu name
        public string Name { get; set; } = "";

        // Menu description
        public string Description { get; set; } = "";

        // Menu price
        public int Price { get; set; }
    }
}