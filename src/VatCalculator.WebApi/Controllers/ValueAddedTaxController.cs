using Microsoft.AspNetCore.Mvc;
using VatCalculator.Application.UseCases.CalculateVat;
using VatCalculator.Application.UseCases.CalculateVat.Dtos;
using VatCalculator.WebApi.Extensions;

namespace VatCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValueAddedTaxController : ControllerBase
    {
        private readonly ICalculateVatUseCase calculateVatUseCase;

        public ValueAddedTaxController(ICalculateVatUseCase calculateVatUseCase)
        {
            this.calculateVatUseCase = calculateVatUseCase;
        }

        [HttpPost]
        public IActionResult Calculate(CalculateVatCommand calculateVatCommand)
        {
            var result = calculateVatUseCase.Execute(calculateVatCommand);

            return result.Match(
                calculateVatResult => Ok(calculateVatResult),
                error => this.ErrorsToProblem(error));
        }
    }
}
