using Dadata;
namespace WebApplication1

{

    public class DadataApiRequester
    {
        public string token { get; set; }

        public DadataApiRequester()
        {
            token = "***"; // вписать токен
        }

        public async Task<string> GetCompanyName(string INN)
        {
            var api = new SuggestClientAsync(token);
            var response = await api.FindParty(INN);
            var party = response.suggestions[0].data;
            return party.name.full;

        }
    }
}
