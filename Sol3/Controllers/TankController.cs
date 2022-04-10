using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sol3.Profiles;
using FluentValidation;
using System.Text;

namespace Sol3.Controllers
{
    public class TankController : ControllerBase
    {
        private FacilityRepo repo;
        private readonly IMapper mapper;
        private readonly IValidator<TankDTO> validator;
        private readonly ILogger<TankController> logger;
        public TankController(FacilityRepo repo, IMapper mapper , IValidator<TankDTO> validator, ILogger<TankController> logger)
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
        [HttpGet("tank/{tankId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TankDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TankDTO>> GetTankById([FromRoute] int tankId)
        {
            var result = await repo.GetTankById(tankId);
            if (result is null)
            {
                logger.LogError($"Get: Tank с Id {tankId} не найден");
                return NotFound($"Tank с Id {tankId} не найден");
                
            }
            logger.LogInformation($"Get: получена информация по Tank c Id {tankId}");
            return mapper.Map<TankDTO>(result);
        }
        /// <summary>
        /// добавление нового резервуара
        /// </summary>
        /// <param name="tankS">Short Json резервуара</param>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpPost("tank/unit/{unitId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TankDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TankDTO>> AddTank([FromBody] CreateTankDTO tankS, [FromRoute] int unitId)
        {

            if (tankS is null)
            {
                logger.LogError($"Post: не введены параметры Tank");
                return BadRequest($"Не введены параметры Tank");
            }
            var tank = mapper.Map<TankDTO>(tankS);

            var validationResult = validator.Validate(tank);

            if (!validationResult.IsValid)
            {
                var logMsg = new StringBuilder();
                validationResult.Errors.ForEach(x => logMsg.Append($"{x.ErrorMessage} "));
                logger.LogError($"Post: {logMsg}");
                return BadRequest(logMsg);
            }
            var unit = await repo.GetUnitById(unitId);
            if (unit is null)
            {
                logger.LogError($"Post: Unit с Id {unitId} не существует");
                return NotFound($"Добавление Tank: Unit с Id {unitId} не существует");
                
            }
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
        [HttpPut("tank/{tankId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TankDTO>> UpdateTank([FromRoute] int tankId, [FromBody] TankDTO tank)
        {
            if (tank is null)
            {
                logger.LogError($"Put: введенный пользователем Tank is null");
                return BadRequest($"Не введены параметры Tank");
            }
            var validationResult = validator.Validate(tank);

            if (!validationResult.IsValid)
            {
                var logMsg = new StringBuilder();
                validationResult.Errors.ForEach(x => logMsg.Append($"{x.ErrorMessage} "));
                logger.LogError($"Put: {logMsg}");

                return BadRequest(logMsg);
            }
            var tankCheck = await repo.GetTankById(tankId);
            if (tankCheck is null)
            {
                logger.LogError($"Put: Tank с Id {tankId} не найден");
                return NotFound($"Tank с Id {tankId} не найден");
                
            }
            var result = await repo.UpdateTank(tankId, tank);
            logger.LogInformation($"Put: изменен Tank c Id {tankId}");
            return mapper.Map<TankDTO>(result);
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="tankId">id резервуара</param>
        /// <returns></returns>
        [HttpDelete("tank/{tankId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTankById([FromRoute] int tankId)
        {
            var tankCheck = await repo.GetTankById(tankId);
            if (tankCheck is null)
            {
                logger.LogError($"Delete: Tank с Id {tankId} не найден");
                return NotFound($"Tank с Id {tankId} не найден");
                
            }
            await repo.DeleteTankById(tankId);
            logger.LogInformation($"Delete: удален Tank с Id {tankId}");
            return NoContent();
        }
    }
}
