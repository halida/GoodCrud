using GoodCrud.Contract.Interfaces;
using GoodCrud.Domain;

namespace Books.Domain
{
    public class Book : BaseEntity
    {
        public string Title;
        public string Description;
    }

    public interface IBooksUnitOfWork : IBaseUnitOfWork
    {
        IBookRepo BookRepo { get; set; }
    }
    public interface IBookRepo : IRepo<Book>
    {
    }

}