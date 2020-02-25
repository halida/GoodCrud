using System;
using Books.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GoodCrud.Data.Tests.Helpers
{
    public class Utils
    {
        public static void WithTestDatabase(Action<IBooksUnitOfWork> func)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<Context>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new Context(options))
                {
                    context.Database.EnsureCreated();
                }

                // Run the test against one instance of the context
                using (var context = new Context(options))
                {
                    var uow = new BooksUnitOfWork(context);
                    func(uow);
                }
            }
            finally
            {
                connection.Close();
            }

        }

    }
}