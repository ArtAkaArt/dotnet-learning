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
        var uniqueTitle = new Dictionary<string, Post>();   //коллекция для уникального поля Title
        list.OrderBy(p =>p.Title)                           //если правильно помню по отсортированному быстрее поиск проходит
            .ToList()
            .ForEach(post => uniqueTitle.Add(post.Title, post));

        var uniqueId = new Dictionary<int, Post>(); //коллекция для уникального поля Id, предполагая, что Id уникален
        list.ForEach(post => uniqueId.Add(post.Id, post));

        //если нет никаких уникальных полей, то так в List и оставить и дергать линком по специфичным запросам, наверно

        Console.WriteLine(uniqueTitle["qui est esse"].Body); // проверка
        Console.WriteLine(uniqueId[2].Body); // проверка

        Console.ReadKey();
    }
}
