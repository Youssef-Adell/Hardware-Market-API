namespace Core.Entities.OrderAggregate;

public enum OrderStatus
{
    Pending,
    Orderd,
    InProgress,
    Completed,
    Canceled,
    Failed
}