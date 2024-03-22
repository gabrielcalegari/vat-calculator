using FluentAssertions;
using VatCalculator.Application.UseCases.CalculateVat;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;
using VatCalculator.Domain.Services;

namespace VatCalculator.Application.Tests.UseCases.CalculateVat;

public class TaxCalculationStrategyFactoryTests
{
    [Fact]
    public void CreateStrategyAndAmount_WhenStrategyCanNotBeDefined_InvalidOperationException()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var strategyFactory = new TaxCalculationStrategyFactory();

        // Act
        var act = () => strategyFactory.CreateStrategyAndAmount(command);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("A strategy could not be found.");
    }

    [Fact]
    public void CreateStrategyAndAmount_WhenCommandHasGrossAmount_GrossAmountStrategyReturned()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            GrossAmount = 1100,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var strategyFactory = new TaxCalculationStrategyFactory();

        // Act
        (ITaxCalculationStrategy strategy, decimal amount) = strategyFactory.CreateStrategyAndAmount(command);

        // Assert
        strategy.Should().BeAssignableTo<GrossAmountStrategy>();
        amount.Should().Be(command.GrossAmount);
    }

    [Fact]
    public void CreateStrategyAndAmount_WhenCommandHasNetAmount_NetAmountStrategyReturned()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            NetAmount = 1000,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var strategyFactory = new TaxCalculationStrategyFactory();

        // Act
        (ITaxCalculationStrategy strategy, decimal amount) = strategyFactory.CreateStrategyAndAmount(command);

        // Assert
        strategy.Should().BeAssignableTo<NetAmountStrategy>();
        amount.Should().Be(command.NetAmount);
    }

    [Fact]
    public void CreateStrategyAndAmount_WhenCommandHasVatAmount_VatAmountStrategyReturned()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            VatAmount = 1000,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var strategyFactory = new TaxCalculationStrategyFactory();

        // Act
        (ITaxCalculationStrategy strategy, decimal amount) = strategyFactory.CreateStrategyAndAmount(command);

        // Assert
        strategy.Should().BeAssignableTo<VatAmountStrategy>();
        amount.Should().Be(command.VatAmount);
    }
}
