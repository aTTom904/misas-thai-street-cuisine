namespace Models
{
    public class SausageType : ICartItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

    }

    public class SideDish : ICartItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Platter : ICartItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}