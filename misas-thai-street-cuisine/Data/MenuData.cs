using Models;

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
                Price = 10.50m,
            },
            new SausageType
            {
                Name =  "Beef",
                Category = "Sausage",
                Price = 10.75m,
            },
            new SausageType
            {
                Name =  "Turkey",
                Category = "Sausage",
                Price = 10.00m,
            },
            new SausageType
            {
                Name =  "Mushroom",
                Category = "Sausage",
                Price = 11.00m,
            },
        };

        public static SideDish[] SideDishes { get; } = new SideDish[]
        {
            new SideDish
            {
                Name="Sticky Rice",
                Category = "Side",
                Price= 02.95m
            },
            new SideDish
            {
                Name="Chili Paste (Prik Noom)",
                Category = "Side",
                Price= 02.00m
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
            new Platter
            {
                Name = "Beef",
                Category = "Platter",
                Price = 25.00m
            },
            new Platter
            {
                Name = "Turkey",
                Category = "Platter",
                Price = 24.00m
            },
            new Platter
            {
                Name = "Mushroom",
                Category = "Platter",
                Price = 27.25m
            },
        };
    }
}
