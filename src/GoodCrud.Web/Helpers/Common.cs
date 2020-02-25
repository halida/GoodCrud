using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace GoodCrud.Web.Helpers
{
    public class Common
    {
        public static string GetUpdatedUri(HttpRequest request, string key, string value)
        {
            var query = request.Query;
            var qb = new QueryBuilder();
            qb.Add(key, value);

            foreach (var item in query)
            {
                if (item.Key == key) { continue; }
                qb.Add(item.Key, item.Value.First());
            }

            return request.Path + qb.ToQueryString();
        }

        public static string IgnoreException(Func<string> func)
        {
            try
            {
                return func();
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
