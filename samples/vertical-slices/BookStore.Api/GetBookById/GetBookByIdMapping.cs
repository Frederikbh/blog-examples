using AutoMapper;

using BookStore.Api.Data;

namespace BookStore.Api.GetBookById;

public class GetBookByIdMapping : Profile
{
    public GetBookByIdMapping()
    {
        CreateMap<Book, GetBookByIdResponse>();
    }
}
