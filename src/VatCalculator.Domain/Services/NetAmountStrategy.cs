using VatCalculator.Domain.Models;

namespace VatCalculator.Domain.Services;

public sealed class NetAmountStrategy : ITaxCalculationStrategy
{
    public TaxCalculationResult Calculate(TaxRate taxRate, decimal amount)
    {
        var result = new TaxCalculationResult { VatRate = taxRate.VatRate, NetAmount = amount };

        result.VatAmount = taxRate.CalculateVatAmount(amount);
        result.GrossAmount = taxRate.CalculateGrossAmount(amount);;

        return result;
    }
}
