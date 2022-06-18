using System.Collections.Concurrent;
using System.Text;
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
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        List<Func<Task<Post>>> funcs = new();
        
        for (int i = 1; i <= 100; i++)
        {
            var count = i; // !!!! крайневажная переменная без нее все работает неправильно
            funcs.Add(new( async () => await GetPostAsync(count, token)));
        }
        var isListsEqual = true;
        /*
        try
        {
            var list = await funcs.RunInParallel(5, true);

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
        catch (AggregateException ex)
        {
            foreach (Exception innerException in ex.InnerExceptions)
            {
                Console.WriteLine(innerException.Message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ex "+ex.Message);
        }
        */
        var asyncEnumList = funcs.RunInParallelAlt(tokenSource, 5);
        var postsList = new List<Post>();
        try
        {
            await foreach (var post2 in asyncEnumList)
            {
                var post1 = listInThread.FirstOrDefault(x => x.Id == post2.Id);
                var isPostsEqual = post1?.Id == post2?.Id && post1?.Body == post2?.Body && post1?.Title == post2?.Title && post1?.UserId == post2?.UserId;
                isListsEqual = isListsEqual && isPostsEqual;//тут конечно, неверное сравнение листов происходит, т к listInThread может быть больше, чем asyncEnumList
                postsList.Add(post2);
                Console.WriteLine(post2.Id+ " In main");
            }
            Console.WriteLine(isListsEqual);
        }
        catch (AggregateException ex)
        {
            foreach (Exception innerException in ex.InnerExceptions)
            {
                Console.WriteLine(innerException.Message);
            }
            Console.WriteLine(postsList.Count); // получение и проверка списка постов из IAsyncEnum
        }
        
        Console.ReadKey();
    }
    static async Task<Post> GetPostAsync(int number, CancellationToken ct)
    {
        if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
        Console.WriteLine($"Task started");
        // генерация нескольких ошибок
        if (number % 10 == 0)  throw new Exception("Ошибка при получении поста номер: "+number);
        var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
        response.EnsureSuccessStatusCode();
        var responseText = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var post = JsonSerializer.Deserialize<Post>(responseText, options);
        await Task.Delay(1500);
        Console.WriteLine("Task ended_" +post.Id);
        return post;
    }
}