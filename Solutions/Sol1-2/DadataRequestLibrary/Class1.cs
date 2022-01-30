using Dadata;
namespace DadataRequestLibrary
{
    public class DadataLibrary
    {
        private readonly string token;
        public DadataLibrary(string token)
        {
            this.token = token;
        }
        public async Task<CompanyNameQueryResult> GetCompanyName(string INN)
        {
            try
            {
                var api = new SuggestClientAsync(token);
                var response = await api.FindParty(INN);
                var party = response?.suggestions[0]?.data;
                if (party is not null)
                    return new CompanyNameQueryResult { CompanyName = party.name.full, };
                return new CompanyNameQueryResult { CompanyName = null, Error = "Ошибка получения данных, от Dadata API." };
            }
            catch (Exception ex)
            {
                return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            }
        }
    }
}
