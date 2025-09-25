namespace misas_thai_street_cuisine_2._0.Models
{
    public class MenuItem : ICartItem
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class Platter : MenuItem
    {
        public int[] Serves { get; set; }
        public Dictionary<int, decimal> Prices { get; set; }
    }

    public class SideDish : MenuItem
    {
        public decimal Price { get; set; }
    }
}