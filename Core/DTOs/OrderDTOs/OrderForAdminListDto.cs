namespace Core.DTOs.OrderDTOs;

public class OrderForAdminListDto
{
    public int Id { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; }
    public double Total { get; set; }
}