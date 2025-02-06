using BookStore.Api.Common;

using MediatR;

namespace BookStore.Api.GetBookById;

public class GetBookByIdEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app) =>
        app.MapGet("/books/{id}", GetBookById)
            .Produces<GetBookByIdResponse>()
            .Produces<ErrorsResult>(404)
            .WithOpenApi();

    private static async Task<IResult> GetBookById(HttpContext context, string id, IMediator mediator)
    {
        var request = new GetBookByIdRequest(id);

        var response = await mediator.Send(request);
        return response.Match<IResult>(
            TypedResults.Ok,
            error => error.ToTypedResult()
        );
    }
}