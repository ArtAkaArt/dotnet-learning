using System;
using System.Threading.Tasks;
using Sol5_1_Collections;
using System.Text.Json;
using System.Threading;
using System.Net.Http;

namespace MyEnumTesting
{
    public  class Methods
    {
        static HttpClient client = new HttpClient();
        public static async Task<Post> GetPostAsync1(int number, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();

            var rnd = new Random();
            var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var post = JsonSerializer.Deserialize<Post>(responseText, options);
            await Task.Delay(rnd.Next(1000, 2000), ct);
            if (number == 1) throw new Exception("Ошибка при получении поста номер: " + number);
            return post;
        }
        public static async Task<Post> GetPostAsync2(int number, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();

            var rnd = new Random();
            var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var post = JsonSerializer.Deserialize<Post>(responseText, options);
            await Task.Delay(rnd.Next(1000, 2000), ct);
            if (number % 10 == 0) throw new Exception("Ошибка при получении поста номер: " + number);
            return post;
        }
        public static async Task<Post> GetPostAsync3(int number, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();

            var rnd = new Random();
            var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var post = JsonSerializer.Deserialize<Post>(responseText, options);
            await Task.Delay(rnd.Next(1000, 2000), ct);
            return post;
        }
        public static async Task<Post> GetPostAsync4(int number, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();

            var rnd = new Random();
            var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var post = JsonSerializer.Deserialize<Post>(responseText, options);
            await Task.Delay(rnd.Next(1000, 2000), ct);
            throw new Exception("Ошибка при получении поста номер: " + number);
            return post;
        }
    }
}
