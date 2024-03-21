using ErrorOr;
using FluentValidation.Results;

namespace VatCalculator.Application.Extensions;

public static class FluentValidationExtensions
{
    public static ErrorOr<TResult> ToErrorOr<TResult>(this List<ValidationFailure> failures)
    {
        var errors = failures.ConvertAll(error =>
            Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

        return errors;
    }
}
