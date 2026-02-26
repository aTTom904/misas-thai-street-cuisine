using misas_thai_street_cuisine_2._0.Models;
using Square.Invoices;

namespace misas_thai_street_cuisine_2._0.Data
{
    public class MenuData
    {
        // Phad Thai upgrade pricing
        public static decimal PhadThai24ozUpgradePrice { get; } = 12.00m;
        public static decimal PhadThai48ozUpgradePrice { get; } = 24.00m;

        public static Platter[] Platters { get; } = new Platter[]
        {
            new Platter
            {
                Name = "M's Combo",
                Category = "Platter",
                Serves = new int[] { 2, 4 },
                Prices = new Dictionary<int, decimal> { { 2, 48.00m }, { 4, 96.00m } },
                Description = "A perfect pairing of our Northern Thai Sai Ua sausage, packed with bold lemongrass and fragrant herbs, and our grilled chicken, marinated overnight in lemongrass, cilantro roots, garlic, coriander seeds, and soy. Served with both Prik Noom and Jao dips, plus sticky rice and fresh, pre-washed greens. A complete Thai street-style experience. ",
                ImageUrl = "/Resources/Images/HomepageCarousel/ms-platter.webp",
                Includes = new Dictionary<int, string> {
                    {2, string.Join(" \u2022 ", ["2 Sausages", "2 Skewers", "2 Wings", "2 Sticky rice", "2 Dips", "Fresh Greens"])},
                    {4, string.Join(" \u2022 ", ["4 Sausages", "4 Skewers", "4 Wings", "4 Sticky rice", "4 Dips", "Fresh Greens"])}
                },
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
                Prices = new Dictionary<int, decimal> { { 2, 39.00m }, { 4, 78.00m } },
                Description = "Our signature Northern Thai Sai Ua sausage is packed with bold lemongrass and fragrant herbs, then grilled to perfection. Paired with our house-made Prik Noom roasted chili dip, sticky rice, and fresh, pre-wased greens, it's a traditional Thai finger-food platter with layers of flavor.",
                ImageUrl = "/Resources/Images/HomepageCarousel/sausage-combo.webp",
                Includes = new Dictionary<int, string> {
                    {2, string.Join(" \u2022 ", ["4 Sausages", "2 Sticky rice", "2 Dips", "Fresh Greens"])},
                    {4, string.Join(" \u2022 ", ["8 Sausages", "4 Sticky rice", "4 Dips", "Fresh Greens"])}
                },
                AddOns = new List<string> {
                    "Prik Noom Sauce – A roasted pepper, shallot, and garlic blend, perfect with the sausage."
                }
            },
            new Platter
            {
                Name = "Chicken Combo",
                Category = "Platter",
                Serves = new int[] { 2, 4 },
                Prices = new Dictionary<int, decimal> { { 2, 49.00m }, { 4, 98.00m } },
                Description = "Juicy chicken skewers and wings, marinated overnight in lemongrass, cilantro roots, garlic, coriander seeds and soy, then grilled to aromatic, tender perfection. Served with our house-made Jao dip, sticky rice, and fresh greens, making every bite a taste of authentic Thai street cuisine.",
                ImageUrl = "/Resources/Images/HomepageCarousel/chicken-platter.webp",
                Includes = new Dictionary<int, string> {
                    {2, string.Join(" \u2022 ", ["4 Skewers", "2 Wings", "2 Sticky rice", "2 Dips", "Fresh Greens"])},
                    {4, string.Join(" \u2022 ", ["8 Skewers", "4 Wings", "4 Sticky rice", "4 Dips", "Fresh Greens"])}
                },
                AddOns = new List<string> {
                    "Jao Sauce – Our tangy, slightly sweet-and-savory dip made with tamarind, palm sugar, fish sauce, cilantro, and shallots, ideal with the chicken."
                }
            }
        };

