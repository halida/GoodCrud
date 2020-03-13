using GoodCrud.Application.Contract.Dtos;

namespace Books.Application
{
    public class BookDto : EntityDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
    public class BookCreateUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
    public class BookFilterDto : FilterDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

    public class AuthorDto : EntityDto
    {
        public string Name { get; set; }
    }
    public class AuthorCreateUpdateDto
    {
        public string Name { get; set; }
    }
    public class AuthorFilterDto : FilterDto
    {
        public string Name { get; set; }
    }

}