using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Books.Client;
using Books.Application;
using Newtonsoft.Json;

namespace Books.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            var task = TestRun();
            task.Wait();
        }

        public static async Task TestRun()
        {
            var client = new BooksClient("http://localhost:5000");
            // gets
            {
                var result = await client.GetsAsync();
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
            }

            int entityId = 0;
            // get by title
            {
                var result = await client.GetsAsync(new BookFilterDto() { Title = "a2" });
                if (result.List != null && result.List.Count > 0) { entityId = result.List[0].Id; }
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
            }

            // create
            if (entityId <= 0)
            {
                var result = await client.CreateAsync(new BookCreateUpdateDto() { Title = "a2" });
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
                entityId = result.Data.Id;
            }
            // get
            {
                var result = await client.GetAsync(entityId);
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
            }
            // update
            {
                var result = await client.UpdateAsync(entityId, new BookCreateUpdateDto() { Title = "a3" });
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
            }

            // delete
            {
                var result = await client.DeleteAsync(entityId);
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
            }

        }
    }
}
