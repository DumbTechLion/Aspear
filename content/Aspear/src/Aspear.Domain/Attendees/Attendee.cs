namespace Aspear.Domain.Attendees;

public class Attendee : AggregateRoot
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string Pseudo { get; set; }
}