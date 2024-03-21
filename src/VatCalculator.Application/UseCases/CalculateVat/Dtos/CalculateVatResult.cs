using VatCalculator.Domain.Models;

namespace VatCalculator.Application.UseCases.CalculateVat.Dtos;

public record CalculateVatResult
{
    public decimal GrossAmount { get; set; }

    public decimal NetAmount { get; set; }

    public decimal VatAmount { get; set; }

    public AustrianVatRates VatRate { get; init; }
}
