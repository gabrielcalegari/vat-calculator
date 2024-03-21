using FluentAssertions;
using VatCalculator.Domain.Models;
using VatCalculator.Domain.Services;

namespace VatCalculator.Domain.Tests.Services;

public class GrossAmountStrategyTests
{
    [Theory]
    [InlineData(1, 0.91, 0.09)]
    [InlineData(100, 90.91, 9.09)]
    public void Calculate_WithTaxRateAndValidAmount_NetAndVatAmountsCalculated(decimal amount, decimal expectedNetAmount, decimal expectedVatAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);
        var grossAmountStrategy = new GrossAmountStrategy();

        // Act
        var result = grossAmountStrategy.Calculate(taxRate, amount);

        // Assert
        result.VatRate.Should().Be(taxRate.VatRate);
        result.GrossAmount.Should().Be(amount);
        result.NetAmount.Should().BeApproximately(expectedNetAmount, precision: 0.001M);
        result.VatAmount.Should().BeApproximately(expectedVatAmount, precision: 0.001M);
    }
}
