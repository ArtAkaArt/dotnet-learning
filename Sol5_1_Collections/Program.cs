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
        var list = await funcs.RunInParallel(5);
        Console.WriteLine("Main - после RunInParallel");
        var isListsEqual = true;
        //дааа... очень топорная сверка
        for (int i = 1; i <= 100; i++)
        {
            var post1 = listInThread.FirstOrDefault(x => x.Id == i);
            var post2 = list.FirstOrDefault(x => x.Id == i);
            var isPostsEqual = post1.Id == post2.Id && post1.Body == post2.Body && post1.Title == post2.Title && post1.UserId == post2.UserId;
            isListsEqual = isListsEqual && isPostsEqual;   
        }
        Console.WriteLine(isListsEqual);
        Console.ReadKey();
    }
    static async Task<Post> GetPostAsync(int number)
    {
        
        Console.WriteLine($"Task started");
        
        var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
        var responseText = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var post = JsonSerializer.Deserialize<Post>(responseText, options);
        Thread.Sleep(2500);
        Console.WriteLine(111);
        Thread.Sleep(2500);
        Console.WriteLine(222);
        Thread.Sleep(2500);
        Console.WriteLine(333);
        Thread.Sleep(2500);
        Console.WriteLine("Task ended_" +post.Id);
        return post;
    }
}
public static class MyTaskListExtention
{
    public static async Task<IReadOnlyCollection<Post>> RunInParallel(this IEnumerable<Func<Task<Post>>> functs, int vol = 4)
    {
        ConcurrentBag<Post> list = new ();
        var semaphore = new SemaphoreSlim(vol);
        var tasks = new Task[functs.Count()];
        var count = 0;
        foreach (var func in functs)
        {
            var task = await Task.Factory.StartNew(async() => {
                await semaphore.WaitAsync();
                list.Add(await func.Invoke());
                semaphore.Release();
            });
            tasks[count] = task;
            count++;
        }
        Console.WriteLine(tasks.Count(t => t != null) +" Task count");
        await Task.WhenAll(tasks);
        Console.WriteLine("End in RunInParallel");
        return list;
    }
}