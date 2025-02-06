using FluentValidation;
using MediatR;
using OneOf;
using System.Linq.Expressions;

using FluentValidation.Results;

namespace BookStore.Api.Common.Behaviors;

public sealed class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IOneOf
{
    private static bool s_implicitConversionChecked;
    private static Func<ErrorsResult, TResponse>? s_implicitConversionFunc;

    private readonly IEnumerable<IValidator> _validators;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (s_implicitConversionFunc is null && !s_implicitConversionChecked)
        {
            var responseType = typeof(TResponse);

            if (responseType.IsGenericType &&
                responseType.GenericTypeArguments.Any(t => t == typeof(ErrorsResult)))
            {
                var implicitConversionMethod = responseType.GetMethod("op_Implicit", [typeof(ErrorsResult)]);

                if (implicitConversionMethod is not null)
                {
                    var errorsParam = Expression.Parameter(typeof(ErrorsResult), "e");
                    s_implicitConversionFunc =
                        Expression.Lambda<Func<ErrorsResult, TResponse>>(
                                Expression.Call(implicitConversionMethod, errorsParam),
                                errorsParam)
                            .Compile();
                }
            }

            s_implicitConversionChecked = true;
        }

        if (s_implicitConversionFunc is not null)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults =
                await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var validationResult = new ValidationResult(validationResults);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => new Error(e.ErrorMessage));

                return s_implicitConversionFunc(new ErrorsResult(errors, "400"));
            }
        }

        return await next()
            .ConfigureAwait(false);
    }
}
