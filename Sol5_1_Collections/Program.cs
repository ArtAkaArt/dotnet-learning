using System.Text.Json;

namespace Sol5_1_Collections;
public class Program {
    static List<Post> list = new();
    static async Task Main()
    {
        List <Task> tasklist = new();

        //Task task = new Task(async() => await GetPostAsync(1, list));
        Task task = GetPostAsync(1, list);
        //task.Start();
        task.Wait();
        Console.WriteLine(list.Count()+"----------");
        Console.ReadKey();

    }
    static async Task GetPostAsync(int number, List<Post> list)
    {
        
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}"); // здесь происходит переход на завергение программы
        var responseText = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        Post post = JsonSerializer.Deserialize<Post>(responseText, options);
        Console.WriteLine(post.Body);
        list.Add(post);
    }
}
public static class MyTaskListExtention
{
    public static async Task RunInParallel(this IEnumerable<Task> taskList, int vol = 4)
    {
    }
}