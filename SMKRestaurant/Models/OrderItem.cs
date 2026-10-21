namespace SMKRestaurant.Models
{
    public class OrderItem
    {
        public int MenuId { get; set; }
        public string Name { get; set; } = "";
        public int Price { get; set; }
        public int Carbo { get; set; }
        public int Protein { get; set; }
        public int Qty { get; set; }
        public string Photo { get; set; } = "";

        public int Total => Price * Qty;
    }
}
