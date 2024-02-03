namespace Core.Entities.OrderAggregate;

public enum OrderStatus
{
    Pending,
    Failed,
    Received,
    InProgress,
    Completed,
    Canceled
}