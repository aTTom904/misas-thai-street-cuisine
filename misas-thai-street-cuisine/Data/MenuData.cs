using Models;

namespace Data
{
    public static class MenuData
    {
        public static MenuItem[] MenuItems { get; } = new MenuItem[]
        {
            new MenuItem
            {
                Name = "Sai Oua Sausage",
                Description = "The most falvorful and aromatic sausage you've ever had, and handmade from start to finish. A Northern Thai staple, this bold, traditional sausage gets its flavor from a blend of minced pork, lemongrass, kaffir lime leaves, galangal, garlic, and shallots, along with chili for a mild kick.",
                Category = "Appetizer",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/sai-oua.jpg"
            },

            new MenuItem
            {
                Name = "Lemongrass Chicken Wings",
                Description = "Fried chicken wings that are crispy, golden, and infused infused with the fragrant flavors of lemongrass, garlic, and chili, offering a perfect balance of zest and spice with each bite.",
                Category="Appetizer",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/lemongrass-wings.jpg"
            },

            new MenuItem
            {
                Name = "Bangkok Grilled Chicken",
                Description = "Served on skewers, this Bangkok-style grilled chicken boasts smoky, succulent flavors with a marinade of Thai herbs and spices. Paired with zesty jiao dipping sauce, this is a must-have flavor combination.",
                Category="Appetizer",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Larb (Pronounced LAAB)",
                Description = "A zesty Thai salad with finely minced meat, fresh herbs, lime juice, and savory fish sauce, all mixed with roasted, crushed rice for a bold, vibrant flavor. Served with crisp lettuce leaves for a refreshing crunch.",
                Category="Appetizer",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Som Tum Papaya Salad",
                Description = "A fiery Thai green papaya salad bursting with tangy lime, spicy chilies, savory fish sauce, and sweet palm sugar, complemented by crunchy peanuts and juicy tomatoes for a crisp, refreshing kick.",
                Category="Appetizer",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Ho Mok Steamed Curry",
                Description = "A steamed curry dish with tender fish, delicately spiced with red curry paste and fresh herbs, and infused with coconut milk. Served with rice.",
                Category="Entree",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Pad Thai",
                Description = "Thailand's national dish. A classic stir-fry of rice noodles, lightly coated in a tangy tamarind sauce, and tossed with chicken or tofu, eggs, bean sprouts, and roasted peanuts. Finished with a burst of lime and fresh cilantro, this dish offers a perfect blend of sweet, sour, and savory flavors. You can't go wrong.",
                Category="Entree",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Pad Basil",
                Description = "Thailand's REAL national dish. A savory stir-fry featuring slices of fried chicken or tofu, crisp vegetables, and aromatic basil leaves stir-fried in M's signature basil sauce. Served with steamed rice, it's a flavorful, satisfying dish with a perfect balance of heat and savory depth.",
                Category="Entree",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Khao Soi",
                Description = "A rich and aromatic Northern Thai curry noodle soup featuring fried chicken or tofu in a creamy, spicy coconut curry broth. Topped with crispy egg noodles, pickled mustard greens, and fresh cilantro for a satisfying, complex flavor experience.",
                Category="Entree",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "M's Rice",
                Description = "Fluffy, aromatic jasmine rice infused with M's signature flavors. A perfect compliment to just about any item on the menu.",
                Category="Side",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Pandan Coconut Cake",
                Description = "A moist, fragrant cake infused with aromatic pandan leaves, layered with a luscious sweet coconut icing. This tropical treat offers a unique blend of rich, buttery flavors and creamy sweetness for a delightful, exotic dessert.",
                Category="Dessert",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Mango Sticky Rice",
                Description = "A classic Thai dessert featuring sweet sticky rice topped with ripe, juicy mango slices and drizzled with a creamy coconut sauce. This indulgent treat combines the perfect balance of fresh and sweet for a perfect finish to any meal.",
                Category="Dessert",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },

            new MenuItem
            {
                Name = "Thai Tea",
                Description = "A bold and aromatic brew with robust black tea, infused with spices and sweetened with condensed milk, creating a rich, creamy, and slightly spiced drink that's both refreshing and indulgent.",
                Category="Beverage",
                Price = 0.00m,
                PhotoPath = "/Resources/Images/Food/placeholder.jpg"
            },
        };
    }
}
