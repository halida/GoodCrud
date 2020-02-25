using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using GoodCrud.Web.Books;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using GoodCrud.Web.Tests.Helpers;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace GoodCrud.Web.Tests.Controllers
{
    public class CrudControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CrudControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private async Task<string> ParseResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        [Fact]
        public async Task Test_All()
        {
            var client = _factory.CreateClient();

            // gets all
            {
                var response = await client.GetAsync("Books/");

                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Contains("Title", result);
            }

            // create form
            var bookId = 0;
            string authToken;
            KeyValuePair<string, string> cookie;
            {
                var response = await client.GetAsync("/Books/Create");
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Contains("Create Book", result);
                authToken = Utilities.ExtractAntiForgeryToken(result);
                cookie = Utilities.ExtractCookie(response);
            }

            // create
            {
                var data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("__RequestVerificationToken", authToken),
                    new KeyValuePair<string, string>("Title", "a11"),
                    new KeyValuePair<string, string>("Descirption", "desc1")
                };
                // var rs = await ParseResponse((await client.GetAsync("api/Books")));

                var request = new HttpRequestMessage(HttpMethod.Post, "/Books/Create") { Content = new FormUrlEncodedContent(data) };
                request.Headers.Add("Cookie", $"{cookie.Key}={cookie.Value}");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);

                // var rs2 = await ParseResponse((await client.GetAsync("api/Books")));

                Assert.Contains("created", result);
                var pattern = new Regex(@"Books #(?<bookId>\d+) ");
                var match = pattern.Match(result);
                bookId = int.Parse(match.Groups["bookId"].Value);
            }

            // get one
            {
                var response = await client.GetAsync($"/Books/{bookId}");
                var result = await ParseResponse(response);
                Assert.Contains($"Books #{bookId}", result);
            }

            // edit form
            {
                var response = await client.GetAsync($"/Books/{bookId}/Edit");
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Contains("Edit Book", result);
                authToken = Utilities.ExtractAntiForgeryToken(result);
                cookie = Utilities.ExtractCookie(response);
            }

            // update
            {
                var data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("__RequestVerificationToken", authToken),
                    new KeyValuePair<string, string>("Title", "a1"),
                    new KeyValuePair<string, string>("Descirption", "desc2")
                };

                var request = new HttpRequestMessage(HttpMethod.Post, $"/Books/{bookId}/Edit") { Content = new FormUrlEncodedContent(data) };
                request.Headers.Add("Cookie", $"{cookie.Key}={cookie.Value}");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Contains("updated", result);
            }

            // delete
            {
                var response = await client.PostAsync($"/Books/{bookId}/Delete", null);
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Contains("deleted", result);
            }

        }


    }

}