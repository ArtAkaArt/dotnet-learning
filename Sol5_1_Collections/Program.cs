using System.Text.Json;

namespace Sol5_1_Collections;
public class Program {
    
    private static SemaphoreSlim semaphore;
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

        semaphore = new SemaphoreSlim(5);
        List<Func<Task<Post>>> funcs = new();
        
        for (int i = 1; i <= 100; i++)
        {
            var count = i; // !!!! крайневажная переменная без нее все работает неправильно
            funcs.Add(new( async () => await GetPostAsync(count))); // , list
        }
        var list = await funcs.RunInParallel(5);
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
    static async Task<Post> GetPostAsync(int number) // , List<Post> list
    {
        semaphore.Wait();
        Console.WriteLine($"Task started _ {semaphore.CurrentCount}");
        
        var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
        var responseText = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var post = JsonSerializer.Deserialize<Post>(responseText, options);
        Console.WriteLine("Task ended_" +post.Id);
        //list.Add(post);
        semaphore.Release();
        return post;
    }
}
public static class MyTaskListExtention
{
    /* передал значение по умолчанию, но чтобы передать его в семафор, нужно еще сам семафор объявлять в методе и => сами функции тут же
     * это выглядит немного странно в плане функциональности самого метода
     */
    static volatile List<Post> list = new();
    public static async Task<List<Post>> RunInParallel(this IEnumerable<Func<Task<Post>>> functs, int vol = 4)
    {
        List<Task> tasks = new();
        foreach (var func in functs)
        {
            var task = func.Invoke();
            tasks.Add(task);
            list.Add(await task);
            Console.WriteLine("Task added");
        }
        Console.WriteLine(tasks.Count()+" Task count");
        Task.WaitAll(tasks.ToArray());
        Console.WriteLine("End in RunInParallel");
        return list;
    }
}