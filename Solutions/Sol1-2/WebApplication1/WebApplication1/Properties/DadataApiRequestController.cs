using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using DadataRequestLibrary;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DadataApiRequestController : ControllerBase
    {
        private readonly DadataLibrary requster;
        private readonly ILogger<DadataApiRequestController> _logger;
        private readonly TokenContainer _container;

        public DadataApiRequestController(ILogger<DadataApiRequestController> logger, TokenContainer container)
        {
            _logger = logger;
            _container = container;
            requster = new DadataLibrary(_container.GetToken()); // так и не понял как из опций передать значение конструктору в Program.cs
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
            if (Regex.IsMatch(INN, @"^\d{10}$|^\d{12}$")) companyName = await requster.GetCompanyName(INN);
            if (companyName.CompanyName is not null)  return Ok($"ИНН {INN} принадлежит компании {companyName.CompanyName}.");
            _logger.LogInformation("Ошибка получения имени компании "+companyName.Error);
            /* не уверен что это будет так работать, но лучше пока ничего не придумал, добавить логгер в DadataLibrary так и не смог */
            return NotFound($"Произошла ошибка. Компания с ИНН = {INN} не найдена. {companyName.Error}");
        }
        
    }
}
