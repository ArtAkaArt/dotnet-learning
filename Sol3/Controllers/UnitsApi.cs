using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sol3.Profiles;

namespace Sol3.Controllers
{
    public class UnitsApi : ControllerBase
    {
        public FacilityRepo repo;
        public readonly IMapper mapper;

        public UnitsApi(FacilityRepo repo, IMapper mapper)
        {
            this.mapper = mapper;
            this.repo = repo;
        }
        /// <summary>
        /// получение всех юнитов
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UnitDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<UnitDTO>>> GetAllUnits()
        {
            return mapper.Map<List<UnitDTO>>(await repo.GetAllUnits()); //ничосиумный, даже объяснять не пришлось чокуда
        }
        /// <summary>
        /// получение юнита по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/{unitId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UnitDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UnitDTO>> GetUnitById([FromRoute] int unitId)
        {
            var unit = await repo.GetUnitById(unitId);
            if (unit is not null)
                return mapper.Map<UnitDTO>(unit);
            else return BadRequest($"Юнит с ID {unitId} не найден");
        }
        /// <summary>
        /// добавление новой установки
        /// </summary>
        /// <param name="unitS">Json unitShort'а</param>
        /// <returns></returns>
        [HttpPost("unit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UnitDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UnitDTO>> AddUnit([FromBody] UnitShort unitS)
        {
            var factoryCheck = await repo.GetFactoryById(unitS.Factoryid);
            if (factoryCheck is not null)
            {
                var unitD = await repo.AddUnit(unitS);
                return mapper.Map<UnitDTO>(unitD);
            }
            return BadRequest();
        }
        /// <summary>
        /// редактирование установки
        /// </summary>
        /// <param name="unitId"> id юнита для изменения</param>
        /// <param name="unit"> UnitDto json</param>
        /// <returns></returns>
        [HttpPut("unit/{unitId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ReplaceUnitById([FromRoute] int unitId, [FromBody] UnitDTO unit)
        {
            var unitCheck = await repo.GetUnitById(unitId);
            if (unitCheck is not null)
            {
                await repo.ReplaceUnitById(unitId, unit);
                return NoContent();
            }
            else return BadRequest($"Юнит с ID {unitId} не найден");
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpDelete("unit/{unitId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteUnitById([FromRoute] int unitId)
        { 
            var unitCheck = await repo.GetUnitById(unitId);
            if (unitCheck is not null)
            {
                await repo.DeleteUnitById(unitId);
                return NoContent();
            }
            else return BadRequest($"Юнит с ID {unitId} не найден");
        }
    }
}