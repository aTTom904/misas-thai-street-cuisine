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
                Description = "A perfect pairing of our Northern Thai Sai Ua sausage, packed with bold lemongrass and fragrant herbs, and our grilled chicken, marinated overnight in lemongrass, cilantro roots, garlic, coriander seeds, and soy. Served with both Prik Noom and Jao dips, plus sticky rice and fresh, pre-washed greens. A copmlete Thai street-style experience. ",
                ImageUrl = "/Resources/Images/HomepageCarousel/ms-platter.png",
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
                Prices = new Dictionary<int, decimal> { { 2, 35.00m }, { 4, 70.00m } },
                Description = "Our signature Northern Thai Sai Ua sausage is packed with bold lemongrass and fragrant herbs, then grilled to perfection. Paired with our house-made Prik Noom roasted chili dip, sticky rice, and fresh, pre-wased greens, it's a traditional Thai finger-food platter with layers of flavor.",
                ImageUrl = "/Resources/Images/HomepageCarousel/sausage-combo.png",
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
                Prices = new Dictionary<int, decimal> { { 2, 39.00m }, { 4, 78.00m } },
                Description = "Juicy chicken skewers and wings, marinated overnight in lemongrass, cilantro roots, garlic, coriander seeds and soy, then grilled to aromatic, tender perfection. Served with our house-made Jao dip, sticky rice, and fresh greens, making every bite a taste of authentic Thai street cuisine.",
                ImageUrl = "/Resources/Images/HomepageCarousel/chicken-platter.png",
                Includes = new Dictionary<int, string> {
                    {2, string.Join(" \u2022 ", ["4 Skewers", "2 Wings", "2 Sticky rice", "2 Dips", "Fresh Greens"])},
                    {4, string.Join(" \u2022 ", ["8 Skewers", "4 Wings", "4 Sticky rice", "4 Dips", "Fresh Greens"])}
                },
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
