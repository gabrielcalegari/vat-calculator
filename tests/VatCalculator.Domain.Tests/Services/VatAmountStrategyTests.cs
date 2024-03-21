using FluentAssertions;
using VatCalculator.Domain.Models;
using VatCalculator.Domain.Services;

namespace VatCalculator.Domain.Tests.Services;

public class VatAmountStrategyTests
{
    [Theory]
    [InlineData(1, 11, 10)]
    [InlineData(100, 1100, 1000)]
    public void Calculate_WithTaxRateAndValidAmount_GrossAndNetAmountsCalculated(decimal amount, decimal expectedGrossAmount, decimal expectedNetAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);
        var netAmountStrategy = new VatAmountStrategy();

        // Act
        var result = netAmountStrategy.Calculate(taxRate, amount);

        // Assert
        result.VatRate.Should().Be(taxRate.VatRate);
        result.VatAmount.Should().Be(amount);
        result.GrossAmount.Should().BeApproximately(expectedGrossAmount, precision: 0.001M);
        result.NetAmount.Should().BeApproximately(expectedNetAmount, precision: 0.001M);
    }
}
