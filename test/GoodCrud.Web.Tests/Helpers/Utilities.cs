using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Books.Data;
using Books.Domain;

namespace GoodCrud.Web.Tests.Helpers
{
    public static class Utilities
    {
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

        public static string ExtractAntiForgeryToken(string htmlBody)
        {
            var AntiForgeryFieldName = "__RequestVerificationToken";
            var requestVerificationTokenMatch = Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
            {
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            }

            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in HTML", nameof(htmlBody));
        }

        public static KeyValuePair<string, string> ExtractCookie(HttpResponseMessage response)
        {
            var cookieContent = response.Headers.FirstOrDefault(x => x.Key == "Set-Cookie").Value.First().Split(" ")[0];
            var tokenCookie = cookieContent.Split("=");
            var name = tokenCookie[0];
            var value = tokenCookie[1];
            return new KeyValuePair<string, string>(name, value);
        }

    }
}
