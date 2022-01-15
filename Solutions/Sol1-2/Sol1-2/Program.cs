﻿using Dadata;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Solution
{

    class Programm
    {
        static HttpClient httpClient = new HttpClient();
        public static async Task Main()
        {
            Console.WriteLine("Введите ИНН");
            var INN = Console.ReadLine();
            var companyName = new CompanyNameQueryResult();

            while (true)
            {
                if (Regex.IsMatch(INN, @"^\d{10}$|^\d{12}$")) companyName = await GetCompanyNameAlt(INN);// или вызвать GetCompanyNameAlt
                Console.WriteLine(companyName.CompanyName != null ? $"Название компании - {companyName.CompanyName}" : $"Произошла ошибка. {companyName.Error}");
                Console.WriteLine("Для продолжения поиска введите ИНН. Для заершения \"-\" без кавычек");
                INN = Console.ReadLine();
                if (INN == "-") break;
            }

            Console.WriteLine("Завершение");
        }
        private static async Task<CompanyNameQueryResult> GetCompanyName(string INN)
        {
            try
            {
                var token = "***"; // вписать токен
                var api = new SuggestClientAsync(token);
                var response = await api.FindParty(INN);
                var party = response.suggestions[0].data;

                return new CompanyNameQueryResult { CompanyName = party.name.full, };
            }
            catch (Exception ex)
            {
                return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            }
        }

        private static async Task<CompanyNameQueryResult> GetCompanyNameAlt(string INN)
        {
            try
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://suggestions.dadata.ru/suggestions/api/4_1/rs/findById/party"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    request.Headers.TryAddWithoutValidation("Authorization", "Token ***");// вписать токен

                    request.Content = new StringContent("{ \"query\": \"" + INN + "\" }");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    var responseLine = await response.Content.ReadAsStringAsync(); // не придумал адекватного названия переменной
                    var deserializedResponse = JsonConvert.DeserializeObject<Rootobject>(responseLine);

                    if (deserializedResponse?.suggestions[0]?.data?.name?.full is null) return new CompanyNameQueryResult { CompanyName = null, Error = "Имя компании не найдено."};

                    return new CompanyNameQueryResult { CompanyName = deserializedResponse?.suggestions[0].data.name.full, };
                }
            }
            catch (Exception ex)
            {
                return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            }
        }
    }
}