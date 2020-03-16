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

namespace Books.Application.Services.Crud
{
    public class AuthorService : CrudService<Author, AuthorDto, AuthorCreateUpdateDto, AuthorCreateUpdateDto, AuthorFilterDto>
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

        public async Task<List<BookDto>> RelatedBooks(string kindFrom, string kindTo)
        {
            var list = await Domain.BookService.RelatedBooks(this.Uow, kindFrom, kindTo);
            return list.Select(e => this.Mapper.Map<BookDto>(e)).ToList();
        }
    }
}