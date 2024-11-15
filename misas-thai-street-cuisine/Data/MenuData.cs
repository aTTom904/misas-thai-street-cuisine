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
                Price = 10.50m,
            },
            new SausageType
            {
                Name =  "Beef",
                Price = 10.75m,
            },
            new SausageType
            {
                Name =  "Turkey",
                Price = 10.00m,
            },
            new SausageType
            {
                Name =  "Mushroom",
                Price = 11.00m,
            },
        };

        public static SideDish[] SideDishes { get; } = new SideDish[]
        {
            new SideDish
            {
                Name="Sticky Rice",
                Price= 02.95m
            },
            new SideDish
            {
                Name="Chili Paste (Prik Noom)",
                Price= 02.00m
            }
        };

        public static Platter[] Platters { get; } = new Platter[]
        {
            new Platter
            {
               Name = "Pork or Beef",
               Price = 25.00m
            },
            new Platter
            {
                Name = "Turkey",
                Price = 24.00m
            },
            new Platter
            {
                Name = "Mushroom",
                Price = 27.25m
            },
        };
    }
}
