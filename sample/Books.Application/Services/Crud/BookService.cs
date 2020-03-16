using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.Application.Services.Domain;
using Books.Data.Contract;
using Books.Domain;
using FluentValidation;
using GoodCrud.Application.Services;

namespace Books.Application.Services.Crud
{
    public class BookService : CrudService<Book, BookDto, BookCreateUpdateDto, BookCreateUpdateDto, BookFilterDto>
    {
        public BookService(IBooksUnitOfWork uow, IMapper mapper, IValidator<Book> validator) : base(uow, mapper, validator)
        {
            PageSize = 10;
        }

        public override async Task<IQueryable<Book>> ListFilterAsync(BookFilterDto filter)
        {
            var query = Repo.Queryable();
            if (!String.IsNullOrEmpty(filter.Title)) { query = query.Where(e => e.Title!.Contains(filter.Title)); }
            if (!String.IsNullOrEmpty(filter.Description)) { query = query.Where(e => e.Description!.Contains(filter.Description)); }
            query = query.OrderByDescending(e => e.Id);
            return await Task.FromResult<IQueryable<Book>>(query);
        }

        public async Task<IEnumerable<BookDto>?> RelatedBooksAsync(int id)
        {
            var entity = await Repo.FindAsync(id);
            if (entity == null) { return null; }
            var list = await entity.RelatedBooksAsync(this.Uow);

            return list.Select(e => EntityDto(e));
        }

    }
}