
using Books.Application;
using GoodCrud.Client;

namespace Books.Client
{
    public class BooksClient : CrudClient<BookDto, BookCreateUpdateDto, BookCreateUpdateDto, BookFilterDto>
    {
        public BooksClient(string website) : base(website, "api", "Books")
        {
        }
    }
}