namespace misas_thai_street_cuisine_2._0.Data;

public static class HeroContent
{
    public static readonly List<(string Title, string Subtitle)> Options = new()
    {
        (
            "The<br />Weeknight<br />Bite",
            "From Our Grill to Your Table.<br />Authentic Thai Flavor, Fresh Every Time."
        ),
        (
            "Dinner<br />Done<br />Right",
            "Bold Thai Flavors, Cooked Fresh.<br />Delivered Straight to Your Door."
        ),
        (
            "Fresh Thai,<br />Your Way",
            "Skip the Takeout Line.<br />We Bring the Flavor to You."
        ),
        (
            "Flavor<br />Without<br />the Fuss",
            "Authentic Thai Food, Family-Style<br />Made From Scratch Every Time."
        ),
        (
            "Thai<br />Night:<br />Sorted",
            "Homestyle Thai Cooking.<br />Delivered Hot to Your Table."
        ),
        (
            "Crave It.<br />Order It.<br />Done.",
            "Bold Thai Street Food.<br />From Our Kitchen to Your Door."
        ),
        (
            "Eat Like<br />You Mean It",
            "Big Flavor. Zero Effort.<br />Thai Street Food, Your Way."
        ),
        (
            "Spice Up<br />Your<br />Weeknight",
            "Forget the Usual Routine.<br />Real Thai Food, Made Fresh."
        ),
        (
            "Your<br />Table<br />Awaits",
            "Family-Style Thai Food Delivered.<br />Just Sit Down and Dig In."
        )
    };

    private static readonly Random _random = new();

    public static (string Title, string Subtitle) GetRandom()
    {
        return Options[_random.Next(Options.Count)];
    }
}
