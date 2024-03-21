using ErrorOr;
using FluentValidation;
using VatCalculator.Application.Extensions;
using VatCalculator.Application.Mapping;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;
using VatCalculator.Domain.Models;
using VatCalculator.Domain.Services;

namespace VatCalculator.Application.UseCases.CalculateVat;

public class CalculateVatUseCase : ICalculateVatUseCase
{
    private readonly IValidator<CalculateVatCommand> validator;

    public CalculateVatUseCase(IValidator<CalculateVatCommand> validator)
    {
        this.validator = validator;
    }

    public ErrorOr<CalculateVatResult> Execute(CalculateVatCommand command)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToErrorOr<CalculateVatResult>();
        }

        (var taxCalculationStrategy, var amount) = GetTaxCalculationStrategy(command);

        var taxCalculator = new TaxCalculator(taxCalculationStrategy);
        var taxRate = new TaxRate(command.VatRate);

        var taxCalculatorResult = taxCalculator.Calculate(taxRate, amount);

        var mapper = new CalculateVatResultMapper();
        return mapper.ToApplication(taxCalculatorResult);
    }

    private static (ITaxCalculationStrategy strategy, decimal amount) GetTaxCalculationStrategy(CalculateVatCommand command)
    {
        if (command.GrossAmount.HasValue)
        {
            return (new GrossAmountStrategy(), command.GrossAmount.Value);
        }

        if (command.NetAmount.HasValue)
        {
            return (new NetAmountStrategy(), command.NetAmount.Value);
        }

        if (command.VatAmount.HasValue)
        {
            return (new VatAmountStrategy(), command.VatAmount.Value);
        }

        throw new InvalidOperationException("A strategy could not be found.");
    }
}
