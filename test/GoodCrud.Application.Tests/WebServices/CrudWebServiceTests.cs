using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using GoodCrud.Application.Contract.Dtos;
using AutoMapper;
using System.Threading.Tasks;
using Books.Application;
using Books.Domain;
using GoodCrud.Data.Tests.Helpers;
using FluentValidation;
using Books.Domain.Validations;
using Books.Data.Contract;
using Books.Application.Services.Crud;

namespace GoodCrud.Application.Tests.WebServices
{
    public class TestService : BookService
    {
        public TestService(IBooksUnitOfWork uow, IMapper mapper, IValidator<Book> validator) : base(uow, mapper, validator)
        {
            PageSize = 2;
        }
    }

    public class TestCallbackService : TestService
    {
        public TestCallbackService(IBooksUnitOfWork uow, IMapper mapper, IValidator<Book> validator) : base(uow, mapper, validator)
        {
        }

        public override async Task<ResultDto<BookDto>> CreateCallbackAsync(BookCreateUpdateDto dto, Book entity)
        {
            await Task.Delay(0);
            return ResultDto<BookDto>.Failed("create failed");
        }
        public override async Task<ResultDto<BookDto>> UpdateCallbackAsync(BookCreateUpdateDto dto, Book entity)
        {
            await Task.Delay(0);
            return ResultDto<BookDto>.Failed("update failed");
        }
        public override async Task<ResultDto<BookDto>> DeleteCallbackAsync(Book entity)
        {
            await Task.Delay(0);
            return ResultDto<BookDto>.Failed("delete failed");
        }
    }

    public class CrudWebServiceTests
    {
        protected static void WithService(Action<BookService> func, string type = null)
        {
            Utils.WithTestDatabase((uow) =>
            {
                var mapper = AutoMapperConfig.CreateMapper((cfg) => cfg.AddProfile(new MappingProfile()));
                var config = new ConfigurationBuilder().Build();
                var validator = new BookValidator();

                TestService service;
                service = type switch
                {
                    "TestCallbackService" => new TestCallbackService(uow, mapper, validator),
                    _ => new TestService(uow, mapper, validator),
                };
                service.Repo.DeleteAll();
                func(service);
            });
        }

        [Fact]
        public void Test_ListAsync()
        {
            WithService(async (service) =>
            {
                foreach (var i in Enumerable.Range(1, 9))
                {
                    var book = new Book() { Title = $"b{i}:seq{i % 2}", Description = $"{i % 2}" };
                    service.Repo.Insert(book);
                }
                await service.Uow.SaveChangesAsync();

                var filter = new BookFilterDto() { };
                // use sync to fix X.PagedList Task.Run issue
                var resultT = service.ListAsync(filter);
                resultT.Wait();
                var result = resultT.Result;

                // with pages
                Assert.Equal(9, result.MetaData.TotalItemCount);
                Assert.Equal(2, result.List.Count);
                Assert.Equal(5, result.MetaData.PageCount);
                Assert.Equal(1, result.MetaData.PageNumber);

                // select page
                var filter2 = new BookFilterDto() { Page = 3 };
                // use sync to fix X.PagedList Task.Run issue
                var result2T = service.ListAsync(filter2);
                result2T.Wait();
                var result2 = result2T.Result;

                Assert.Equal(9, result2.MetaData.TotalItemCount);
                Assert.Equal(2, result2.List.Count);
                Assert.Equal(5, result2.MetaData.PageCount);
                Assert.Equal(3, result2.MetaData.PageNumber);
                Assert.Equal("b5:seq1,b4:seq0", string.Join(",", result2.List.Select(e => e.Title)));

                // use filter
                var filter3 = new BookFilterDto() { Title = "seq1" };
                // use sync to fix X.PagedList Task.Run issue
                var result3T = service.ListAsync(filter3);
                result3T.Wait();
                var result3 = result3T.Result;

                Assert.Equal(5, result3.MetaData.TotalItemCount);
                Assert.Equal(2, result3.List.Count);
                Assert.Equal(3, result3.MetaData.PageCount);
                Assert.Equal("b9:seq1,b7:seq1", string.Join(",", result3.List.Select(e => e.Title)));

            });
        }

        [Fact]
        public void Test_ListFilterAsync()
        {
            WithService((service) =>
            {
                // already tested in Test_ListAsync
            });
        }

        [Fact]
        public void Test_GetAsync()
        {
            WithService(async (service) =>
            {
                var b1 = new Book() { Title = "b1" };
                service.Repo.Insert(b1);
                var b2 = new Book() { Title = "b2" };
                service.Repo.Insert(b2);
                await service.Uow.SaveChangesAsync();

                var gb1 = await service.GetAsync(b1.Id);
                Assert.Equal(ResultStatus.Succeed, gb1.Status);
                Assert.Equal(gb1.Data.Id, b1.Id);
                // check not found
                var result = await service.GetAsync(b2.Id + 1);
                Assert.Equal(ResultStatus.NotFound, result.Status);
            });
        }

        [Fact]
        public void Test_CreateAsync()
        {
            WithService(async (service) =>
            {
                var dto = new BookCreateUpdateDto() { Title = "create" };
                var r = await service.CreateAsync(dto);
                Assert.Equal(ResultStatus.Succeed, r.Status);
                Assert.Contains("created", r.Description);
                Assert.Equal(1, await service.Repo.Query().CountAsync());
            });
        }

