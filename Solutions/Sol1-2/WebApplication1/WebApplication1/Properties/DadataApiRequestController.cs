using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;


namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("[controller]")]
    
    public class DadataApiRequestController : ControllerBase
    {

        private readonly DadataApiRequester requster;
        private readonly ILogger<DadataApiRequestController> _logger;
 
        public DadataApiRequestController(DadataApiRequester requster, ILogger<DadataApiRequestController> logger)
        {
            this.requster = requster;
            _logger = logger;
        }
        [HttpGet(Name = "GetCompanyName")]
        
        public IActionResult GetName([FromBody] string INN) // тут явно есть проблемы, но надо тестить
        {
            if (Regex.IsMatch(INN, @"^\d{10}$|^\d{12}$")) return Ok(requster.GetCompanyName(INN));
            return NotFound();
        }
    }
}
