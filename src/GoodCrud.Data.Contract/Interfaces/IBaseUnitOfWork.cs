using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoodCrud.Domain.Contract.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using URF.Core.Abstractions;

namespace GoodCrud.Data.Contract.Interfaces
{
    public interface IBaseUnitOfWork : IUnitOfWork
    {
        IRepo<E> GetRepo<E>() where E : class, IIdentifiable;
        IRepo<E> GetRepo<E>(E ignored) where E : class, IIdentifiable;

        TPorperty? GetReference<TEntity, TPorperty>(TEntity entity, Expression<Func<TEntity, TPorperty?>> property)
        where TEntity : class, IIdentifiable where TPorperty : class;

        Task<TPorperty?> GetReferenceAsync<TEntity, TPorperty>(TEntity entity, Expression<Func<TEntity, TPorperty?>> property)
        where TEntity : class, IIdentifiable where TPorperty : class;

        IQueryable<TProperty> GetCollectionQuery<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>?>> property)
        where TEntity : class, IIdentifiable where TProperty : class;

        IDbContextTransaction BeginTransaction();
        Task WithTransaction(Action<IDbContextTransaction> func);
    }
}
