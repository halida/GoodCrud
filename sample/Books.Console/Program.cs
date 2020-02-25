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
            // create
            int bookId;
            {
                var result = await client.CreateAsync(new BookCreateUpdateDto() { Title = "a2" });
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
                bookId = result.Data.Id;
            }
            // update
            {
                var result = await client.UpdateAsync(bookId, new BookCreateUpdateDto() { Title = "a3" });
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
            }

            // delete
            {
                var result = await client.DeleteAsync(bookId);
                System.Console.WriteLine(JsonConvert.SerializeObject(result));
            }

        }
    }
}
