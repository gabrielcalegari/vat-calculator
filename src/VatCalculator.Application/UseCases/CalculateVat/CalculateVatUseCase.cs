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
    private readonly ITaxCalculationStrategyFactory taxCalculationStrategyFactory;

    public CalculateVatUseCase(IValidator<CalculateVatCommand> validator, ITaxCalculationStrategyFactory taxCalculationStrategyFactory)
    {
        this.validator = validator;
        this.taxCalculationStrategyFactory = taxCalculationStrategyFactory;
    }

    public ErrorOr<CalculateVatResult> Execute(CalculateVatCommand command)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors.ToErrorOr<CalculateVatResult>();
        }

        (var taxCalculationStrategy, var amount) = taxCalculationStrategyFactory.CreateStrategyAndAmount(command);

        var taxCalculator = new TaxCalculator(taxCalculationStrategy);
        var taxRate = new TaxRate(command.VatRate);

        var taxCalculatorResult = taxCalculator.Calculate(taxRate, amount);

        var mapper = new CalculateVatResultMapper();
        return mapper.ToApplication(taxCalculatorResult);
    }
}
