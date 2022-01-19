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
        /// <summary>
        /// Ввести ИНН компании для поиска имени
        /// </summary>
        /// <param name="INN"> ИНН сюда</param>
        /// <returns></returns>
        [HttpPost(Name = "GetCompanyName/{INN}")]
        
        public string GetName([FromQuery] string INN)
        {
            
            var requster = new DadataApiRequester();
            if (Regex.IsMatch(INN, @"^\d{10}$|^\d{12}$")) return requster.GetCompanyName(INN).Result;
            return "NotFound";
        }
    }
}
