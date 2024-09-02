namespace Prep4IELTS.Business.Models;

public class MomoPaymentItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public long Price { get; set; } 
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public long TotalPrice { get; set; }
}