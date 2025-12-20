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
        // For trays, which size was selected (null for non-trays)
        public string? SelectedSize { get; set; }

        public int UpgradePhadThai24Qty { get; set; }
        public int UpgradePhadThai48Qty { get; set; }
        // For trays, add-on quantity (sauce, etc.)
        public int AddOnQty { get; set; }

        // Constructor for platters
        public CartItem(ICartItem item, int quantity, int? selectedServes = null, int upgradePhadThai24Qty = 0, int upgradePhadThai48Qty = 0)
        {
            Item = item;
            Quantity = quantity;
            SelectedServes = selectedServes;
            UpgradePhadThai24Qty = upgradePhadThai24Qty;
            UpgradePhadThai48Qty = upgradePhadThai48Qty;
            AddOnQty = 0;
        }

        // Constructor for trays
        public CartItem(Tray tray, int quantity, string selectedSize, int addOnQty = 0)
        {
            Item = tray;
            Quantity = quantity;
            SelectedSize = selectedSize;
            AddOnQty = addOnQty;
        }

        public decimal GetUnitPrice()
        {
            // Platter: look up price by serving size
            if (Item is Platter platter && SelectedServes.HasValue)
            {
                if (platter.Prices != null && platter.Prices.TryGetValue(SelectedServes.Value, out var price))
                    return price;
            }
            // Tray: look up price by size
            if (Item is Tray tray && SelectedSize != null)
            {
                if (tray.Prices != null && tray.Prices.TryGetValue(SelectedSize, out var price))
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
            
            if (Item is Tray && SelectedSize != null)
            {
                decimal addOnPrice = SelectedSize == "Half" ? 15m : 25m;
                total += AddOnQty * addOnPrice;
            }
            
            return total;
        }
    }
}
