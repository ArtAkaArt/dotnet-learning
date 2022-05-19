using System.Text.Json;


namespace Sol5_1_Collections;
public class Program {

    static readonly HttpClient client = new HttpClient();
    static async Task Main()
    {
        List<Post> list = new();
        try
        {
            var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            list = JsonSerializer.Deserialize<List<Post>>(responseText);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        list.ForEach(post => Console.WriteLine(post.Title));
        Console.ReadKey();
    }
}
