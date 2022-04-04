using FacilityContextLib;
using AutoMapper;
using FluentValidation;

namespace Sol3.Profiles
{
    public class FacilityProfiles : Profile
    {
        public FacilityProfiles()
        {
            CreateMap<Unit, UnitDTO>();
            CreateMap<Tank, TankDTO>();
            CreateMap<CreateTankDTO, TankDTO>();
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
    public class TankDTOValidator : AbstractValidator<TankDTO>
    {
        public TankDTOValidator()
        {
            RuleFor(x => x.Name).Length(0, 50);
            RuleFor(x => x.Description).Length(0, 50);
            RuleFor(x => x.Volume).GreaterThanOrEqualTo(0).LessThanOrEqualTo(x => x.Maxvolume);
        }
    }
}
