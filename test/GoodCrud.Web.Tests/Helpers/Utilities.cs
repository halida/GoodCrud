using System.Collections.Generic;
using Books.Data;
using Books.Domain;

namespace GoodCrud.Web.Tests.Helpers
{
    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(Context db)
        {
            db.Books.AddRange(GetSeedingBooks());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(Context db)
        {
            db.Books.RemoveRange(db.Books);
            InitializeDbForTests(db);
        }

        public static List<Book> GetSeedingBooks()
        {
            return new List<Book>()
            {
                new Book(){ Title = "b1" },
                new Book(){ Title = "b2" },
                new Book(){ Title = "b3" },
            };
        }
        #endregion
    }
}