        [Fact]
        public void Test_BulkCreateAsync()
        {
            WithService(async (service) =>
            {
                var creates = new List<BookCreateUpdateDto>(){
                    new BookCreateUpdateDto(){ Title= "a1" },
                    new BookCreateUpdateDto(){ Title= "a2" },
                    new BookCreateUpdateDto(){ Title= "a3" },
                };
                var result = await service.BulkCreateAsync(creates);
                Assert.Equal(3, result.Count);
            });
        }

        [Fact]
        public void Test_CreateCallbackAsync()
        {
            WithService(async (service) =>
            {
                var dto = new BookCreateUpdateDto() { Title = "create" };
                var r = await service.CreateAsync(dto);
                Assert.Equal(ResultStatus.Failed, r.Status);
                Assert.Contains("create failed", r.Description);

            }, "TestCallbackService");
        }

        [Fact]
        public void Test_UpdateAsync()
        {
            WithService(async (service) =>
            {
                var book = new Book() { Title = "a1", CreatedAt = DateTime.UtcNow };
                await service.Repo.InsertAndSaveAsync(book);
                var dto = new BookCreateUpdateDto() { Title = "bb" };
                // check not found
                var r1 = await service.UpdateAsync(book.Id + 1, dto);
                Assert.Equal(ResultStatus.NotFound, r1.Status);

                // updated
                var r2 = await service.UpdateAsync(book.Id, dto);
                Assert.Equal(ResultStatus.Succeed, r2.Status);
                // db updated
                Assert.Equal(dto.Title, (await service.Repo.FindAsync(book.Id)).Title);
                // don't override not changed
                Assert.Equal(book.CreatedAt, r2.Data.CreatedAt);
                Assert.Contains("updated", r2.Description);
            });
        }

        [Fact]
        public void Test_UpdateCallbackAsync()
        {
            WithService(async (service) =>
            {
                var book = new Book() { Title = "a1" };
                await service.Repo.InsertAndSaveAsync(book);
                var dto = new BookCreateUpdateDto() { Title = "bb" };

                var r2 = await service.UpdateAsync(book.Id, dto);
                Assert.Equal(ResultStatus.Failed, r2.Status);
                Assert.Contains("update failed", r2.Description);
            }, "TestCallbackService");
        }

        [Fact]
        public void Test_DeleteAsync()
        {
            WithService(async (service) =>
            {
                var b1 = new Book() { Title = "b1" };
                await service.Repo.InsertAndSaveAsync(b1);

                var r1 = await service.DeleteAsync(b1.Id + 1);
                Assert.Equal(ResultStatus.NotFound, r1.Status);

                var r2 = await service.DeleteAsync(b1.Id);
                Assert.Equal(ResultStatus.Succeed, r2.Status);
                Assert.Contains("deleted", r2.Description);
                // db deleted
                Assert.Equal(0, await service.Repo.Query().CountAsync());
            });
        }

        [Fact]
        public void Test_DeleteCallbackAsync()
        {
            WithService(async (service) =>
            {
                var b1 = new Book() { Title = "b1" };
                await service.Repo.InsertAndSaveAsync(b1);

                var r2 = await service.DeleteAsync(b1.Id);
                Assert.Equal(ResultStatus.Failed, r2.Status);
                Assert.Contains("delete failed", r2.Description);

            }, "TestCallbackService");
        }

        [Fact]
        public void Test_ExistsAsync()
        {
            WithService(async (service) =>
            {
                var b1 = new Book() { Title = "b1" };
                await service.Repo.InsertAndSaveAsync(b1);

                Assert.True(await service.ExistsAsync(b1.Id));
                Assert.False(await service.ExistsAsync(b1.Id + 1));
            });
        }

        [Fact]
        public void Test_EntityDto()
        {
            WithService((service) =>
            {
                var book = new Book() { Title = "ccc" };
                var dto = service.EntityDto(book);
                Assert.True(dto.Title == "ccc");
            });
        }

        [Fact]
        public void Test_UpdateDto()
        {
            WithService((service) =>
            {
                var book = new Book() { Title = "ccc" };
                var dto = service.UpdateDto(book);
                Assert.True(dto.Title == "ccc");
            });
        }

        [Fact]
        public void Test_AuthorService()
        {
            Utils.WithTestDatabase(async (uow) =>
            {
                var mapper = AutoMapperConfig.CreateMapper((cfg) => cfg.AddProfile(new MappingProfile()));
                var config = new ConfigurationBuilder().Build();

                var service = new AuthorService(uow, mapper, new AuthorValidator());
                var authorRepo = uow.GetRepo<Author>();
                authorRepo.DeleteAll();
                var bookRepo = uow.GetRepo<Book>();
                bookRepo.DeleteAll();

                var a1 = new Author() { Name = "a1" };
                var a2 = new Author() { Name = "a2" };
                foreach (var e in new Author[] { a1, a2 }) { await authorRepo.InsertAndSaveAsync(e); }

                var b11 = new Book() { Title = "china 1", AuthorId = a1.Id };
                var b12 = new Book() { Title = "us 1", AuthorId = a1.Id };
                var b21 = new Book() { Title = "us 2", AuthorId = a2.Id };
                var b22 = new Book() { Title = "us 3", AuthorId = a2.Id };
                foreach (var e in new Book[] { b11, b12, b21, b22 }) { await bookRepo.InsertAndSaveAsync(e); }

                var books = await service.RelatedBooks("china", "us");
                Assert.Single(books);
                Assert.Equal(b12.Id, books[0].Id);
            });
        }

    }
}