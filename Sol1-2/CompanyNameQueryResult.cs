
namespace Solution
{
    internal class CompanyNameQueryResult
    {
        public string? CompanyName { get; set; }
        public string? Error { get; set; }
    }
}
//тут исхожу из того что, класс при создании будет содержать или имя или сообщение ошибки, поэтому через нуль оператор