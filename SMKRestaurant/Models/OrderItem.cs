namespace SMKRestaurant.Models
{
    public class OrderItem
    {
        // Store the menu ID
        public int MenuId { get; set; }

        // Store the menu name
        public string Name { get; set; } = "";

        // Store the menu unit price
        public int Price { get; set; }

        // Store carbohydrate per menu
        public int Carbo { get; set; }

        // Store protein per menu
        public int Protein { get; set; }

        // Store the ordered quantity
        public int Qty { get; set; }
    }
}