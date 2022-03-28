using Microsoft.AspNetCore.Mvc;

namespace Sol3.Controllers
{
    public class UnitsApi : ControllerBase
    {
        public FacilityRepo repo;

        public UnitsApi(FacilityRepo repo)
        {
            this.repo = repo;
        }
        /// <summary>
        /// получение всех юнитов
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/all")]
        public async Task<ActionResult> GetAllUnits()
        {
            return Ok(await repo.GetAllUnits());
        }
        /// <summary>
        /// получение юнита по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/{unitId}")]
        public async Task<ActionResult<UnitDTO>> GetUnitById([FromRoute] int unitId)
        {
            var unit = await repo.GetUnitById(unitId);
            if (unit is not null)
                return new UnitDTO
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    Description = unit.Description,
                    Factoryid = unit.FactoryId
                };
            else return BadRequest($"Юнит с ID {unitId} не найден");
        }
        /// <summary>
        /// добавление новой установки
        /// </summary>
        /// <param name="unit">Json юнита</param>
        /// <returns></returns>
        [HttpPost("unit")]
        public async Task<ActionResult<UnitDTO>> AddUnit([FromBody] UnitShort unit)
        {
            var factoryCheck = await repo.GetFactoryById(unit.Factoryid);
            if (factoryCheck is not null)
            {
                var unit2 = await repo.AddUnit(unit);

                return new UnitDTO {Id = unit2.Id, 
                    Name = unit2.Name,
                    Description = unit2.Description, 
                    Factoryid = unit2.FactoryId };
            }
            return BadRequest();
        }
        /// <summary>
        /// редактирование установки
        /// </summary>
        /// <param name="unitId"> id юнита для изменения</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        [HttpPut("unit/{unitId}")]
        public async Task<ActionResult> ReplaceUnitById([FromRoute] int unitId, [FromBody] UnitDTO unit)
        {
            var unitCheck = await repo.GetUnitById(unitId);
            if (unitCheck is not null)
            {
                await repo.ReplaceUnitById(unitId, unit);
                return Ok();
            }
            else return BadRequest($"Юнит с ID {unitId} не найден");
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpDelete("unit/{unitId}")]
        public async Task<ActionResult> DeleteUnitById([FromRoute] int unitId)
        { 
            var unitCheck = await repo.GetUnitById(unitId);
            if (unitCheck is not null)
            {
                await repo.DeleteUnitById(unitId);
                return Ok();
            }
            else return BadRequest($"Юнит с ID {unitId} не найден");
        }
    }
    public class UnitShort
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Factoryid { get; set; }
    }
    public class UnitDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Factoryid { get; set; }
    }
}