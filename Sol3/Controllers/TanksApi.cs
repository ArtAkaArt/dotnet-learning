using Microsoft.AspNetCore.Mvc;

namespace Sol3.Controllers
{
    public class TanksApi : ControllerBase
    {
        public FacilityRepo repo;
        public TanksApi(FacilityRepo repo)
        {
            this.repo = repo;
        }
        /// <summary>
        /// получение резервуара по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("tank/{tankId}")]
        public async Task<ActionResult<TankDTO>> GetTankById([FromRoute] int tankId)
        {
            var tank = await repo.GetTankById(tankId);
            if (tank is not null)
                return new TankDTO
                {
                    Id = tank.Id,
                    Name = tank.Name,
                    Description = tank.Description,
                    Volume = tank.Volume,
                    Maxvolume = tank.Maxvolume,
                    Unitid = tank.UnitId
                };
            else return BadRequest($"Танк с Id {tankId} не найден");
        }
        /// <summary>
        /// добавление нового резервуара
        /// </summary>
        /// <param name="tank">Json резервуара</param>
        /// <param name="unitId">id юнита</param>
        /// <returns></returns>
        [HttpPost("tank/unit/{unitId}")]
        public async Task<ActionResult<TankDTO>> AddTank([FromBody] TankShort tank, [FromRoute] int unitId) // танк обрезать
        {
            var unit = await repo.GetUnitById(unitId);
            if (unit is not null)
            {
                var tank2 = await repo.AddTank(unitId, tank);
                return new TankDTO
                {
                    Id = tank2.Id,
                    Name = tank2.Name,
                    Description = tank2.Description,
                    Volume = tank2.Volume,
                    Maxvolume = tank2.Maxvolume,
                    Unitid = tank2.UnitId
                };
            }
            else return BadRequest($"Юнита с Id {unitId} не существует");
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
            var tankCheck = await repo.GetTankById(tankId);
            if (tankCheck is not null)
            {
                await repo.ReplaceTankById(tankId, tank);
                return Ok();
            }
            else return BadRequest($"Танк с Id {tankId} не найден");
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="tankId">id резервуара</param>
        /// <returns></returns>
        [HttpDelete("tank/{tankId}")]
        public async Task<ActionResult> DeleteTankById([FromRoute] int tankId)
        {
            var tankCheck = await repo.GetTankById(tankId);
            if (tankCheck is not null)
            {
                await repo.DeleteTankById(tankId);
                return Ok();
            }
            else return BadRequest($"Танк с Id {tankId} не найден");
        }
        public class TankShort
        {
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public int Volume { get; set; }
            public int Maxvolume { get; set; }
        }
        public class TankDTO
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public int Volume { get; set; }
            public int Maxvolume { get; set; }
            public int Unitid { get; set; }
        }
    }
}
