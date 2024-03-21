using VatCalculator.Domain.Models;

namespace VatCalculator.Domain.Services;

public sealed class GrossAmountStrategy : ITaxCalculationStrategy
{
    public TaxCalculationResult Calculate(TaxRate taxRate, decimal amount)
    {
        var result = new TaxCalculationResult { VatRate = taxRate.VatRate, GrossAmount = amount };

        result.NetAmount = taxRate.CalculateNetAmount(amount);
        result.VatAmount = result.GrossAmount - result.NetAmount;

        return result;
    }
}
