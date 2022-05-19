using System.Text.Json.Serialization;

namespace Sol5_1_Collections
{
    public class Post
    {
        //[JsonPropertyName("userId")] альтернатива игноркейс опции
        public int UserId { get; set; }
        //[JsonPropertyName("id")]
        public int Id { get; set; }
        //[JsonPropertyName("title")]
        public string Title { get; set; } = null!;
        //[JsonPropertyName("body")]
        public string Body { get; set; } = null!;
    }
}
