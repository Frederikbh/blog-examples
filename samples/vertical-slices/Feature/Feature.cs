using AutoMapper;

using FluentValidation;

using MediatR;

using OneOf;

namespace $rootnamespace$;

public record $safeitemname$Request() : IRequest<OneOf<$safeitemname$Response, ErrorsResult>>;

public record $safeitemname$Response();

public class $safeitemname$RequestHandler : IRequestHandler<$safeitemname$Request, OneOf<$safeitemname$Response, ErrorsResult>>
{
    private readonly IMapper _mapper;

    public $safeitemname$RequestHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<OneOf<$safeitemname$Response, ErrorsResult>> Handle($safeitemname$Request request, CancellationToken cancellationToken)
    {
        return default;
    }
}

public class $safeitemname$Endpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app) =>
        app.MapGet("/$safeitemname$", $safeitemname$)
            .Produces<$safeitemname$Response>()
            .WithOpenApi();

    private static async Task<IResult> $safeitemname$(HttpContext context, IMediator mediator)
    {
        var request = new $safeitemname$Request();

        var response = await mediator.Send(request);
        return response.Match<IResult>(
            TypedResults.Ok,
            error => error.ToTypedResult()
        );
    }
}

public class $safeitemname$Mapping : Profile
{
    public $safeitemname$Mapping()
    {
    }
}

public class $safeitemname$Validator : AbstractValidator<$safeitemname$Request>
{
    public $safeitemname$Validator()
    {
    }
}
