namespace Core.Entities.OrderAggregate;

public enum OrderStatus
{
    Pending,
    Failed,
    OnHold,
    InProgress,
    Completed
}