namespace Testt.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public string Status { get; set; } = null!;
    public ClientDto Client { get; set; } = null!;
    public List<ProductDto> Products { get; set; } = [];
}
