using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoodCrud.Data.Contract.Interfaces;
using GoodCrud.Domain.Contract.Interfaces;
using URF.Core.EF;

namespace GoodCrud.Data
{
    public class Repo<E> : Repository<E>, IRepo<E>
    where E : class, IIdentifiable
    {
        public Repo(BaseContext context) : base(context)
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
            switch (((BaseContext)Context).GetDatabaseProvider())
            {
                case DatabaseProvider.Sqlite:
                    Context.Database.ExecuteSqlRaw($"DELETE FROM \"{TableName()}\"");
                    break;
                case DatabaseProvider.MySql:
                    Context.Database.ExecuteSqlRaw($"TRUNCATE TABLE \"{TableName()}\"");
                    break;
                default:
                    foreach (var item in Set.ToList()) { Set.Remove(item); }
                    Context.SaveChanges();
                    break;
            }
        }

        public string TableName()
        {
            return Context.Model.FindEntityType(typeof(E)).GetTableName();
        }
    }
}
