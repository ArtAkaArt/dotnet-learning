using Microsoft.AspNetCore.Mvc;
using DadataRequestLibrary;

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
        [HttpGet(Name = "GetCompanyName/{INN}")]
        
        public async Task<ActionResult> GetName([FromQuery] string INN)
        {
            if (!requster.CheckINN(INN))
                return BadRequest($"{INN} должен быть десяти- или двенадцатизначным числом");

            var companyName = await requster.GetCompanyName(INN);
            if (companyName.CompanyName is not null)  
                return Ok($"ИНН {INN} принадлежит компании {companyName.CompanyName}.");
            return NotFound($"Произошла ошибка. Компания с ИНН = {INN} не найдена. {companyName.Error}");
        }
        
    }
}
