using System.Collections.Generic;
using System.Threading.Tasks;
using GoodCrud.Domain.Contract.Interfaces;
using URF.Core.Abstractions;

namespace GoodCrud.Data.Contract.Interfaces
{
    public interface IRepo<E> : IRepository<E>
        where E : class, IIdentifiable
    {
        Task InsertAndSaveAsync(E entity);
        Task UpdateAndSaveAsync(E entity);
        Task DeleteAndSaveAsync(E entity);

        void DeleteAll();
        string TableName();
        Task BulkInsertAsync(IEnumerable<E> entities);
    }

    public enum DatabaseProvider
    {
        Unknown,
        Sqlite,
        InMemory,
        MySql,
        SqlServer
    }

}
