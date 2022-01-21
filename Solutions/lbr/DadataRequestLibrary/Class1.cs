﻿using Dadata;

namespace DadataRequestLibrary
{
    public class DadataLib
    {
        string token { get; set; }
        public DadataLib(string token)
        {
            this.token = token;
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
                return new CompanyNameQueryResult { CompanyName = null, Error = ex.Message };
            }
        }
    }
}
