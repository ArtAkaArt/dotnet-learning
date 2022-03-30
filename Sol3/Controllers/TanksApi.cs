using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sol3.Profiles;

namespace Sol3.Controllers
{
    public class TanksApi : ControllerBase
    {
        public FacilityRepo repo;
        public readonly IMapper mapper;
        public TanksApi(FacilityRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        /// <summary>
        /// получение резервуара по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("tank/{tankId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TankDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TankDTO>> GetTankById([FromRoute] int tankId)
        {
            var tank = await repo.GetTankById(tankId);
            if (tank is not null)
                return mapper.Map<TankDTO>(tank);
            else return BadRequest($"Танк с Id {tankId} не найден");
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
        public async Task<ActionResult<TankDTO>> AddTank([FromBody] TankShort tankS, [FromRoute] int unitId)
        {
            var unit = await repo.GetUnitById(unitId);
            if (unit is not null)
            {
                var tankD = await repo.AddTank(unitId, tankS);
                return mapper.Map<TankDTO>(tankD);
            }
            else return BadRequest($"Юнита с Id {unitId} не существует");
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
        public async Task<ActionResult> ReplaceTankById([FromRoute] int tankId, [FromBody] TankDTO tank)
        {
            var tankCheck = await repo.GetTankById(tankId);
            if (tankCheck is not null)
            {
                await repo.ReplaceTankById(tankId, tank);
                return NoContent();
            }
            else return BadRequest($"Танк с Id {tankId} не найден");
        }
        /// <summary>
        /// удаление установки со всеми резервуарами
        /// </summary>
        /// <param name="tankId">id резервуара</param>
        /// <returns></returns>
        [HttpDelete("tank/{tankId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteTankById([FromRoute] int tankId)
        {
            var tankCheck = await repo.GetTankById(tankId);
            if (tankCheck is not null)
            {
                await repo.DeleteTankById(tankId);
                return NoContent();
            }
            else return BadRequest($"Танк с Id {tankId} не найден");
        }

    }
}
