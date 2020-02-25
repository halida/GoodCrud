using GoodCrud.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using URF.Core.EF;

namespace GoodCrud.Data
{
    public class BaseUnitOfWork<C> : UnitOfWork, IBaseUnitOfWork
    where C : BaseContext
    {
        public BaseUnitOfWork(DbContext context) : base(context)
        {
        }

        public virtual object GetRepo(Type type)
        {
            throw new System.ArgumentException($"Cannot find repo for type: {type}");
        }


    }
}
