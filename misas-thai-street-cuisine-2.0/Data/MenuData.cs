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
                Price = 42.00m,
                Serves = 2,
                Description = "Dinner for two, four, or more.",
                ImageUrl = "/Resources/Images/HomepageCarousel/ms-platter.png"
            },
            new Platter
            {
                Name = "Sausage Combo",
                Category = "Platter",
                Price = 35.00m,
                Serves = 2,
                Description = "Savory sausage platter.",
                ImageUrl = "/Resources/Images/HomepageCarousel/sausage-combo.png"
            },
            new Platter
            {
                Name = "Chicken Combo",
                Category = "Platter",
                Price = 39.00m,
                Serves = 2,
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
                Price = 3.50m,
                Description = "Fresh and spicy salad.",
                ImageUrl = "/Resources/Images/HomepageCarousel/papaya-salad.png"
            }
        };
    }
}
