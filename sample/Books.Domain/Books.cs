using GoodCrud.Domain;

namespace Books.Domain
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
    }

    public class Author : BaseEntity
    {
        public string Name { get; set; }
    }
}