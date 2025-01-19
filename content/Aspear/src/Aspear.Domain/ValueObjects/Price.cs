using Aspear.Domain.Enumerations;

namespace Aspear.Domain.ValueObjects;

public record Price(decimal Value, Currency Currency);