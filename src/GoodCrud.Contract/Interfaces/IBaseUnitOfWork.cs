using System;
using URF.Core.Abstractions;

namespace GoodCrud.Contract.Interfaces
{
    public interface IBaseUnitOfWork : IUnitOfWork
    {
        object GetRepo(Type type);

    }
}
