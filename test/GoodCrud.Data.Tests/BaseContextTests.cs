using System;
using Xunit;
using Books.Domain;
using Books.Data;
using System.Threading;
using GoodCrud.Data.Tests.Helpers;

namespace GoodCrud.Data.Tests
{
    public class EventTest
    {
        [Fact]
        public void Test_Timestamps()
        {
            Utils.WithTestDatabase(async (uow) =>
            {
                uow.BookRepo.DeleteAll();
                // assign CreatedAt and UpdatedAt
                var book = new Book() { Title = "b1" };
                uow.BookRepo.Insert(book);
                await uow.SaveChangesAsync();

                var today = DateTime.UtcNow.Date;
                Assert.NotNull(book.CreatedAt);
                if (book.CreatedAt == null)
                {
                    Assert.Equal(today, ((DateTime)book.CreatedAt).Date);
                }

                Assert.Equal(book.CreatedAt, book.UpdatedAt);

                // has CreatedAt will use it
                var bookN = new Book() { Title = "b2", CreatedAt = today };
                uow.BookRepo.Insert(bookN);
                await uow.SaveChangesAsync();
                Assert.Equal(today, bookN.CreatedAt);

                // change will touch UpdatedAt
                var old = book.UpdatedAt;
                book.Title = "b1u";
                Thread.Sleep(1);
                uow.BookRepo.Update(book);
                await uow.SaveChangesAsync();
                Assert.NotEqual(old, book.UpdatedAt);

                // async save will work
                var old2 = book.UpdatedAt;
                book.Title = "b1u2";
                Thread.Sleep(1);
                uow.BookRepo.Update(book);
                await uow.SaveChangesAsync();
                Assert.NotEqual(old2, book.UpdatedAt);
            });
        }
    }
}