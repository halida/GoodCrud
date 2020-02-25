using System.Threading.Tasks;
using URF.Core.Abstractions;

namespace GoodCrud.Contract.Interfaces
{
    public interface IRepo<E> : IRepository<E>
    where E : class, IIdentifiable
    {
        Task InsertAndSaveAsync(E entity);
        Task UpdateAndSaveAsync(E entity);
        Task DeleteAndSaveAsync(E entity);

        void DeleteAll();
        DatabaseProvider GetDatabaseProvider();
        string TableName();
    }

    public enum DatabaseProvider
    {
        Unknown,
        Sqlite,
        InMemory,
        Mysql,
        SqlServer
    }

}
