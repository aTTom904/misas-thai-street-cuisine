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

        public int UpgradePhadThai24Qty { get; set; }
        public int UpgradePhadThai48Qty { get; set; }

        public CartItem(ICartItem item, int quantity, int? selectedServes = null, int upgradePhadThai24Qty = 0, int upgradePhadThai48Qty = 0)
        {
            Item = item;
            Quantity = quantity;
            SelectedServes = selectedServes;
            UpgradePhadThai24Qty = upgradePhadThai24Qty;
            UpgradePhadThai48Qty = upgradePhadThai48Qty;
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
            decimal total = GetUnitPrice() * Quantity;
            total += UpgradePhadThai24Qty * 9m;
            total += UpgradePhadThai48Qty * 18m;
            return total;
        }
    }
}
