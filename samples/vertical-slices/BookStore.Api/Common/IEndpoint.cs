namespace BookStore.Api.Common;

public interface IEndpoint
{
    RouteHandlerBuilder Map(IEndpointRouteBuilder app);
}

