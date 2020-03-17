using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Domain;
using FluentValidation;
using GoodCrud.Data.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Books.Application.Services.Domain
{
    public static class BookService
    {
        public static async Task<List<Book>> RelatedBooks(IBaseUnitOfWork uow, string kindFrom, string kindTo)
        {
            var results = new List<Book>();

            // demo only, not effecient
            var query = uow.GetRepo<Book>().Queryable()
                .Where(e => e.Title!.Contains(kindFrom));
            foreach (var book in await query.ToListAsync())
            {
                var author = await uow.GetReferenceAsync(book, e => e.Author);
                if (author == null) { continue; }

                var authorBooksQuery = uow.GetCollectionQuery(author, e => e.Books)
                    .Where(e => e.Title!.Contains(kindTo));
                foreach (var authorBook in await authorBooksQuery.ToListAsync())
                {
                    results.Add(authorBook);
                }
            }
            return results;
        }

        public static async Task<List<Book>?> RelatedBooksAsync(this Book book, IBaseUnitOfWork uow)
        {
            var author = await uow.GetReferenceAsync(book, e => e.Author);
            if (author == null) { return null; }

            var books = await uow.GetReferenceAsync(author, e => e.Books);
            return books;
        }
    }
}