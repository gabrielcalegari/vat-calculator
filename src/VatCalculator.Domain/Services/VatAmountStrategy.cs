using VatCalculator.Domain.Models;

namespace VatCalculator.Domain.Services;

public sealed class VatAmountStrategy : ITaxCalculationStrategy
{
    public TaxCalculationResult Calculate(TaxRate taxRate, decimal amount)
    {
        var result = new TaxCalculationResult { VatRate = taxRate.VatRate, VatAmount = amount };

        result.NetAmount = taxRate.CalculateNetAmountFromVatAmount(amount);
        result.GrossAmount = taxRate.CalculateGrossAmount(result.NetAmount);

        return result;
    }
}