using Dadata;

namespace WebApplication1
{
    public class DadataLibrary
    {
        private readonly string token;
        private ILogger<DadataLibrary> logger;

        public DadataLibrary(TokenContainer tokenCont, ILogger<DadataLibrary> logger)
        {
            token = tokenCont.GetToken();
            this.logger = logger;
        }
        public async Task<CompanyNameQueryResult> GetCompanyName(string INN)
        {
            try
            {
                var api = new SuggestClientAsync(token);
                var response = await api.FindParty(INN);
                var party = response.suggestions[0].data;
                return new CompanyNameQueryResult { CompanyName = party.name.full, };
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Ошибка получения имени компании по ИНН {INN}. { ex.Message}");
                return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            }
        }
    }
}
