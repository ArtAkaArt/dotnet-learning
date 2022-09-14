using Microsoft.AspNetCore.Mvc;
using FacilityContextLib;
using Sol3.Profiles;

namespace Sol3.Controllers
{
    public class HomeController : ControllerBase
    {
        CatDbContext catDbContext;
        public HomeController(CatDbContext context)
        {
            catDbContext = context;
        }
        [HttpGet("Test/Test1")]
        public async Task<IReadOnlyCollection<UnitDTO>> Test20()
        {
            return await catDbContext.Test<UnitDTO>("Select * From \"Units\"");
        }
    }
}
