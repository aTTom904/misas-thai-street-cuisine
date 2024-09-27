using Models;

namespace Data
{
    public class PackageData
    {
        public static Package[] Packages { get; } = new Package[]
        {
            new Package
            {
                Name = "Small Gathering",
                Description = "Good for hosting a few good friends",
                Servings = "8-10",
                Entrees = 2,
                Appetizers = 2,
                Price = 0.00m
            },
            new Package
            {
                Name = "Shindig",
                Description = "For a fuller crowd",
                Servings = "10-15",
                Entrees = 3,
                Appetizers = 3,
                Price = 0.00m
            },
            new Package
            {
                Name = "Suaree",
                Description = "Feed the masses",
                Servings = "15-20",
                Entrees = 4,
                Appetizers = 4,
                Price = 0.00m
            }
        };
    }
}