using FluentAssertions;
using VatCalculator.Application.UseCases.CalculateVat;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;

namespace VatCalculator.Application.Tests.UseCases.CalculateVat;

public class CalculateVatCommandValidatorTests
{
    [Fact]
    public void CalculateVatCommandValidator_WhenMoreThanOneAmountHasValue_ValidationError()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            GrossAmount = 10,
            NetAmount = 10,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = new CalculateVatCommandValidator();

        // Act
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors[0].ErrorMessage.Should().Be("Exactly one amount must be provided.");
    }

    [Fact]
    public void CalculateVatCommandValidator_WithUndefinedVatRate_ValidationError()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            GrossAmount = 10,
            VatRate = (Domain.Models.AustrianVatRates)11
        };

        var validator = new CalculateVatCommandValidator();

        // Act
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors[0].ErrorMessage.Should().Be("VAT Rate must be a valid Austrian VAT Rate.");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void CalculateVatCommandValidator_WithVatAmountInvalid_ValidationError(decimal amount)
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            VatAmount = amount,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = new CalculateVatCommandValidator();

        // Act
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors[0].ErrorMessage.Should().Be("'Vat Amount' must be greater than '0'.");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void CalculateVatCommandValidator_WithNetAmountInvalid_ValidationError(decimal amount)
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            NetAmount = amount,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = new CalculateVatCommandValidator();

        // Act
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors[0].ErrorMessage.Should().Be("'Net Amount' must be greater than '0'.");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void CalculateVatCommandValidator_WithGrossAmountInvalid_ValidationError(decimal amount)
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            GrossAmount = amount,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = new CalculateVatCommandValidator();

        // Act
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors[0].ErrorMessage.Should().Be("'Gross Amount' must be greater than '0'.");
    }

    [Fact]
    public void CalculateVatCommandValidator_WithGrossAmountValid_Success()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            GrossAmount = 100,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = new CalculateVatCommandValidator();

        // Act
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CalculateVatCommandValidator_WithNetAmountValid_Success()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            NetAmount = 100,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = new CalculateVatCommandValidator();

        // Act
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CalculateVatCommandValidator_WithVatAmountValid_Success()
    {
        // Arrange
        var command = new CalculateVatCommand
        {
            VatAmount = 100,
            VatRate = Domain.Models.AustrianVatRates.TenPercent
        };

        var validator = new CalculateVatCommandValidator();

        // Act
        var validationResult = validator.Validate(command);

        // Assert
        validationResult.IsValid.Should().BeTrue();
    }
}
