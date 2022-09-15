using Microsoft.AspNetCore.Mvc;
using FacilityContextLib;
using Sol3.Profiles;

namespace Sol3.Controllers
{
    public class ORMTestController : ControllerBase
    {
        CatDbContext catDbContext;
        public ORMTestController(CatDbContext context)
        {
            catDbContext = context;
        }
        [HttpGet("Test/Test1")]
        public async Task<IReadOnlyCollection<UnitDTO>> Test20()
        {
            using (catDbContext)
            {
                await catDbContext.OpenAsync();
                return await catDbContext.Test<UnitDTO>("Select * From \"Units\"");
            }
        }
    }
}
