using Dadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DadataRequestLibrary
{
    public class DadataLibrary
    {
        private readonly string token;
        private ILogger<DadataLibrary> logger;
        public DadataLibrary(IOptions<DadataConfiguration> configs, ILogger<DadataLibrary> logger)
        {
            token = configs.Value.Token;
            this.logger = logger;
        }
        public async Task<CompanyNameQueryResult> GetCompanyName(string INN)
        {
            try
            {
                var api = new SuggestClientAsync(token);
                var apiResponse = await api.FindParty(INN);
                var party = apiResponse?.suggestions[0]?.data;
                if (party == null || apiResponse?.suggestions.Count < 1)
                {
                    var response = new CompanyNameQueryResult { CompanyName = null, Error = "Ошибка получений данных из ответа DadataApi" };
                    logger.LogError($"Ошибка получения имени компании по ИНН {INN}. {response.Error}");
                    return response;
                }
                return new CompanyNameQueryResult { CompanyName = party.name.full, };
            }
            catch (Exception ex)
            {
                logger.LogError($"Ошибка получения имени компании по ИНН {INN}. { ex.Message}");
                return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            }
        }
    }
}
