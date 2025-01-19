using Aspear.Domain.ValueObjects;

namespace Aspear.Domain.Tickets;

public class TicketOption
{
    public Guid TicketId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Price Price { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}