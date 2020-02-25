using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoodCrud.Contract.Interfaces;
using URF.Core.EF;

namespace GoodCrud.Data
{
    public abstract class Repo<E, C> : Repository<E>
    where E : class, IIdentifiable
    where C : BaseContext
    {
        public Repo(DbContext context) : base(context)
        {
        }

        public async Task InsertAndSaveAsync(E entity)
        {
            Insert(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync(E entity)
        {
            Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAndSaveAsync(E entity)
        {
            Delete(entity);
            await Context.SaveChangesAsync();
        }

        public void DeleteAll()
        {
            switch (GetDatabaseProvider())
            {
                case DatabaseProvider.Sqlite:
                    Context.Database.ExecuteSqlRaw($"DELETE FROM \"{TableName()}\"");
                    break;
                case DatabaseProvider.Mysql:
                    Context.Database.ExecuteSqlRaw($"TRUNCATE TABLE \"{TableName()}\"");
                    break;
                default:
                    foreach (var item in Set.ToList()) { Set.Remove(item); }
                    Context.SaveChanges();
                    break;
            }
        }

        public DatabaseProvider GetDatabaseProvider()
        {
            var s = Context.Database.ProviderName;
            switch (s)
            {
                case "Microsoft.EntityFrameworkCore.Sqlite": return DatabaseProvider.Sqlite;
                case "Microsoft.EntityFrameworkCore.InMemory": return DatabaseProvider.InMemory;
                default: return DatabaseProvider.Unknown;
            }
        }

        public string TableName()
        {
            return Context.Model.FindEntityType(typeof(E)).GetTableName();
        }
    }
}
