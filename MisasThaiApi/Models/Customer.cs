namespace MisasThaiApi.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool ConsentToUpdates { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastOrderDate { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
}