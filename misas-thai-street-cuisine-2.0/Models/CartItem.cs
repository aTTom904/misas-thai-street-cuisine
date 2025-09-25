namespace misas_thai_street_cuisine_2._0.Models
{

    public interface ICartItem
    {
        string Name { get; }
        string Category { get; }
    }

    public class CartItem
    {
        public ICartItem Item { get; set; }
        public int Quantity { get; set; }
        // For platters, which serving size was selected (null for sides)
        public int? SelectedServes { get; set; }

        public CartItem(ICartItem item, int quantity, int? selectedServes = null)
        {
            Item = item;
            Quantity = quantity;
            SelectedServes = selectedServes;
        }

        public decimal GetUnitPrice()
        {
            // Platter: look up price by serving size
            if (Item is Platter platter && SelectedServes.HasValue)
            {
                if (platter.Prices != null && platter.Prices.TryGetValue(SelectedServes.Value, out var price))
                    return price;
            }
            // SideDish: use its Price property
            if (Item is SideDish side)
                return side.Price;
            return 0m;
        }

        public decimal GetTotalPrice()
        {
            return GetUnitPrice() * Quantity;
        }
    }
}
