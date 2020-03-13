using GoodCrud.Data;
using Microsoft.EntityFrameworkCore;
using Books.Data.Contract;
using Books.Domain;

namespace Books.Data
{

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

        public virtual DbSet<Book>? Books { get; set; }
        public virtual DbSet<Author>? Authors { get; set; }

    }

    public class BooksUnitOfWork : BaseUnitOfWork, IBooksUnitOfWork
    {
        public BooksUnitOfWork(Context context) : base(context)
        {
        }

    }

}