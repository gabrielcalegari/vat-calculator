using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace VatCalculator.WebApi.Extensions
{
    public static class ErrorOrExtensions
    {
        public static IActionResult ErrorsToProblem(this ControllerBase controller, List<Error> errors)
        {
            var firstError = errors[0];

            var statusCode = firstError.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            if (errors.Count > 1)
            {
                var details = errors.Select(e => e.Description).ToArray();
                return controller.Problem(statusCode: statusCode, title: "Multiple errors ocurred.", detail: string.Join("; ", details));
            }

            return controller.Problem(statusCode: statusCode, title: firstError.Description);
        }
    }
}
