using GoodCrud.Domain.Contract.Interfaces;
using URF.Core.Abstractions;

namespace GoodCrud.Data.Contract.Interfaces
{
    public interface IBaseUnitOfWork : IUnitOfWork
    {
        IRepo<E> GetRepo<E>() where E : class, IIdentifiable;
        IRepo<E> GetRepo<E>(E ignored) where E : class, IIdentifiable;
    }
}
