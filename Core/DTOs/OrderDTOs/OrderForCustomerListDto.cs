namespace Core.DTOs.OrderDTOs;

public class OrderForCustomerListDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public double Total { get; set; }
}