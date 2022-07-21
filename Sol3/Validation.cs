using FluentValidation;
using Attributes;

namespace Sol3.Profiles
{
    public class TankDTOValidator : AbstractValidator<TankDTO>
    {
        public TankDTOValidator()
        {
            var tank = typeof(FacilityContextLib.Tank);
            var volume = tank.GetProperty("Volume");
            var maxvolume = tank.GetProperty("Maxvolume");
            var volumeAttr = (AllowedRangeAttribute?)Attribute.GetCustomAttribute(volume!, typeof(AllowedRangeAttribute));
            var maxVolumeAttr = (AllowedRangeAttribute?)Attribute.GetCustomAttribute(maxvolume!, typeof(AllowedRangeAttribute));
            RuleFor(x => x.Name).Length(3, 50).WithMessage($"{nameof(TankDTO.Name)} должен быть от 3 до 50 символов.");
            RuleFor(x => x.Description).Length(0, 50).WithMessage($"{nameof(TankDTO.Description)} превышает 50 символов.");
            RuleFor(x => x.Volume).GreaterThanOrEqualTo(0).LessThanOrEqualTo(x => x.Maxvolume).WithMessage($"{nameof(TankDTO.Volume)} превышает допустимый предел.");
            RuleFor(x => x.Volume).GreaterThanOrEqualTo(volumeAttr!.Min).LessThanOrEqualTo(volumeAttr!.Max).WithMessage($"{nameof(TankDTO.Volume)} превышает допустимый предел.");
            RuleFor(x => x.Maxvolume).GreaterThanOrEqualTo(maxVolumeAttr!.Min).LessThanOrEqualTo(maxVolumeAttr!.Max).WithMessage($"{nameof(TankDTO.Volume)} превышает допустимый предел.");
        }
    }
    public class UnitDTOValidator : AbstractValidator<UnitDTO>
    {
        public UnitDTOValidator()
        {
            RuleFor(x => x.Name).Length(3, 50).WithMessage($"{nameof(UnitDTO.Name)} должен быть от 3 до 50 символов.");
            RuleFor(x => x.Description).Length(0, 50).WithMessage($"{nameof(UnitDTO.Description)} превышает 50 символов.");
        }
    }
}
