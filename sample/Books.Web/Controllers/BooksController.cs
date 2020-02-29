using Books.Application;
using Books.Data.Contract;
using Books.Domain;
using GoodCrud.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GoodCrud.Web.Books
{
    [Route("[controller]")]
    public class BooksController : CrudController<BookService, Book, BookDto, BookCreateUpdateDto, BookCreateUpdateDto, BookFilterDto>
    {
        public BooksController(BookService service) : base(service) { }
    }
}