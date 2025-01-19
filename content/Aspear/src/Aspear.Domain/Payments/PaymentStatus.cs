namespace Aspear.Domain.Payments;

public enum PaymentStatus
{
    Unknown,
    Pending,
    Failed,
    Cancelled,
    Paid,
    Refunded,
}