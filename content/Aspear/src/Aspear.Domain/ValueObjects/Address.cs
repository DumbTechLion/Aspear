namespace Aspear.Domain.ValueObjects;

public record Address(
    string StreetLine1,
    string StreetLine2,
    string City,
    string StateOrProvince,
    string PostalCode,
    string Country);