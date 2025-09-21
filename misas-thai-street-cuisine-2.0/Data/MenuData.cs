using misas_thai_street_cuisine_2._0.Models;

namespace Data
{
    public static class MenuData
    {
        public static SausageType[] SausageTypes { get; } = new SausageType[]
        {
            new SausageType
            {
                Name =  "Pork",
                Category = "Sausage",
                Price = 11.00m,
            },
//            new SausageType
//            {
//                Name =  "Beef",
//                Category = "Sausage",
//                Price = 10.75m,
//            },
           // new SausageType
           // {
           //     Name =  "Turkey",
           //     Category = "Sausage",
           //     Price = 11.00m,
           //},
//           new SausageType
//            {
//                Name =  "Mushroom",
//                Category = "Sausage",
//                Price = 11.00m,
//            },
        };

        public static SideDish[] SideDishes { get; } = new SideDish[]
        {
            new SideDish
            {
                Name="Extra Sausage",
                Category = "Side",
                Price= 08.25m
            },
            new SideDish
            {
                Name="Sticky Rice",
                Category = "Side",
                Price= 02.00m
            },
            new SideDish
            {
                Name="Prik Noom Chili Paste",
                Category = "Side",
                Price= 03.55m
            },
            new SideDish
            {
                Name="Curry Sauce",
                Category = "Side",
                Price = 02.25m
            }
        };

        public static Platter[] Platters { get; } = new Platter[]
        {
            new Platter
            {
               Name = "Pork",
               Category = "Platter",
               Price = 25.00m
            },
//            new Platter
//            {
//                Name = "Beef",
//                Category = "Platter",
//                Price = 25.00m
//            },
            //new Platter
            //{
            //    Name = "Turkey",
            //    Category = "Platter",
            //    Price = 24.00m
            //},
//            new Platter
//           {
//                Name = "Mushroom",
//                Category = "Platter",
//                Price = 27.25m
//            },
        };
    }
}
