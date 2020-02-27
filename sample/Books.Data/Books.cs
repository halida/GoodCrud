using System;
using Books.Domain;
using GoodCrud.Contract.Interfaces;
using GoodCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class BookRepo : Repo<Book>, IBookRepo
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

    public class BooksUnitOfWork : BaseUnitOfWork, IBooksUnitOfWork
    {
        public BooksUnitOfWork(Context context) : base(context)
        {
            BookRepo = new BookRepo(context);
        }

        public override IRepo<E> GetRepo<E>() where E : class
        {
            if (typeof(E) == typeof(Book)) { return (IRepo<E>)this.BookRepo; }
            else { return base.GetRepo<E>(); }
        }

        public IBookRepo BookRepo { get; set; }

    }

}