using FluentAssertions;
using VatCalculator.Domain.Models;
using VatCalculator.Domain.Services;

namespace VatCalculator.Domain.Tests.Services;

public class NetAmountStrategyTests
{
    [Theory]
    [InlineData(1, 1.1, 0.1)]
    [InlineData(100, 110, 10)]
    public void Calculate_WithTaxRateAndValidAmount_GrossAndVatAmountsCalculated(decimal amount, decimal expectedGrossAmount, decimal expectedVatAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);
        var netAmountStrategy = new NetAmountStrategy();

        // Act
        var result = netAmountStrategy.Calculate(taxRate, amount);

        // Assert
        result.VatRate.Should().Be(taxRate.VatRate);
        result.NetAmount.Should().Be(amount);
        result.GrossAmount.Should().BeApproximately(expectedGrossAmount, precision: 0.001M);
        result.VatAmount.Should().BeApproximately(expectedVatAmount, precision: 0.001M);
    }
}
