using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Sol0;

namespace Sol3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnitApi : ControllerBase
    {
        private string login;
        private string pwd;

        public UnitApi(IOptions<AccountB> acc)
        {
            login = acc.Value.AccName;
            pwd = acc.Value.Password;
        }
        /// <summary>
        /// получение всех юнитов
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult GetAll()
        {
            return Ok();
        }
        /// <summary>
        /// получение юнита по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute] string id)
        {
            return Ok(id);
        }
        /// <summary>
        /// добавление новой установки
        /// </summary>
        /// <param name="unit">Json юнита</param>
        /// <returns></returns>
        [HttpPost(Name = "Add")]
        public ActionResult AddUnit([FromBody] Unit unit)
        {
            return Ok(unit.Name);
        }
        /// <summary>
        /// редактирование установки
        /// </summary>
        /// <param name="id"> id юнита для изменения</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult ReplaceById([FromRoute] string id, [FromBody] Unit unit)
        {
            return Ok($"Меняем юнит по {id}, на {unit.Name}");
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="id">id юнита</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteById([FromRoute] string id)
        {
            return Ok($"Будет удален юнит с {id}");
        }
    }
}