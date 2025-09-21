namespace misas_thai_street_cuisine_2._0.Models
{
    public class MenuItem : ICartItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }

    public class SausageType : ICartItem
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal Price { get; set; }

    }

    public class SideDish : ICartItem
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class Platter : ICartItem
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal Price { get; set; }
    }
}