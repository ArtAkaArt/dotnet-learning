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
    public class UnitController : ControllerBase
    {
        private readonly IMyRepo repo;
        private readonly IMapper mapper;
        ILogger<UnitController> logger;
        private readonly IValidator<UnitDTO> validator;
        public UnitController(IMyRepo repo, IMapper mapper, ILogger<UnitController> logger, IValidator<UnitDTO> validator)
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
        [HttpGet("unit/all"), Authorize(Roles = "Admin")] //тестовый метод с ограничением прав
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UnitDTO>))]
        public async Task<ActionResult<List<UnitDTO>>> GetAllUnits()
        {
            logger.LogInformation("Get/all: получение списка Units");
            return mapper.Map<List<UnitDTO>>(await repo.GetAllUnits()); //ничосиумный, даже объяснять не пришлось чокуда
        }
        /// <summary>
        /// получение юнита по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/{unitId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UnitDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UnitDTO>> GetUnitById([FromRoute] int unitId)
        {
            var result = await repo.GetUnitById(unitId);
            if (result is null)
            {
                logger.LogError($"Get: Unit с ID {unitId} не найден");
                return NotFound($"Unit с ID {unitId} не найден");
            }
            logger.LogInformation($"Get: получение Unit по Id {unitId}");
            return mapper.Map<UnitDTO>(result);
        }
        /// <summary>
        /// добавление новой установки
        /// </summary>
        /// <param name="unitS">Json unitShort'а</param>
        /// <returns></returns>
        [HttpPost("unit"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UnitDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UnitDTO>> AddUnit([FromBody] CreateUnitDTO unitS)
        {
            var methodMsg = new StringBuilder("Post: ");
            if (!await CheckAndLog(unitS, methodMsg))
                return NotFound(methodMsg.ToString());
            var result = await repo.AddUnit(unitS);
            logger.LogInformation($"Post: добавлен новый Unit");
            return mapper.Map<UnitDTO>(result);
        }
        /// <summary>
        /// редактирование установки
        /// </summary>
        /// <param name="unitId"> id юнита для изменения</param>
        /// <param name="unitDto"> UnitDto json</param>
        /// <returns></returns>
        [HttpPut("unit/{unitId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UnitDTO>> UpdateUnit([FromRoute] int unitId, [FromBody] UnitDTO unitDto)
        {
            
            var methodMsg = new StringBuilder($"Put: Id = {unitId}. ");
            var unit = await repo.GetUnitById(unitId);
            if (!await CheckAndLog(unit, unitDto, methodMsg))
                return BadRequest(methodMsg.ToString());
            var result = await repo.UpdateUnit(unit, unitDto);
            logger.LogInformation($"Put: изменен Unit с Id {unitId}");
            return mapper.Map<UnitDTO>(result);
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpDelete("unit/{unitId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUnitById([FromRoute] int unitId)
        { 
            var methodMsg = new StringBuilder($"Delete: Id = {unitId}. ");
            var unit = await repo.GetUnitById(unitId);
            if (!await NullCheckAndLog(unit, methodMsg))
                return NotFound(methodMsg.ToString());
            await repo.DeleteUnitById(unit);
            logger.LogInformation($"Delete: удален Unit с Id {unitId}");
            return NoContent();
        }

        private async Task<bool> NullCheckAndLog(Unit unit, StringBuilder msg)
        {
            if (unit is null)
            {
                msg.Append($"Unit не найден");
                logger.LogError(msg.ToString());
                return false;
            }
            return true;
        }
        private async Task<bool> FactoryCheckAndLog(int factoryId, StringBuilder msg)
        {
            var factory = await repo.GetFactoryById(factoryId);
            if (factory is null)
            {
                msg.Append("Невозможно добавить Unit, т.к. в базе отсутствует заданный Factory");
                logger.LogError(msg.ToString());
                return false;
            }
            return true;
        }
        private bool ValidationAndLog(UnitDTO uDto, StringBuilder msg)
        {
            var validationResult = validator.Validate(uDto);
            if (!validationResult.IsValid)
            {
                validationResult.Errors.ForEach(x => msg.Append($"{x.ErrorMessage} "));
                logger.LogError(msg.ToString());
                return false;
            }
            return true;
        }

        private async Task<bool> CheckAndLog(Unit unit, UnitDTO uDto, StringBuilder msg)
        {
            if (!await NullCheckAndLog(unit, msg))
                return false;
            if (uDto.Id != unit.Id)
            {
                msg.Append($"Заданный Id {unit.Id} не соответствует Id в DTO {uDto.Id}");
                logger.LogError(msg.ToString());
                return false;
            }
            return await CheckAndLog(uDto, msg);
        }
        private async Task<bool> CheckAndLog(UnitDTO uDto, StringBuilder msg)
        {
            if (!await FactoryCheckAndLog(uDto.Factoryid, msg))
                return false;
            return ValidationAndLog(uDto, msg);
        }
        private async Task<bool> CheckAndLog(CreateUnitDTO unitS, StringBuilder msg)
        {
            if (!await FactoryCheckAndLog(unitS.Factoryid, msg))
                return false;
            var uDto = mapper.Map<UnitDTO>(unitS);
            return ValidationAndLog(uDto, msg);
        }
    }
}