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
                Description = "A balanced mix of smokey Thai sausage, tender grilled chicken skewers, and flavorful Thai-style wings.",
                ImageUrl = "/Resources/Images/HomepageCarousel/ms-platter.png",
                Includes = new List<string> { "2 Sausages", "2 Skewers", "2 Wings", "2 Sticky rice", "2 Dips", "Fresh Greens" },
                AddOns = new List<string> {
                    "Prik Noom Sauce – A roasted pepper, shallot, and garlic blend, perfect with the sausage.",
                    "Jao Sauce – Our tangy, slightly sweet-and-savory dip made with tamarind, palm sugar, fish sauce, cilantro, and shallots, ideal with the chicken."
                }
            },
            new Platter
            {
                Name = "Sausage Combo",
                Category = "Platter",
                Serves = new int[] { 2, 4 },
                Prices = new Dictionary<int, decimal> { { 2, 35.00m }, { 4, 70.00m } },
                Description = "Our signature Northern Thai Sai Ua—bold with lemongrass, herbs, and chili, grilled to perfection.",
                ImageUrl = "/Resources/Images/HomepageCarousel/sausage-combo.png",
                Includes = new List<string> { "4 Sausages", "2 Sticky rice", "2 Dips", "Fresh Greens" },
                AddOns = new List<string> {
                    "Prik Noom Sauce – A roasted pepper, shallot, and garlic blend, perfect with the sausage."
                }
            },
            new Platter
            {
                Name = "Chicken Combo",
                Category = "Platter",
                Serves = new int[] { 2, 4 },
                Prices = new Dictionary<int, decimal> { { 2, 39.00m }, { 4, 78.00m } },
                Description = "Tender grilled chicken skewers and Thai-style wings.",
                ImageUrl = "/Resources/Images/HomepageCarousel/chicken-platter.png",
                Includes = new List<string> { "4 Skewers", "2 Wings", "2 Sticky rice", "2 Dips", "Fresh Greens" },
                AddOns = new List<string> {
                    "Jao Sauce – Our tangy, slightly sweet-and-savory dip made with tamarind, palm sugar, fish sauce, cilantro, and shallots, ideal with the chicken."
                }
            }
        };

        public static SideDish[] SideDishes { get; } = new SideDish[]
        {
            new SideDish
            {
                Name = "M's Thai Sausage",
                Category = "Side",
                Price = 6.00m,
                Description = "1 Grilled Thai sausage link infused with herbs and spices.",
                ImageUrl = "/Resources/Images/Food/ms-sausage.jpg"
            },
            new SideDish
            {
                Name = "Chicken Skewer",
                Category = "Side",
                Price = 5.00m,
                Description = "1 Tender grilled chicken skewer.",
                ImageUrl = "/Resources/Images/Food/grilled-chicken-skewer.jpg"
            },
            new SideDish
            {
                Name = "Chicken Wing",
                Category = "Side",
                Price = 3.00m,
                Description = "1 grilled, whole-style chicken wing.",
                ImageUrl = "/Resources/Images/Food/grilled-chicken-wing.jpg"
            },
            new SideDish
            {
                Name = "Phad Thai for 2",
                Category = "Side",
                Price = 17.00m,
                Description = "Classic Thai noodles.",
                ImageUrl = "/Resources/Images/Food/phad-thai.jpg"
            },
            new SideDish
            {
                Name = "Papaya Salad",
                Category = "Side",
                Price = 6.00m,
                Description = "Fresh and spicy salad.",
                ImageUrl = "/Resources/Images/Food/papaya-salad.jpg"
            },
            new SideDish
            {
                Name = "Sticky Rice",
                Category = "Side",
                Price = 4.00m,
                Description = "Steamed sticky rice.",
                ImageUrl = "/Resources/Images/Food/sticky-rice.jpg"
            },
        };
    }
}
