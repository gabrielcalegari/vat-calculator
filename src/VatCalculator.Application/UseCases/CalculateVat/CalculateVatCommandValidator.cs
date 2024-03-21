using FluentValidation;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;

namespace VatCalculator.Application.UseCases.CalculateVat;

public class CalculateVatCommandValidator : AbstractValidator<CalculateVatCommand>
{
    public CalculateVatCommandValidator()
    {
        RuleFor(c => c)
            .Must(c => new[] { c.GrossAmount, c.NetAmount, c.VatAmount }
                    .Count(amount => amount.HasValue) == 1)
            .WithMessage("Exactly one amount must be provided.");

        RuleFor(c => c.VatRate)
            .IsInEnum()
            .WithMessage("VAT Rate must be a valid Austrian VAT Rate.");

        RuleFor(c => c.VatAmount)
            .GreaterThan(0)
            .When(c => c.VatAmount.HasValue);

        RuleFor(c => c.NetAmount)
            .GreaterThan(0)
            .When(c => c.NetAmount.HasValue);

        RuleFor(c => c.GrossAmount)
            .GreaterThan(0)
            .When(c => c.GrossAmount.HasValue);
    }
}
