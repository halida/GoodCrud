using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.Data.Contract;
using Books.Domain;
using FluentValidation;
using GoodCrud.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Books.Application
{
    public class AuthorService : EntityService<Author, AuthorDto, AuthorCreateUpdateDto, AuthorCreateUpdateDto, AuthorFilterDto>
    {
        public AuthorService(IBooksUnitOfWork uow, IMapper mapper, IValidator<Author> validator) : base(uow, mapper, validator)
        {
            PageSize = 10;
        }

        public override async Task<IQueryable<Author>> ListFilterAsync(AuthorFilterDto filter)
        {
            var query = Repo.Queryable().Include(x => x.Books).AsQueryable();
            if (!String.IsNullOrEmpty(filter.Name)) { query = query.Where(e => e.Name!.Contains(filter.Name)); }
            query = query.OrderByDescending(e => e.Id);
            return await Task.FromResult<IQueryable<Author>>(query);
        }

        public async Task<List<Book>> RelatedBooks(string kindFrom, string kindTo)
        {
            var results = new List<Book>();

            // demo only, not effecient
            var query = this.Uow.GetRepo<Book>().Queryable()
                .Where(e => e.Title!.Contains(kindFrom));
            foreach (var book in await query.ToListAsync())
            {
                var author = await Uow.GetReferenceAsync(book, e => e.Author);
                var authorBooksQuery = Uow.GetCollectionQuery(author, e => e.Books)
                    .Where(e => e.Title!.Contains(kindTo));
                foreach (var authorBook in await authorBooksQuery.ToListAsync())
                {
                    results.Add(authorBook);
                }
            }
            return results;
        }
    }
}