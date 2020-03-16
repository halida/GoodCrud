using GoodCrud.Domain;
using System.Collections.Generic;

namespace Books.Domain
{
    public class Author : BaseEntity
    {
        public string? Name { get; set; }

        public virtual List<Book>? Books { get; set; }
    }
}