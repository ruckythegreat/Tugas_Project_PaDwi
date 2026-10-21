namespace SMKRestaurant.Models
{
    public class OrderDetailLine
    {
        public string Menu { get; set; } = "";
        public int Qty { get; set; }
        public int Price { get; set; }
        public int Total { get; set; }
    }
}
