using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HelloApp2
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly string adress;
        static readonly HttpClient client = new HttpClient();
        public HelloController(IOptions<AdressContainer>  cont)
        {
            adress = cont.Value.Adress;
        }
        /// <summary>
        /// Гет запрос получает, ответ HelloApp
        /// </summary>
        [HttpGet(Name = "GetWorld/{INN}")]
        public async Task<ActionResult> GetWorld()
        {
            try
            {
                var response = await client.GetAsync(adress);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return Ok($"Hello, {responseBody}");
            }
            catch (Exception e)
            {
                return NotFound($"Message :{0} , { e.Message}");
            }
        }
    }
}