        public static Tray[] Trays { get; } = new Tray[]
        {
            new Tray
            {
                Name = "Chicken Skewers",
                Category = "Tray",
                Size = new string[] { "Half", "Full" },
                Prices = new Dictionary<string, decimal> { { "Half", 155.00m }, { "Full", 295.00m } },
                Description = "Juicy chicken skewers, marinated in lemongrass, cilantro roots, garlic, coriander seeds and soy, then grilled to aromatic, tender perfection.",
                ImageUrl = "/Resources/Images/Food/chicken-skewer-tray.webp",
                Includes = new Dictionary<string, string> {
                    {"Half", string.Join(" \u2022 ", ["25 Skewers"])},
                    {"Full", string.Join(" \u2022 ", ["50 Skewers"])}
                },
                Serves = new Dictionary<string, string> {
                    {"Half", "20-25"},
                    {"Full", "45-50"}
                },
                AddOns = new List<string> {
                    "Jao Sauce – Our tangy, slightly sweet-and-savory dip made with tamarind, palm sugar, fish sauce, cilantro, and shallots, and topped with toaEsted rice.",
                }
            },
            new Tray
            {
                Name = "Sai Ua Sausage",
                Category = "Tray",
                Size = new string[] { "Half", "Full" },
                Prices = new Dictionary<string, decimal> { { "Half", 110.00m }, { "Full", 215.00m } },
                Description = "Our signature Northern Thai Sai Ua sausage is packed with fresh lemongrass and fragrant herbs, then grilled to perfection. Intensely aromatic and perfectly seasoned, our Sai Ua offers a bold yet balanced flavor that's unmistakably Thai.",
                ImageUrl = "/Resources/Images/Food/sausage-tray.webp",
                Includes = new Dictionary<string, string> {
                    {"Half", string.Join(" \u2022 ", ["20 Sausage Links"])},
                    {"Full", string.Join(" \u2022 ", ["40 Sausage Links"])}
                },
                Serves = new Dictionary<string, string> {
                    {"Half", "15-20"},
                    {"Full", "30-35"}
                },
                AddOns = new List<string> {
                    "Prik Noom Sauce – A roasted pepper, shallot, and garlic blend, perfectly paired with our Sai Ua sausage.",
                }
            },
            new Tray
            {
                Name = "Thai Style Wings",
                Category = "Tray",
                Size = new string[] { "Half", "Full" },
                Prices = new Dictionary<string, decimal> { { "Half", 90.00m }, { "Full", 175.00m } },
                Description = "Crispy-skinned chicken wings marinated in lemongrass, cilantro roots, garlic, coriander seeds and soy, then grilled to juicy perfection.",
                ImageUrl = "/Resources/Images/Food/wings-tray.webp",
                Includes = new Dictionary<string, string> {
                    {"Half", string.Join(" \u2022 ", ["25 Wings"])},
                    {"Full", string.Join(" \u2022 ", ["50 Wings"])}
                },
                Serves = new Dictionary<string, string> {
                    {"Half", "12-15"},
                    {"Full", "25-30"}
                },
                AddOns = new List<string> {
                    "Jao Sauce – Our tangy, slightly sweet-and-savory dip made with tamarind, palm sugar, fish sauce, cilantro, and shallots, and topped with toasted rice.",
                }
            },
            new Tray
            {
                Name = "Phad Thai",
                Category = "Tray",
                Size = new string[] { "Half", "Full" },
                Prices = new Dictionary<string, decimal> { { "Half", 95.00m }, { "Full", 180.00m } },
                Description = "Classic Thai noodles stir-fried with eggs, tofu, bean sprouts,  green onions, garnished with crushed peanuts and lime wedges. *Contains shellfish, eggs, soy*",
                ImageUrl = "/Resources/Images/Food/phad-thai-tray.webp",
                Includes = new Dictionary<string, string> {
                    {"Half", string.Join(" \u2022 ", ["Half Tray of Phad Thai", "Crushed Peanuts", "Lime Wedges", "Bean Sprouts"])},
                    {"Full", string.Join(" \u2022 ", ["Full Tray of Phad Thai", "Crushed Peanuts", "Lime Wedges", "Bean Sprouts"])}
                },
                Serves = new Dictionary<string, string> {
                    {"Half", "6-8"},
                    {"Full", "12-15"}
                },
                AddOns = new List<string> {
                    "Extra Lime & Peanuts – Additional garnishes for those who love extra zing and crunch."
                }
            },
            new Tray
            {
                Name = "Sticky Rice",
                Category = "Tray",
                Size = new string[] { "Half", "Full" },
                Prices = new Dictionary<string, decimal> { { "Half", 38.00m }, { "Full", 75.00m } },
                Description = "Steamed sticky rice, the perfect complement to any of our grilled dishes.",
                ImageUrl = "/Resources/Images/Food/sticky-rice-tray.webp",
                Includes = new Dictionary<string, string> {
                    {"Half", string.Join(" \u2022 ", ["Half Tray of Sticky Rice"])},
                    {"Full", string.Join(" \u2022 ", ["Full Tray of Sticky Rice"])}
                },
                Serves = new Dictionary<string, string> {
                    {"Half", "10-12"},
                    {"Full", "20-25"}
                },
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
                ImageUrl = "/Resources/Images/Food/ms-sausage.webp"
            },
            new SideDish
            {
                Name = "Chicken Skewer",
                Category = "Side",
                Price = 7.00m,
                Description = "1 Tender grilled chicken skewer.",
                ImageUrl = "/Resources/Images/Food/grilled-chicken-skewer.webp"
            },
            new SideDish
            {
                Name = "Chicken Wing",
                Category = "Side",
                Price = 4.00m,
                Description = "1 grilled, whole-style chicken wing.",
                ImageUrl = "/Resources/Images/Food/grilled-chicken-wing.webp"
            },
            new SideDish
            {
                Name = "Phad Thai",
                Category = "Side",
                Price = 23.00m,
                Description = "24oz Classic Thai noodles. Feeds 1-2. *Contains shellfish, eggs, soy*",
                ImageUrl = "/Resources/Images/Food/phad-thai.webp"
            },
            new SideDish
            {
                Name = "Papaya Salad",
                Category = "Side",
                Price = 6.00m,
                Description = "Fresh and spicy salad.",
                ImageUrl = "/Resources/Images/Food/papaya-salad.webp"
            },
            new SideDish
            {
                Name = "Sticky Rice",
                Category = "Side",
                Price = 4.00m,
                Description = "Steamed sticky rice.",
                ImageUrl = "/Resources/Images/Food/sticky-rice.webp"
            },
        };
    }
}
