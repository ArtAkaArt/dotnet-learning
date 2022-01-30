using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DadataApiRequestController : ControllerBase
    {
        private readonly DadataLibrary requster;
        public DadataApiRequestController(DadataLibrary lib)
        {
            requster = lib;
        }
        /// <summary>
        /// Ввести ИНН компании для поиска имени
        /// </summary>
        /// <param name="INN"> ИНН сюда</param>
        /// <returns></returns>
        [HttpPost(Name = "GetCompanyName/{INN}")]
        
        public async Task<ActionResult> GetName([FromQuery] string INN)
        {
            var companyName = new CompanyNameQueryResult();
            if (Regex.IsMatch(INN, @"^\d{10}$|^\d{12}$")) 
                companyName = await requster.GetCompanyName(INN);
            if (companyName.CompanyName is not null)  
                return Ok($"ИНН {INN} принадлежит компании {companyName.CompanyName}.");
            return NotFound($"Произошла ошибка. Компания с ИНН = {INN} не найдена. {companyName.Error}");
        }
        
    }
}
