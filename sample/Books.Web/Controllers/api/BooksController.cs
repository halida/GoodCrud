using Books.Application;
using Books.Data.Contract;
using Books.Domain;
using GoodCrud.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GoodCrud.Web.Books.Api
{
    [Area("api")]
    [Route("api/[controller]")]
    public class BooksController : CrudController<BookWebService, Book, IBooksUnitOfWork, BookDto, BookCreateUpdateDto, BookCreateUpdateDto, BookFilterDto>
    {
        public BooksController(BookWebService service) : base(service) { }
    }
}