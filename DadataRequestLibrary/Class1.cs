using Dadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        public DadataLibrary(string configs, ILogger<DadataLibrary> logger)
        {
            token = configs;
            this.logger = logger;
        }
        public async Task<CompanyNameQueryResult> GetCompanyName(string INN)
        {
            //try
            //{
                var api = new SuggestClientAsync(token);
                var apiResponse = await api.FindParty(INN);
                if (apiResponse == null || apiResponse?.suggestions.Count < 1)
                {
                    var response = new CompanyNameQueryResult { CompanyName = null, Error = "Ошибка получений данных из ответа DadataApi" };
                    logger.LogError($"Ошибка получения имени компании по ИНН {INN}. {response.Error}");
                    return response;
                }
            var party = apiResponse.suggestions[0].data; // да, вылет тут происходил, при плохом запросе; от API генерируется объект apiResponse с пустым suggestion
            return new CompanyNameQueryResult { CompanyName = party.name.full, };
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError($"Ошибка получения имени компании по ИНН {INN}. { ex.Message}");
            //    return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            //}
        }
    }
}
