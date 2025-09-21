namespace misas_thai_street_cuisine_2._0.Models
{
    public interface ICartItem
    {

        string Name { get; }
        string Category { get; }
        decimal Price { get; }
    }

    public class CartItem
    {
        public ICartItem Item { get; set; }
        public int Quantity { get; set; }

        public CartItem(ICartItem item, int quanitity)
        {
            Item  = item;
            Quantity = quanitity;
        }

        public decimal GetTotalPrice()
        {
            return Item.Price * Quantity;
        }
    }
}
