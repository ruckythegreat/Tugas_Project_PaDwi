namespace SMKRestaurant.Models
{
    public class MenuInput
    {
        public string Name { get; set; } = "";
        public int Price { get; set; }
        public string Photo { get; set; } = "";
        public int Carbo { get; set; }
        public int Protein { get; set; }
    }
}
