using System.Text.Json;

namespace Sol5_1_Collections;
public class Program
{

    static HttpClient client = new HttpClient();
    static async Task Main()
    {

        //List<Post> listInThread = await GetAllPosts();
        IEnumerable<Func<CancellationToken, Task<Post>>> funcs = ConstructList();
        Console.WriteLine(await CompareLists3(funcs));
    }
    public static async Task<bool> CompareLists3(IEnumerable<Func<CancellationToken, Task<Post>>> funcs)
    {
        var isListsEqual = true;
        var myExtPosts = new List<Post>();
        var myExtEx = new List<Exception>();
        var myEnumPosts = new List<Post>();
        var myEnumEx = new List<Exception>();
        var myEmumerable = new MyAsyncEnumerable<Post>(funcs, 5);
        try
        {
            await foreach (var post2 in myEmumerable)
            {
                myEnumPosts.Add(post2);
            }
        }
        catch (AggregateException ex)
        {
            foreach (Exception innerException in ex.InnerExceptions)
            {
                myEnumEx.Add(innerException);
            }
        }
        var asyncEnumList = funcs.RunInParallelAlt(5);
        try
        {
            await foreach (var post2 in asyncEnumList)
            {
                myExtPosts.Add(post2);
            }
        }
        catch (AggregateException ex)
        {
            foreach (Exception innerException in ex.InnerExceptions)
            {
                myExtEx.Add(innerException);
            }
        }
        if (myEnumPosts.Count   != myExtPosts.Count) return false;
        if (myEnumEx.Count      != myExtEx.Count) return false;
        foreach (var post1 in myEnumPosts)
        {
            var post2 = myExtPosts.FirstOrDefault(x => x.Id == post1.Id);
            var isPostsEqual = post1?.Id == post2?.Id && post1?.Body == post2?.Body && post1?.Title == post2?.Title && post1?.UserId == post2?.UserId;
            isListsEqual = isListsEqual && isPostsEqual;
        }
        foreach (var ex1 in myEnumEx)
        {
            var ex2 = myExtEx.FirstOrDefault(x => x.Message == ex1.Message);
            isListsEqual = isListsEqual && ex2 != null;
        }
        return isListsEqual;

    }

    public static async Task<bool> CompareLists2(List<Post> listInThread)
    {
        var isListsEqual = true;
        var postsList = new List<Post>();
        var funcs = ConstructList();
        var asyncEnumList = new MyAsyncEnumerable<Post>(funcs);
        try
        {
            await foreach (var post2 in asyncEnumList)
            {
                var post1 = listInThread.FirstOrDefault(x => x.Id == post2.Id);
                var isPostsEqual = post1?.Id == post2?.Id && post1?.Body == post2?.Body && post1?.Title == post2?.Title && post1?.UserId == post2?.UserId;
                isListsEqual = isListsEqual && isPostsEqual;
                postsList.Add(post2);
            }
        }
        catch (AggregateException ex)
        {
            foreach (Exception innerException in ex.InnerExceptions)
            {
                //нет сравнения ошибок с listInThread
            }
        }
        return isListsEqual;
    }
    public static async Task<bool> CompareLists1(List<Post> listInThread)
    {
        var isListsEqual = true;
        var funcs = ConstructList();
        try
        {
            var list = await funcs.RunInParallel(5, true);
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
                //нет сравнения ошибок с listInThread
            }
        }
        return isListsEqual;
    }
    public static async Task<List<Post>> GetAllPosts()
    {
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
        return listInThread;
    }
    public static List<Func<CancellationToken, Task<Post>>> ConstructList()
    {
        List<Func<CancellationToken, Task<Post>>> funcs = new();

        for (int i = 1; i <= 100; i++)
        {
            var count = i;
            funcs.Add(async (CancellationToken token) => await GetPostAsync(count, token));
        }
        return funcs;
    }
    static async Task<Post> GetPostAsync(int number, CancellationToken ct)
    {
        if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
        //Console.WriteLine($"Task started " + number);

        var rnd = new Random();
        var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{number}");
        response.EnsureSuccessStatusCode();
        var responseText = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var post = JsonSerializer.Deserialize<Post>(responseText, options);
        // Task.Delay(rnd.Next(1000, 2000), ct);
        // генерация нескольких ошибок
        if (number  == 1) throw new Exception("Ошибка при получении поста номер: " + number);
        //Console.WriteLine("Task ended_" + post.Id);
        return post;
    }
}