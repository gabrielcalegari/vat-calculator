using Riok.Mapperly.Abstractions;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;
using VatCalculator.Domain.Models;

namespace VatCalculator.Application.Mapping;

[Mapper]
public partial class CalculateVatResultMapper
{
    public partial CalculateVatResult ToApplication(TaxCalculationResult taxCalculationResult);
}
