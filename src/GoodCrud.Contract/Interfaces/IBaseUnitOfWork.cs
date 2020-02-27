using URF.Core.Abstractions;

namespace GoodCrud.Contract.Interfaces
{
    public interface IBaseUnitOfWork : IUnitOfWork
    {
        IRepo<E> GetRepo<E>() where E : class, IIdentifiable;
        IRepo<E> GetRepo<E>(E ignored) where E : class, IIdentifiable;
    }
}
