using VatCalculator.Domain.Models;

namespace VatCalculator.Domain.Services;

public interface ITaxCalculationStrategy
{
    TaxCalculationResult Calculate(TaxRate taxRate, decimal amount);
}


