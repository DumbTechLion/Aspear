using Aspear.Domain.ValueObjects;

namespace Aspear.Domain.Payments;

public class Payment : AggregateRoot
{
    public Price Price { get; set; }
    public PaymentStatus Status { get; set; }
}