using misas_thai_street_cuisine_2._0.Models;

namespace misas_thai_street_cuisine_2._0.Data
{
    public class MenuData
    {
        public static Platter[] Platters { get; } = new Platter[]
        {
            new Platter
            {
                Name = "M's Combo",
                Category = "Platter",
                Serves = new int[] { 2, 4 },
                Prices = new Dictionary<int, decimal> { { 2, 42.00m }, { 4, 84.00m } },
                Description = "Dinner for two, four, or more.",
                ImageUrl = "/Resources/Images/HomepageCarousel/ms-platter.png"
            },
            new Platter
            {
                Name = "Sausage Combo",
                Category = "Platter",
                Serves = new int[] { 2, 4 },
                Prices = new Dictionary<int, decimal> { { 2, 35.00m }, { 4, 70.00m } },
                Description = "Savory sausage platter.",
                ImageUrl = "/Resources/Images/HomepageCarousel/sausage-combo.png"
            },
            new Platter
            {
                Name = "Chicken Combo",
                Category = "Platter",
                Serves = new int[] { 2, 4 },
                Prices = new Dictionary<int, decimal> { { 2, 39.00m }, { 4, 78.00m } },
                Description = "Grilled chicken platter.",
                ImageUrl = "/Resources/Images/HomepageCarousel/chicken-platter.png"
            }
        };

        public static SideDish[] SideDishes { get; } = new SideDish[]
        {
            new SideDish
            {
                Name = "Phad Thai",
                Category = "Side",
                Price = 14.00m,
                Description = "Classic Thai noodles.",
                ImageUrl = "/Resources/Images/HomepageCarousel/phad-thai.png"
            },
            new SideDish
            {
                Name = "Papaya Salad",
                Category = "Side",
                Price = 4.00m,
                Description = "Fresh and spicy salad.",
                ImageUrl = "/Resources/Images/HomepageCarousel/papaya-salad.png"
            }
        };
    }
}
