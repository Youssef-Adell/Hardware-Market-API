using System.Runtime.Serialization;

namespace Core.Entities.OrderAggregate;

public enum OrderStatus
{
    // [EnumMember(Value = "Pending")]
    Pending,

    // [EnumMember(Value = "Failed")]
    Failed,

    // [EnumMember(Value = "On-Hold")]
    OnHold,

    // [EnumMember(Value = "In-Progress")]
    InProgress,

    // [EnumMember(Value = "Completed")]
    Completed
}