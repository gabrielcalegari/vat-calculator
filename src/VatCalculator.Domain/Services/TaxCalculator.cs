using VatCalculator.Domain.Models;

namespace VatCalculator.Domain.Services;

public class TaxCalculator
{
    private readonly ITaxCalculationStrategy strategy;

    public TaxCalculator(ITaxCalculationStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(strategy);

        this.strategy = strategy;
    }

    public TaxCalculationResult Calculate(TaxRate taxRate, decimal amount)
    {
        ArgumentNullException.ThrowIfNull(taxRate);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(amount, 0);

        return strategy.Calculate(taxRate, amount);
    }
}
