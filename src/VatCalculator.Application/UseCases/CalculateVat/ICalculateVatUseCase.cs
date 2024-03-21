using ErrorOr;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;

namespace VatCalculator.Application.UseCases.CalculateVat;

public interface ICalculateVatUseCase
{
    ErrorOr<CalculateVatResult> Execute(CalculateVatCommand command);
}
