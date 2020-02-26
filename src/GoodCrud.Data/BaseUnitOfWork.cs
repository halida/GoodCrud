using GoodCrud.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using URF.Core.EF;

namespace GoodCrud.Data
{
    public class BaseUnitOfWork : UnitOfWork, IBaseUnitOfWork
    {
        public BaseUnitOfWork(BaseContext context) : base(context)
        {
        }

        public virtual IRepo<E> GetRepo<E>() where E : class, IIdentifiable
        {
            return new Repo<E>((BaseContext)Context);
        }
        public virtual IRepo<E> GetRepo<E>(E ignored) where E : class, IIdentifiable
        {
            return GetRepo<E>();
        }


    }
}
