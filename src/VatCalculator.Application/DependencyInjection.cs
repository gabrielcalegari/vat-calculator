using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VatCalculator.Application.UseCases.CalculateVat;

namespace VatCalculator.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services = services ?? throw new ArgumentNullException(nameof(services));

        services.AddScoped<ICalculateVatUseCase, CalculateVatUseCase>();
        services.AddScoped<ITaxCalculationStrategyFactory, TaxCalculationStrategyFactory>();

        services.AddValidatorsFromAssemblyContaining<IApplication>();

        return services;
    }
}
