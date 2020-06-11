using GoodCrud.Data.Contract.Interfaces;
using GoodCrud.Domain.Contract.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using URF.Core.EF;

namespace GoodCrud.Data
{
    public class BaseUnitOfWork : UnitOfWork, IBaseUnitOfWork
    {
        public Dictionary<Type, dynamic> RepoCache;
        public BaseUnitOfWork(BaseContext context) : base(context)
        {
            RepoCache = new Dictionary<Type, dynamic>() { };
        }

        public virtual IRepo<E> GetRepo<E>() where E : class, IIdentifiable
        {
            var type = typeof(E);
            try
            {
                return RepoCache[type];
            }
            catch (KeyNotFoundException)
            {
                var newRepo = new Repo<E>((BaseContext)Context);
                RepoCache[type] = newRepo;
                return newRepo;
            }

        }
        public virtual IRepo<E> GetRepo<E>(E ignored) where E : class, IIdentifiable
        {
            return GetRepo<E>();
        }

        public TProperty? GetReference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> property)
        where TEntity : class, IIdentifiable where TProperty : class
        {
            var getProperty = property.Compile();
            var value = getProperty(entity);
            if (value != null) { return value; }

            Context.Entry(entity).Reference(property).Load();
            return getProperty(entity);
        }

        public async Task<TProperty?> GetReferenceAsync<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> property)
        where TEntity : class, IIdentifiable where TProperty : class
        {
            var getProperty = property.Compile();
            var value = getProperty(entity);
            if (value != null) { return value; }

            await Context.Entry(entity).Reference(property).LoadAsync();
            return getProperty(entity);
        }

        public IQueryable<TProperty> GetCollectionQuery<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>?>> property)
        where TEntity : class, IIdentifiable where TProperty : class
        {
            return Context.Entry(entity).Collection(property).Query();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public async Task WithTransaction(Action<IDbContextTransaction> func)
        {
            using var transaction = BeginTransaction();
            try
            {
                func(transaction);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
    }
}
