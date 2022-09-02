using FacilityContextLib;
using AutoMapper;
using CustomAttributes;

namespace Sol3.Profiles
{
    public class FacilityProfiles : Profile
    {
        public FacilityProfiles()
        {
            CreateMap<Unit, UnitDTO>();
            CreateMap<Tank, TankDTO>();
            CreateMap<CreateTankDTO, TankDTO>();
            CreateMap<CreateUnitDTO, UnitDTO>();
        }
    }
    public class CreateUnitDTO
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
    public class CreateTankDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [AllowedRange(0,1000)]
        public int Volume { get; set; }
        [AllowedRange(200, 1000)]
        public int Maxvolume { get; set; }
    }
    public class TankDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [AllowedRange(0, 1000)]
        public int Volume { get; set; }
        [AllowedRange(200, 1000)]
        public int Maxvolume { get; set; }
        public int Unitid { get; set; }
    }
}
