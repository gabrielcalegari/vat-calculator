using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using VatCalculator.Application.UseCases.CalculateVat;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;
using VatCalculator.Domain.Models;
using VatCalculator.Domain.Services;

namespace VatCalculator.Application.Tests.UseCases.CalculateVat;

public class CalculateVatUseCaseTests
{
    [Fact]
    public void Execute_WithInvalidCommand_Error()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            GrossAmount = 10,
            NetAmount = 10,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var expectedError = "Anything happened.";

        var validator = Substitute.For<IValidator<CalculateVatCommand>>();
        validator
            .Validate(command)
            .Returns(new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure() { ErrorMessage = expectedError }
                }
            });

        var strategyFactory = Substitute.For<ITaxCalculationStrategyFactory>();

        var useCase = new CalculateVatUseCase(validator, strategyFactory);

        // Act
        var result = useCase.Execute(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be(expectedError);
    }

    [Fact]
    public void Execute_WhenStrategyCanNotBeDefined_ExceptionThrown()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = Substitute.For<IValidator<CalculateVatCommand>>();

        validator
            .Validate(command)
            .Returns(new ValidationResult());

        var strategyFactory = Substitute
            .For<ITaxCalculationStrategyFactory>();

        strategyFactory
            .CreateStrategyAndAmount(command)
            .Throws<InvalidOperationException>();


        var useCase = new CalculateVatUseCase(validator, strategyFactory);

        // Act
        var act = () => useCase.Execute(command);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Execute_WithValidCommandAndStrategy_Success()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            GrossAmount = 1100,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        const decimal expectedNetAmount = 1000;
        const decimal expectedVatAmount = 100;

        var validator = Substitute.For<IValidator<CalculateVatCommand>>();

        validator
            .Validate(command)
            .Returns(new ValidationResult());

        var strategy = Substitute.For<ITaxCalculationStrategy>();

        strategy.Calculate(Arg.Is<TaxRate>(r => r.VatRate == command.VatRate), command.GrossAmount.Value)
            .Returns(new TaxCalculationResult
            {
                GrossAmount = command.GrossAmount.Value,
                VatRate = command.VatRate,
                NetAmount = 1000,
                VatAmount = 100
            });

        var strategyFactory = Substitute.For<ITaxCalculationStrategyFactory>();

        strategyFactory
            .CreateStrategyAndAmount(command)
            .Returns((strategy, command.GrossAmount.Value));

        var useCase = new CalculateVatUseCase(validator, strategyFactory);

        // Act
        var result = useCase.Execute(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.GrossAmount.Should().Be(command.GrossAmount);
        result.Value.NetAmount.Should().Be(expectedNetAmount);
        result.Value.VatAmount.Should().Be(expectedVatAmount);
        result.Value.VatRate.Should().Be(command.VatRate);
    }
}
