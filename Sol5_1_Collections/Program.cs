using System.Text.Json;

namespace Sol5_1_Collections;
public class Program {
    static volatile List<Post> list = new();
    
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

        List<Func<Task>> funcs = new();
        
        for (int i = 1; i <= 100; i++)
        {
            var count = i; // !!!! крайневажная переменная без нее все работает неправильно
            Console.WriteLine(i);
            funcs.Add(new( async () => await GetPostAsync(count, list)));
        }
        await funcs.RunInParallel(5);
        Console.WriteLine("Main thread - после RunInParallel");

        //дааа... очень топорная сверка
        for (int i = 1; i <= 100; i++)
        {
            var post1 = listInThread.FirstOrDefault(x => x.Id == i);
            var post2 = list.FirstOrDefault(x => x.Id == i);
            var isEqual = post1.Id == post2.Id && post1.Body == post2.Body && post1.Title == post2.Title && post1.UserId == post2.UserId;
            Console.WriteLine(isEqual);
        }
        Console.ReadKey();
    }
    static async Task GetPostAsync(int number, List<Post> list)
    {
        
        Console.WriteLine($"Task started");
        
        var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
        var responseText = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var post = JsonSerializer.Deserialize<Post>(responseText, options);
        Console.WriteLine("Task ended_" +post.Id);
        list.Add(post);
        
    }
}
public static class MyTaskListExtention
{
    public static async Task RunInParallel(this IEnumerable<Func<Task>> functs, int vol = 4)
    {
        functs.Count();
        var semaphore = new SemaphoreSlim(vol);
        //List<Task> tasks = new();
        var tasks = new Task[functs.Count()];
        var count = 0;
        foreach (var func in functs)
        {
            await semaphore.WaitAsync();
            var task = await Task.Factory.StartNew(async() => { await func.Invoke(); semaphore.Release();});
            tasks[count] = task;
            Console.WriteLine($"Task added count {semaphore.CurrentCount}");
            //semaphore.Release();
            count++;
        }
        Console.WriteLine(tasks.Length+" Task count");
        await Task.WhenAll(tasks);
        Console.WriteLine("End in RunInParallel");
    }
}