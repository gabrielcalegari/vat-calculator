using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using VatCalculator.Application.UseCases.CalculateVat;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;

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

        var useCase = new CalculateVatUseCase(validator);

        // Act
        var result = useCase.Execute(command);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Be(expectedError);
    }

    [Fact]
    public void Execute_WithUndefinedStrategy_InvalidOperationException()
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

        var useCase = new CalculateVatUseCase(validator);

        // Act
        var act = () => useCase.Execute(command);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("A strategy could not be found.");
    }

    [Fact]
    public void Execute_WithGrossAmount_Success()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            GrossAmount = 1000,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = Substitute.For<IValidator<CalculateVatCommand>>();
        validator
            .Validate(command)
            .Returns(new ValidationResult());

        var useCase = new CalculateVatUseCase(validator);

        // Act
        var result = useCase.Execute(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.GrossAmount.Should().Be(1000M);
        result.Value.NetAmount.Should().BeApproximately(909.09M, 0.001M);
        result.Value.VatAmount.Should().BeApproximately(90.91M, 0.001M);
        result.Value.VatRate.Should().Be(Domain.Models.AustrianVatRates.TenPercent);
    }

    [Fact]
    public void Execute_WithNetAmount_Success()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            NetAmount = 1000,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = Substitute.For<IValidator<CalculateVatCommand>>();
        validator
            .Validate(command)
            .Returns(new ValidationResult());

        var useCase = new CalculateVatUseCase(validator);

        // Act
        var result = useCase.Execute(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.NetAmount.Should().Be(1000M);
        result.Value.GrossAmount.Should().Be(1100M);
        result.Value.VatAmount.Should().Be(100M);
        result.Value.VatRate.Should().Be(Domain.Models.AustrianVatRates.TenPercent);
    }

    [Fact]
    public void Execute_WithVatAmount_Success()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            VatAmount = 100,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = Substitute.For<IValidator<CalculateVatCommand>>();
        validator
            .Validate(command)
            .Returns(new ValidationResult());

        var useCase = new CalculateVatUseCase(validator);

        // Act
        var result = useCase.Execute(command);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.VatAmount.Should().Be(100M);
        result.Value.GrossAmount.Should().Be(1100M);
        result.Value.NetAmount.Should().Be(1000M);
        result.Value.VatRate.Should().Be(Domain.Models.AustrianVatRates.TenPercent);
    }
}
