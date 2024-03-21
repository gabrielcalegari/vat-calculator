using FluentAssertions;
using VatCalculator.Domain.Models;

namespace VatCalculator.Domain.Tests.Models;

public class TaxRateTests
{
    [Fact]
    public void TaxRate_WithInvalidAustrianVatRates_ArgumentOutOfRangeException()
    {
        // Arrange
        AustrianVatRates austrianVatRate = 0;

        // Act
        var act = () => new TaxRate(austrianVatRate);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void TaxRate_WithValidAustrianVatRates_Success()
    {
        // Arrange
        AustrianVatRates austrianVatRate = AustrianVatRates.TwentyPercent;

        // Act
        var taxRate = new TaxRate(austrianVatRate);

        // Assert
        taxRate.Should().NotBeNull();
        taxRate.VatRate.Should().Be(austrianVatRate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateVatAmount_WithInvalidAmount_ArgumentOutOfRangeException(decimal netAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);

        // Act
        Action act = () => taxRate.CalculateVatAmount(netAmount);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1, 0.1)]
    [InlineData(100, 10)]
    public void CalculateVatAmount_WithValidAmount_Success(decimal netAmount, decimal expectedVatAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);

        // Act
        var vatAmount = taxRate.CalculateVatAmount(netAmount);

        // Assert
        vatAmount.Should().Be(expectedVatAmount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateGrossAmount_WithInvalidAmount_ArgumentOutOfRangeException(decimal grossAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);

        // Act
        Action act = () => taxRate.CalculateGrossAmount(grossAmount);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1, 1.1)]
    [InlineData(100, 110)]
    public void CalculateGrossAmount_WithValidAmount_Success(decimal netAmount, decimal expectedGrossAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);

        // Act
        var grossAmount = taxRate.CalculateGrossAmount(netAmount);

        // Assert
        grossAmount.Should().Be(expectedGrossAmount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateNetAmount_WithInvalidAmount_ArgumentOutOfRangeException(decimal grossAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);

        // Act
        Action act = () => taxRate.CalculateNetAmount(grossAmount);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1, 0.91)]
    [InlineData(100, 90.91)]
    public void CalculateNetAmount_WithValidAmount_Success(decimal grossAmount, decimal expectedNetAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);

        // Act
        var netAmount = taxRate.CalculateNetAmount(grossAmount);

        // Assert
        netAmount.Should().BeApproximately(expectedNetAmount, precision: 0.001M);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateNetAmountFromVatAmount_WithInvalidAmount_ArgumentOutOfRangeException(decimal vatAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);

        // Act
        Action act = () => taxRate.CalculateNetAmountFromVatAmount(vatAmount);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(100, 1000)]
    public void CalculateNetAmountFromVatAmount_WithValidAmount_Success(decimal grossAmount, decimal expectedNetAmount)
    {
        // Arrange
        var taxRate = new TaxRate(AustrianVatRates.TenPercent);

        // Act
        var netAmount = taxRate.CalculateNetAmountFromVatAmount(grossAmount);

        // Assert
        netAmount.Should().BeApproximately(expectedNetAmount, precision: 0.001M);
    }
}
