namespace BookStore.Api.Common;

public record ErrorsResult(IEnumerable<Error> Errors, string StatusCode)
{
    public IResult ToTypedResult() =>
        StatusCode switch
        {
            "400" => TypedResults.BadRequest(this),
            "401" => TypedResults.Unauthorized(),
            "403" => TypedResults.Forbid(),
            "404" => TypedResults.NotFound(this),
            "409" => TypedResults.Conflict(this),
            _ => TypedResults.BadRequest(this)
        };
}

public record Error(string ErrorMessage);