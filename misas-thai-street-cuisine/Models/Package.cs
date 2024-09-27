namespace Models
{
    public class Package : ICartItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Servings { get; set; }
        public int Entrees {  get; set; }
        public int Appetizers { get; set; }
        public decimal Price { get; set; }
    }
}
