using System.Collections.Concurrent;
using System.Text.Json;

namespace Sol5_1_Collections;
public class Program {
    
    static HttpClient client = new HttpClient();
    static async Task Main()
    {
        //получаю лист в одном треде, для сравнения с ним
        List<Post> listInThread = new();
        try
        {
            var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            listInThread = JsonSerializer.Deserialize<List<Post>>(responseText, options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        List<Func<Task<Post>>> funcs = new();
        
        for (int i = 1; i <= 100; i++)
        {
            var count = i; // !!!! крайневажная переменная без нее все работает неправильно
            funcs.Add(new( async () => await GetPostAsync(count)));
        }
        var isListsEqual = true;
        try
        {
            var list = await funcs.RunInParallel(5);

            Console.WriteLine("Main - после RunInParallel");
            
            //дааа... очень топорная сверка
            for (int i = 1; i <= 100; i++)
            {
                var post1 = listInThread.FirstOrDefault(x => x.Id == i);
                var post2 = list.FirstOrDefault(x => x.Id == i);
                var isPostsEqual = post1?.Id == post2?.Id && post1?.Body == post2?.Body && post1?.Title == post2?.Title && post1?.UserId == post2?.UserId;
                isListsEqual = isListsEqual && isPostsEqual;
            }
            Console.WriteLine(isListsEqual);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.ReadKey();
    }
    static async Task<Post> GetPostAsync(int number)
    {
        
        Console.WriteLine($"Task started");
        if (number % 10 == 0) { Console.WriteLine("ex"); throw new Exception("Ошибка номер "+number); }
        var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
        response.EnsureSuccessStatusCode();
        var responseText = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var post = JsonSerializer.Deserialize<Post>(responseText, options);
        await Task.Delay(500);
        Console.WriteLine(111);
        await Task.Delay(500);
        Console.WriteLine(222);
        await Task.Delay(500);
        Console.WriteLine(333);
        await Task.Delay(500);
        Console.WriteLine("Task ended_" +post.Id);
        return post;
    }
}
public static class MyTaskListExtention
{
    public static async Task<IReadOnlyCollection<Post>> RunInParallel(this IEnumerable<Func<Task<Post>>> functs, int vol = 4, bool throwException = false )
    {
        ConcurrentBag<Post> list = new ();
        var semaphore = new SemaphoreSlim(vol);
        var tasks = new List<Task>();
        try
        {
        foreach (var func in functs)
        {
            var task = await Task.Factory.StartNew(async () => {
                await semaphore.WaitAsync();
                try
                {
                    list.Add(await func.Invoke());
                    semaphore.Release();

                } catch (Exception ex)
                {
                    semaphore.Release();
                    if (throwException)
                    {
                        throw;
                    }
                }
            });
            tasks.Add(task);
        }
        Console.WriteLine(tasks.Count(t => t != null) +" Task count");
        await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            throw;
        }
        Console.WriteLine("End in RunInParallel");
        return list;
    }
}