using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sol3.Profiles;
using Sol3.Repos;
using FluentValidation;
using System.Text;
using FacilityContextLib;
using Microsoft.AspNetCore.Authorization;

namespace Sol3.Controllers
{
    public class TankController : ControllerBase
    {
        private IMyRepo repo;
        private readonly IMapper mapper;
        private readonly IValidator<TankDTO> validator;
        private readonly ILogger<TankController> logger;
        public TankController(IMyRepo repo, IMapper mapper, IValidator<TankDTO> validator, ILogger<TankController> logger)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.validator = validator;
            this.logger = logger;
        }
        /// <summary>
        /// получение резервуара по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("tank/{tankId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TankDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TankDTO>> GetTankById([FromRoute] int tankId)
        {
            var logMsg = new StringBuilder("Get: ");
            var tankCheck = await repo.GetTankById(tankId);
            if (!DBTankCheck(tankCheck, logMsg, tankId))
                return BadRequest(logMsg.ToString());

            logger.LogInformation($"Get: получена информация по Tank c Id {tankId}");
            return mapper.Map<TankDTO>(tankCheck);
        }
        /// <summary>
        /// добавление нового резервуара
        /// </summary>
        /// <param name="tankS">Short Json резервуара</param>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpPost("tank/unit/{unitId}")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TankDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TankDTO>> AddTank([FromBody] CreateTankDTO tankS, [FromRoute] int unitId)
        {
            //Проверка аттрибутов
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                return BadRequest($"Модель не прошла валидацию {message}");
            }
            var logMsg = new StringBuilder("Post: ");
            if (tankS is null)
            {
                logger.LogError($"Post: не введены параметры Tank");
                return BadRequest($"Не введены параметры Tank");
            }
            var tank = mapper.Map<TankDTO>(tankS);

            
            if (!ValidationCheck(tank, logMsg))
                return BadRequest(logMsg.ToString());

            if (!await DBUnitCheck(unitId, logMsg))
                return NotFound(logMsg.ToString());

            var result = await repo.AddTank(unitId, tank);
            logger.LogInformation($"Post: добавлен новый Tank");
            return mapper.Map<TankDTO>(result);
        }
        /// <summary>
        /// редактирование резервуара
        /// </summary>
        /// <param name="tankId">id резервуара для изменения</param>
        /// <param name="tank">DTO Json резервуара</param>
        /// <returns></returns>
        [HttpPut("tank/{tankId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TankDTO>> UpdateTank([FromRoute] int tankId, [FromBody] TankDTO tank)
        {
            var logMsg = new StringBuilder("Put: ");
            if (tank is null)
            {
                logger.LogError($"Put: введенный пользователем Tank is null");
                return BadRequest($"Не введены параметры Tank");
            }
            
            if (!ValidationCheck(tank, logMsg))
                return BadRequest(logMsg.ToString());

            if (tank.Id != tankId)
            {
                logger.LogError($"Put: Заданный Id {tankId} не соответствует переданному Id в DTO {tank.Id}");
                return BadRequest($"Заданный Id {tankId} не соответствует переданному Id в DTO {tank.Id}");
            }

            var tankCheck = await repo.GetTankById(tankId);
            if (!DBTankCheck(tankCheck, logMsg, tankId))
                return BadRequest(logMsg.ToString());

            if (!await DBUnitCheck(tank.Unitid, logMsg))
                return NotFound(logMsg.ToString());

            var result = await repo.UpdateTank(tankCheck, tank);
            logger.LogInformation($"Put: изменен Tank c Id {tankId}");
            return mapper.Map<TankDTO>(result);
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="tankId">id резервуара</param>
        /// <returns></returns>
        [HttpDelete("tank/{tankId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTankById([FromRoute] int tankId)
        {
            var logMsg = new StringBuilder("Delete: ");
            var tankCheck = await repo.GetTankById(tankId);
            if (!DBTankCheck(tankCheck, logMsg, tankId))
                return BadRequest(logMsg.ToString());
            await repo.DeleteTankById(tankCheck);
            logger.LogInformation($"Delete: удален Tank с Id {tankId}");
            return NoContent();
        }

        bool ValidationCheck(TankDTO tank, StringBuilder errMsg)
        {
            var validationResult = validator.Validate(tank);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => errMsg.Append($"{x.ErrorMessage} "));
                logger.LogError($"Put: {errMsg}");
                return false;
            }
            return true;
        }
        bool DBTankCheck(Tank tankCheck, StringBuilder errMsg, int tankId)
        {
            if (tankCheck is null)
            {
                errMsg.Append($"Tank c Id {tankId} не найден");
                logger.LogError(errMsg.ToString());
                return false;
            }
            return true;
        }
        async Task<bool> DBUnitCheck(int unitId, StringBuilder errMsg)
        {
            var unit = await repo.GetUnitById(unitId);
            if (unit is null)
            {
                errMsg.Append($"Put: Unit с Id {unitId} не существует");
                logger.LogError(errMsg.ToString());
                return false;
            }
            return true;
        }
    }
}
