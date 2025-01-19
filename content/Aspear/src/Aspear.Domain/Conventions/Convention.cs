namespace Aspear.Domain.Conventions;

public class Convention : AggregateRoot
{
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}