using AutoMapper;

using BookStore.Api.Common;
using BookStore.Api.Data;

using MediatR;

using OneOf;

namespace BookStore.Api.GetBookById;
public record GetBookByIdRequest(string Id) : IRequest<OneOf<GetBookByIdResponse, ErrorsResult>>;

public record GetBookByIdResponse(string Id, string BookName, string Author, DateTime PublishedAt);

public class GetBookByIdRequestHandler : IRequestHandler<GetBookByIdRequest, OneOf<GetBookByIdResponse, ErrorsResult>>
{
    private readonly IMapper _mapper;
    private readonly BookStoreContext _context;

    public GetBookByIdRequestHandler(IMapper mapper, BookStoreContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<OneOf<GetBookByIdResponse, ErrorsResult>> Handle(GetBookByIdRequest request, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .FindAsync([request.Id], cancellationToken);

        if (book is null)
        {
            return new ErrorsResult([new Error("Book not found")], "404");
        }

        return _mapper.Map<GetBookByIdResponse>(book);
    }
}
