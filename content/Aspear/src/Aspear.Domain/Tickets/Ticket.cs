namespace Aspear.Domain.Tickets;

public class Ticket : AggregateRoot
{
    private readonly List<TicketOption> _options = [];

    public Guid? PaymentId { get; set; }
    public IReadOnlyList<TicketOption> Options => _options.AsReadOnly();
}