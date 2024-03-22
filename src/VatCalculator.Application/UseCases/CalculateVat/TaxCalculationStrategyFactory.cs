using VatCalculator.Application.UseCases.CalculateVat.Dtos;
using VatCalculator.Domain.Services;

namespace VatCalculator.Application.UseCases.CalculateVat;

public class TaxCalculationStrategyFactory : ITaxCalculationStrategyFactory
{
    public (ITaxCalculationStrategy strategy, decimal amount) CreateStrategyAndAmount(CalculateVatCommand command)
    {
        if (command.GrossAmount.HasValue)
        {
            return (new GrossAmountStrategy(), command.GrossAmount.Value);
        }

        if (command.NetAmount.HasValue)
        {
            return (new NetAmountStrategy(), command.NetAmount.Value);
        }

        if (command.VatAmount.HasValue)
        {
            return (new VatAmountStrategy(), command.VatAmount.Value);
        }

        throw new InvalidOperationException("A strategy could not be found.");
    }
}
