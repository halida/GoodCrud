using AutoMapper;
using Books.Domain;

namespace Books.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookCreateUpdateDto, Book>();
            CreateMap<Book, BookCreateUpdateDto>();
        }
    }

}