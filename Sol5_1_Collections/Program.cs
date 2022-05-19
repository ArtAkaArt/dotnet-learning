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
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            list = JsonSerializer.Deserialize<List<Post>>(responseText, options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        var unique = new Dictionary<string, Post>(); //коллекция для уникального поля Title
        list.ForEach(post =>unique.Add(post.Title, post));

        Console.WriteLine(unique["qui est esse"].Body); // проверка
        Console.ReadKey();
    }
}
