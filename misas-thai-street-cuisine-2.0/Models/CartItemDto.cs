namespace misas_thai_street_cuisine_2._0.Models
{
    public class CartItemDto
    {
        public string ItemName { get; set; } = "";
        public string Category { get; set; } = "";
        public int Quantity { get; set; }
        public int? SelectedServes { get; set; }
        public string? SelectedSize { get; set; }
        public int UpgradePhadThai24Qty { get; set; }
        public int UpgradePhadThai48Qty { get; set; }
        public int AddOnQty { get; set; }
    }
}