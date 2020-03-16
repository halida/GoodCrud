using Books.Application;
using Books.Application.Services.Crud;
using Books.Domain;
using GoodCrud.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GoodCrud.Web.Books.Api
{
    [Area("api")]
    [Route("api/[controller]")]
    public class BooksController : CrudController<BookService, Book, BookDto, BookCreateUpdateDto, BookCreateUpdateDto, BookFilterDto>
    {
        public BooksController(BookService service) : base(service) { }
    }
}