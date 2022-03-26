using Microsoft.AspNetCore.Mvc;
using FacilityContextLib;

namespace Sol3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacilitiApi : ControllerBase
    {
        public FacilityRepo repo;

        public FacilitiApi(FacilityRepo repo)
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
            try 
            {
                return Ok(await repo.GetAllUnits());
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// получение юнита по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("unit/{unitId}")]
        public async Task<ActionResult> GetUnitById([FromRoute] int unitId)
        {
            try
            {
                var unit = await repo.GetUnitById(unitId);
                if (unit is not null)
                    return Ok(unit);
                return BadRequest("Юнит не найден");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// добавление новой установки
        /// </summary>
        /// <param name="unit">Json юнита</param>
        /// <returns></returns>
        [HttpPost("unit")]
        public async Task<ActionResult> AddUnit([FromBody] UnitShort unit)
        {
            try
            {
                await repo.AddUnit(unit);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
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
            try 
            { 
                await repo.ReplaceUnitById(unitId, unit);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpDelete("unit/{unitId}")]
        public async Task<ActionResult> DeleteUnitById([FromRoute] int unitId)
        {
            try
            {
                await repo.DeleteUnitById(unitId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        /// <summary>
        /// получение резервуара по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("tank/{tankId}")]
        public async Task<ActionResult> GetTankById([FromRoute] int tankId)
        {
            try
            {
                var tank = await repo.GetUnitById(tankId);
                if (tank is not null)
                    return Ok(tank);
                return BadRequest("Резервуар не найден");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// добавление нового резервуара
        /// </summary>
        /// <param name="tank">Json резервуара</param>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpPost("tank/unit/{unitId}")]
        public async Task<ActionResult> AddTank([FromBody] TankShort tank, [FromRoute] int unitId) // танк обрезать
        {
            try
            {
                await repo.AddTank(unitId, tank);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        /// <summary>
        /// редактирование резервуара
        /// </summary>
        /// <param name="tankId">id резервуара для изменения</param>
        /// <param name="tank">Json резервуара</param>
        /// <returns></returns>
        [HttpPut("tank/{tankId}")]
        public async Task<ActionResult> ReplaceTankById([FromRoute] int tankId, [FromBody] TankDTO tank)
        {
            try
            {
                await repo.ReplaceTankById(tankId, tank);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="tankId">id резервуара</param>
        /// <returns></returns>
        [HttpDelete("tank/{tankId}")]
        public async Task<ActionResult> DeleteTankById([FromRoute] int tankId)
        {
            try 
            {
            await repo.DeleteTankById(tankId);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
    public class UnitShort
    {
        public string Name { get; set; } = null!;
        public int Factoryid { get; set; }
    }
    public class UnitDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Factoryid { get; set; }
    }
    public partial class TankShort
    {
        public string Name { get; set; } = null!;
        public int Volume { get; set; }
        public int Maxvolume { get; set; }
        public int Unitid { get; set; }
    }
    public partial class TankDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Volume { get; set; }
        public int Maxvolume { get; set; }
        public int Unitid { get; set; }
    }
}