using GoodCrud.Data.Contract.Interfaces;
using GoodCrud.Domain.Contract.Interfaces;
using System;
using System.Collections.Generic;
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


    }
}
