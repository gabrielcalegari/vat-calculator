using VatCalculator.Application.UseCases.CalculateVat.Dtos;
using VatCalculator.Domain.Services;

namespace VatCalculator.Application.UseCases.CalculateVat;

public interface ITaxCalculationStrategyFactory
{
    (ITaxCalculationStrategy strategy, decimal amount) CreateStrategyAndAmount(CalculateVatCommand command);
}
