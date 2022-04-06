using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sol3.Profiles;
using FluentValidation;

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
            if (result is not null)
            {
                logger.LogInformation($"Get: получена информация по резервуару c Id {tankId}");
                return mapper.Map<TankDTO>(result);
            }
            else
            {
                logger.LogError($"Get: резервуар с Id {tankId} не найден");
                return NotFound($"Резервуар с Id {tankId} не найден");
            }
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
                logger.LogError($"Post: не введены параметры Tank'а");
                return BadRequest($"Не введены параметры Tank'а");
            }
            var tank = mapper.Map<TankDTO>(tankS);

            if (!validator.Validate(tank).IsValid)
            {
                logger.LogError($"Post: значение Volume {tank.Volume} выходит за допустимый предел", tank.Volume);
                return BadRequest($"Значение Volume {tank.Volume} выходит за допустимый предел");
            }
            var unit = await repo.GetUnitById(unitId);
            if (unit is not null)
            {
                var result = await repo.AddTank(unitId, tank);
                logger.LogInformation($"Post: добавлен новый резервуар {tank}");
                return mapper.Map<TankDTO>(result);
            }
            else
            {
                logger.LogError($"Post: юнита с Id {unitId} не существует");
                return NotFound($"Добавление резервуара: юнита с Id {unitId} не существует");
            }
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
        public async Task<ActionResult<TankDTO>> ReplaceTankById([FromRoute] int tankId, [FromBody] TankDTO tank)
        {
            if (tank is null)
            {
                logger.LogError($"Put: введенный пользователем резервуар is null");
                return BadRequest($"Не введены параметры резервуара");
            }

            if (!validator.Validate(tank).IsValid)
            {
                logger.LogError($"Put: значение Volume {tank.Volume} выходит за допустимый предел", tank.Volume);
                return BadRequest($"Значение Volume {tank.Volume} выходит за допустимый предел");
            }
            var tankCheck = await repo.GetTankById(tankId);
            if (tankCheck is not null)
            {
                var result = await repo.ReplaceTankById(tankId, tank);
                logger.LogInformation($"Put: изменен резервуар {tank}");
                return mapper.Map<TankDTO>(result);
            }
            else
            {
                logger.LogError($"Put: резервуар с Id {tankId} не найден");
                return NotFound($"резервуар с Id {tankId} не найден");
            }
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
            if (tankCheck is not null)
            {
                await repo.DeleteTankById(tankId);
                logger.LogInformation($"Delete: удален резервуар с Id {tankId}");
                return NoContent();
            }
            else
            {
                logger.LogError($"Delete: резервуар с Id {tankId} не найден");
                return NotFound($"Резервуар с Id {tankId} не найден");
            }
        }
    }
}
