using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sol3.Profiles;
using FluentValidation;

namespace Sol3.Controllers
{
    public class UnitController : ControllerBase
    {
        public FacilityRepo repo;
        public readonly IMapper mapper;
        ILogger<UnitController> logger;
        private readonly IValidator<UnitDTO> validator;
        public UnitController(FacilityRepo repo, IMapper mapper, ILogger<UnitController> logger, IValidator<UnitDTO> validator)
        {
            this.mapper = mapper;
            this.repo = repo;
            this.logger = logger;
            this.validator = validator;
        }
        /// <summary>
        /// получение всех юнитов
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UnitDTO>))]
        public async Task<ActionResult<List<UnitDTO>>> GetAllUnits()
        {
            logger.LogInformation("Get/all: получение списка юнитов");
            return mapper.Map<List<UnitDTO>>(await repo.GetAllUnits()); //ничосиумный, даже объяснять не пришлось чокуда
        }
        /// <summary>
        /// получение юнита по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/{unitId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UnitDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UnitDTO>> GetUnitById([FromRoute] int unitId)
        {
            var result = await repo.GetUnitById(unitId);
            if (result is null)
            {
                logger.LogError($"Get: юнит с ID {unitId} не найден");
                return NotFound($"Юнит с ID {unitId} не найден");
                
            }
            logger.LogInformation($"Get: получение юнита по Id {unitId}");
            return mapper.Map<UnitDTO>(result);
        }
        /// <summary>
        /// добавление новой установки
        /// </summary>
        /// <param name="unitS">Json unitShort'а</param>
        /// <returns></returns>
        [HttpPost("unit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UnitDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UnitDTO>> AddUnit([FromBody] CreateUnitDTO unitS)
        {
            var factoryCheck = await repo.GetFactoryById(unitS.Factoryid);
            if (factoryCheck is null)
            {
                logger.LogError($"Post: Невозможно добавить установку, т.к. в базе отсутствует заданный завод");
                return NotFound("Невозможно добавить установку, т.к. в базе отсутствует заданный завод");
            }
            var validationResult = validator.Validate(mapper.Map<UnitDTO>(unitS));
            if (!validationResult.IsValid)
            {
                var logMsg = "";
                validationResult.Errors.ForEach(x => logMsg += ($"{x.ErrorMessage} "));
                logger.LogError($"Post: {logMsg}");
                return BadRequest(logMsg);
            }
            var result = await repo.AddUnit(unitS);
            logger.LogInformation($"Post: добавлен новый юнит");
            return mapper.Map<UnitDTO>(result);
        }
        /// <summary>
        /// редактирование установки
        /// </summary>
        /// <param name="unitId"> id юнита для изменения</param>
        /// <param name="unit"> UnitDto json</param>
        /// <returns></returns>
        [HttpPut("unit/{unitId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UnitDTO>> ReplaceUnitById([FromRoute] int unitId, [FromBody] UnitDTO unit)
        {
            var unitCheck = await repo.GetUnitById(unitId);
            if (unitCheck is null)
            {
                logger.LogError($"Put: юнит с ID {unitId} не найден");
                return NotFound($"Юнит с ID {unitId} не найден");
            }
            var factoryCheck = await repo.GetFactoryById(unit.Factoryid);
            if (factoryCheck is null)
            {
                logger.LogError($"Put: Невозможно добавить установку, т.к. в базе отсутствует заданный завод");
                return NotFound("Невозможно добавить установку, т.к. в базе отсутствует заданный завод");
            }
            var validationResult = validator.Validate(unit);
            if (!validationResult.IsValid)
            {
                var logMsg = "";
                validationResult.Errors.ForEach(x => logMsg += ($"{x.ErrorMessage} "));
                logger.LogError($"Put: {logMsg}");
                return BadRequest(logMsg);
            }
            var result = await repo.ReplaceUnitById(unitId, unit);
            logger.LogInformation($"Put: изменен юнит с Id {unitId}");
            return mapper.Map<UnitDTO>(result);
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpDelete("unit/{unitId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUnitById([FromRoute] int unitId)
        { 
            var unitCheck = await repo.GetUnitById(unitId);
            if (unitCheck is null)
            {
                logger.LogError($"Delete: юнит с ID {unitId} не найден");
                return NotFound($"Юнит с ID {unitId} не найден");
                
            }
            await repo.DeleteUnitById(unitId);
            logger.LogInformation($"Delete: удален юнит с Id {unitId}");
            return NoContent();
        }
    }
}