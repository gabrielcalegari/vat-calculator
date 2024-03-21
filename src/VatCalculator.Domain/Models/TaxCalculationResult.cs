namespace VatCalculator.Domain.Models;

public record TaxCalculationResult
{
    public decimal GrossAmount { get; set; }

    public decimal NetAmount { get; set; }

    public decimal VatAmount { get; set; }

    public AustrianVatRates VatRate { get; init; }
}
