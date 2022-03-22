using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Sol0;

namespace Sol3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacilitiApi : ControllerBase
    {
        private string login;
        private string pwd;

        public FacilitiApi(IOptions<AccountB> acc)
        {
            login = acc.Value.AccName;
            pwd = acc.Value.Password;
        }
        /// <summary>
        /// получение всех юнитов
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/all")]
        public ActionResult GetAllUnits()
        {
            return Ok();
        }
        /// <summary>
        /// получение юнита по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/{unitId}")]
        public ActionResult GetUnitById([FromRoute] string unitId)
        {
            return Ok(unitId);
        }
        /// <summary>
        /// добавление новой установки
        /// </summary>
        /// <param name="unit">Json юнита</param>
        /// <returns></returns>
        [HttpPost("unit")]
        public ActionResult AddUnit( [FromBody] Unit unit) // unit обрезать [FromRoute] string unitId,
        {
            return Ok(unit.Name);
        }
        /// <summary>
        /// редактирование установки
        /// </summary>
        /// <param name="unitId"> id юнита для изменения</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        [HttpPut("unit/{unitId}")]
        public ActionResult ReplaceUnitById([FromRoute] string unitId, [FromBody] Unit unit)
        {
            return Ok($"Меняем юнит по {unitId}, на {unit.Name}");
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpDelete("unit/{unitId}")]
        public ActionResult DeleteUnitById([FromRoute] string unitId)
        {
            return Ok($"Будет удален юнит с {unitId}");
        }
        /// <summary>
        /// получение резервуара по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("tank/{tankId}")]
        public ActionResult GetTankById([FromRoute] string tankId)
        {
            return Ok(tankId);
        }
        /// <summary>
        /// добавление нового резервуара
        /// </summary>
        /// <param name="tank">Json резервуара</param>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpPost("tank/unit/{unitId}")]
        public ActionResult AddTank([FromBody] Tank tank, [FromRoute] string unitId) // танк обрезать
        {
            return Ok($"{tank.Name}   id = {unitId}");
        }
        /// <summary>
        /// редактирование резервуара
        /// </summary>
        /// <param name="tankId">id резервуара для изменения</param>
        /// <param name="tank">Json резервуара</param>
        /// <returns></returns>
        [HttpPut("tank/{tankId}")]
        public ActionResult ReplaceTankById([FromRoute] string tankId, [FromBody] Tank tank)
        {
            return Ok($"Меняем резервуар по {tankId}, на {tank.Name}");
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="tankId">id резервуара</param>
        /// <returns></returns>
        [HttpDelete("tank/{tankId}")]
        public ActionResult DeleteTankById([FromRoute] string tankId)
        {
            return Ok($"Будет удален резервуар с {tankId}");
        }
    }
}