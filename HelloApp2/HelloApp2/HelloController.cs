using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HelloApp2
{
    [Route("[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly string address;
        static readonly HttpClient client = new HttpClient();
        public HelloController(IOptions<AdressContainer>  cont)
        {
            address = cont.Value.Address;

        }
        /// <summary>
        /// Гет запрос получает, ответ HelloApp
        /// </summary>
        [HttpGet(Name = "GetWorld")]
        public async Task<ActionResult> GetWorld()
        {
            try
            {
                var response = await client.GetAsync(address);
                response.EnsureSuccessStatusCode();
                var responseText = await response.Content.ReadAsStringAsync();
                return Ok($"Hello {responseText}!");
            }
            catch (Exception e)
            {
                return NotFound($"Message :{0} , { e.Message}");
            }
        }
    }
}
