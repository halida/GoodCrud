using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.Domain;
using GoodCrud.Application.WebServices;

namespace Books.Application
{
    public class BookWebService : CrudWebService<Book, IBooksUnitOfWork, BookDto, BookCreateUpdateDto, BookCreateUpdateDto, BookFilterDto>
    {
        public BookWebService(IBooksUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
            PageSize = 10;
        }

        public override async Task<IQueryable<Book>> ListFilterAsync(BookFilterDto filter)
        {
            var query = Repo.Queryable();
            if (!String.IsNullOrEmpty(filter.Title)) { query = query.Where(e => e.Title.Contains(filter.Title)); }
            if (!String.IsNullOrEmpty(filter.Description)) { query = query.Where(e => e.Description.Contains(filter.Description)); }
            query = query.OrderByDescending(e => e.Id);
            return await Task.FromResult<IQueryable<Book>>(query);
        }

    }
}