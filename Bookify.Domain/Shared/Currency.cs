namespace Bookify.Domain.Shared;

public record Currency
{
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");
    public static readonly Currency Cad = new("CAD");
    internal static readonly Currency None = new(string.Empty);

    public static readonly IReadOnlyCollection<Currency> All = [Usd, Cad, Eur];

    public static Currency FromCode(string code) =>
        All.FirstOrDefault(c => c.Code == code)
        ?? throw new ArgumentException("Invalid currency code.");

    public string Code { get; init; }

    private Currency(string code) => Code = code;
}
