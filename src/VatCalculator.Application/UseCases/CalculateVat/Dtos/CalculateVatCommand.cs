using VatCalculator.Domain.Models;

namespace VatCalculator.Application.UseCases.CalculateVat.Dtos;

public record CalculateVatCommand
{
    public decimal? GrossAmount { get; init; }

    public decimal? NetAmount { get; init; }

    public decimal? VatAmount { get; init; }

    public AustrianVatRates VatRate { get; init; }
}
