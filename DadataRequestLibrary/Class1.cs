using Dadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace DadataRequestLibrary
{
    public class DadataLibrary
    {
        private readonly string token;
        private readonly ILogger<DadataLibrary> logger;

        public DadataLibrary(IOptions<DadataConfiguration> configs, ILogger<DadataLibrary> logger)
        {
            token = configs.Value.Token;
            this.logger = logger;
        }
        public async Task<CompanyNameQueryResult> GetCompanyName(string INN)
        {
            Dadata.Model.SuggestResponse<Dadata.Model.Party> apiResponse;
            var api = new SuggestClientAsync(token);
            try
            {
                apiResponse = await api.FindParty(INN);
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка обращения к DadataApi. { ex.Message}");
                return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            }
            if (apiResponse?.suggestions == null || !apiResponse.suggestions.Any()) //это какой-то странный массив, не дает Length
            {
                var response = new CompanyNameQueryResult { CompanyName = null, Error = "Ошибка получений данных из ответа DadataApi" };
                logger.LogError($"Ошибка получения имени компании по ИНН {INN}. {response.Error}");
                return response;
            }
            var party = apiResponse.suggestions[0].data;
            return new CompanyNameQueryResult { CompanyName = party.name.full, };
        }
        public bool CheckINN(string INN)
        {
            return Regex.IsMatch(INN, @"^\d{10}$|^\d{12}$");
        }
    }
}
