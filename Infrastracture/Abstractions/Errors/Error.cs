namespace Infrastructure.Abstractions.Errors;
public record Error(string Code, string Name)
{
    public readonly static Error None = new(string.Empty, string.Empty);
    public readonly static Error NullValue = new("Error.NullValue", "Null value was provided");
}