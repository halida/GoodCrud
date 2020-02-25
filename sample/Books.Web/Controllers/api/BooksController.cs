using Books.Application;
using Books.Domain;
using GoodCrud.Web;
using Microsoft.AspNetCore.Mvc;

namespace GoodCrud.Web.Books
{
    [Area("api")]
    [Route("api/[controller]")]
    public class BooksController : Api.Controllers.CrudController<BookWebService, Book, IBooksUnitOfWork, BookDto, BookCreateUpdateDto, BookCreateUpdateDto, BookFilterDto>
    {
        public BooksController(BookWebService service) : base(service) { }
    }
}