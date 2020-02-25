using System;
using Books.Domain;
using GoodCrud.Contract.Interfaces;
using GoodCrud.Data;
using GoodCrud.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class BookRepo : Repo<Book, Context>, IBookRepo
    {
        public BookRepo(Context context) : base(context) { }
    }


    public class Context : BaseContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(e =>
            {
                e.HasIndex("Title").IsUnique();
            });
        }

        public virtual DbSet<Book> Books { get; set; }

    }

    public class BooksUnitOfWork : BaseUnitOfWork<Context>, IBooksUnitOfWork
    {
        public BooksUnitOfWork(Context context) : base(context)
        {
            BookRepo = new BookRepo(context);
        }

        public override object GetRepo(Type type)
        {
            if (type == typeof(Book)) { return this.BookRepo; }
            else { return base.GetRepo(type); }
        }

        public IBookRepo BookRepo { get; set; }

    }

}