using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using DadataRequestLibrary;
using Serilog;
namespace Solution
{

    class Programm
    {
        private static DadataConfiguration tokenContainer;
        static HttpClient httpClient = new HttpClient();
        public static async Task Main()
        { 
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();

            tokenContainer = config.GetSection("Configuration").Get<DadataConfiguration>();

            Log.Logger = new LoggerConfiguration() // логгер то есть, но как его передать
                            .WriteTo.File($"{Directory.GetCurrentDirectory()}consoleapp.log")
                            .CreateLogger();

            var requester = new DadataLibrary(tokenContainer); // добавил еще 1 конструктор и отключил логирование в либе пока что
            Console.WriteLine("Введите ИНН");
            var INN = Console.ReadLine();
            
            while (true)
            {
                var companyName = new CompanyNameQueryResult();
                if (Regex.IsMatch(INN, @"^\d{10}$|^\d{12}$"))
                    companyName = await requester.GetCompanyName(INN);// или вызвать GetCompanyNameAlt
                Console.WriteLine(companyName.CompanyName != null ? $"Название компании - {companyName.CompanyName}" : $"Произошла ошибка. {companyName.Error}");
                Console.WriteLine("Для продолжения поиска введите ИНН. Для заершения \"-\" без кавычек");
                INN = Console.ReadLine();
                if (INN == "-") break;
            }
            Console.WriteLine("Завершение");
        }
        private static async Task<CompanyNameQueryResult> GetCompanyNameAlt(string INN)
        {
            try
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://suggestions.dadata.ru/suggestions/api/4_1/rs/findById/party"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    request.Headers.TryAddWithoutValidation("Authorization", $"Token {tokenContainer.Token}");

                    request.Content = new StringContent("{ \"query\": \"" + INN + "\" }");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    var responseLine = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonSerializer.Deserialize<Rootobject>(responseLine);

                    var companyName = deserializedResponse?.suggestions[0]?.data?.name?.full;
                    if (companyName is null) 
                        return new CompanyNameQueryResult { CompanyName = companyName, Error = "Имя компанни не найдено"};

                    return new CompanyNameQueryResult { CompanyName = companyName, };
                }
            }
            catch (Exception ex)
            {
                return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            }
        }
    }
}