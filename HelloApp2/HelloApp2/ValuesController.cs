using Microsoft.AspNetCore.Mvc;

namespace HelloApp2
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly string adress;
        private readonly HttpClient client;
        public ValuesController(AdressContainer cont)
        {
            adress = cont.Adress;
            client =  new HttpClient();
        }
        /// <summary>
        /// Отправить гет
        /// </summary>
        [HttpGet(Name = "GetWorld/{INN}")]
        public async Task<string> GetWorld()
        {
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions.
                try
                {
                    HttpResponseMessage response = await client.GetAsync(adress);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return $"Hello, {responseBody}";
                }
                catch (HttpRequestException e)
                {
                    return $"Message :{0} , { e.Message}";
                }
            }
        }
    }
}
