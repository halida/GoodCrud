using System;
using GoodCrud.Contract.Interfaces;

namespace GoodCrud.Domain
{
    public class BaseEntity : IIdentifiable
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
