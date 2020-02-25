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

namespace GoodCrud.Web.Tests.Controllers.Api
{
    public class CrudControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CrudControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private HttpContent JsonContent(dynamic data)
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        }

        private async Task<JToken> ParseResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JToken>(content);
        }

        [Fact]
        public async Task Test_All()
        {
            var client = _factory.CreateClient();

            // gets all
            {
                var response = await client.GetAsync("api/Books/");

                response.EnsureSuccessStatusCode();
                // return json
                Assert.Equal("application/json; charset=utf-8",
                    response.Content.Headers.ContentType.ToString());
                var result = await ParseResponse(response);
                Assert.Equal(3, result["metaData"]["totalItemCount"]);
            }

            // create one
            var bookId = 0;
            {
                var data = new Dictionary<string, string> { { "Title", "a1" } };
                var response = await client.PostAsync("api/Books", JsonContent(data));
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Equal("Succeed", result["status"]);
                bookId = Convert.ToInt32(result["data"]["id"]);
            }

            // bulk create
            {
                var data = new List<dynamic>(){
                    new Dictionary<string, string> { { "Title", "a2" } },
                    new Dictionary<string, string> { { "Title", "a3" } },
                    new Dictionary<string, string> { { "Title", "a4" } },
                };
                var response = await client.PostAsync("api/Books/Bulk", JsonContent(data));
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Equal(3, result.Count());
                Assert.Equal("Succeed", result[2]["status"]);
            }

            // get one
            {
                var response = await client.GetAsync($"api/Books/{bookId}");
                var result = await ParseResponse(response);
                Assert.Equal("a1", result["data"]["title"]);
            }

            // update
            {
                var data = new Dictionary<string, string> { { "Title", "a8" } };
                var response = await client.PutAsync($"api/Books/{bookId}", JsonContent(data));
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Equal("Succeed", result["status"]);
                Assert.Contains("updated", Convert.ToString(result["description"]));
                Assert.Equal("a8", result["data"]["title"]);
            }

            // delete
            {
                var response = await client.DeleteAsync($"api/Books/{bookId}");
                response.EnsureSuccessStatusCode();
                var result = await ParseResponse(response);
                Assert.Contains("deleted", Convert.ToString(result["description"]));
            }

            //check delete
            {
                var response = await client.GetAsync($"api/Books/{bookId}");
                var result = await ParseResponse(response);
                Assert.Equal("NotFound", result["status"]);
            }
        }


    }

}