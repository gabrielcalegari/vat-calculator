using FluentAssertions;
using NSubstitute;
using VatCalculator.Domain.Models;
using VatCalculator.Domain.Services;

namespace VatCalculator.Domain.Tests.Services;

public class TaxCalculatorTests
{
    [Fact]
    public void TaxCalculator_WithTaxCalculationStrategyNull_ArgumentNullException()
    {
        // Act
        var act = () => new TaxCalculator(null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Calculate_WithTaxRateNull_ArgumentNullException()
    {
        // Arrange
        var strategy = Substitute.For<ITaxCalculationStrategy>();
        var taxCalculator = new TaxCalculator(strategy);

        // Act
        var act = () => taxCalculator.Calculate(null, 10M);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Calculate_WithInvalidAmount_ArgumentOutOfRangeException(decimal amount)
    {
        // Arrange
        var strategy = Substitute.For<ITaxCalculationStrategy>();
        var taxCalculator = new TaxCalculator(strategy);
        var taxRate = new TaxRate(AustrianVatRates.TwentyPercent);

        // Act
        var act = () => taxCalculator.Calculate(taxRate, amount);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}