using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace GoodCrud.Web.Helpers
{
    public class Testing
    {
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
            var cookieContent = response.Headers.FirstOrDefault(x => x.Key == "Set-Cookie").Value.First().Split(' ')[0];
            var tokenCookie = cookieContent.Split('=');
            var name = tokenCookie[0];
            var value = tokenCookie[1];
            return new KeyValuePair<string, string>(name, value);
        }

        public static void RemoveService(IServiceCollection services, Type t)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == t);

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

        }

    }
}