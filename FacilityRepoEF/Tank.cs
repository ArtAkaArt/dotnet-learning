using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityContextLib
{
    public class Tank
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Volume { get; set; }
        public int Maxvolume { get; set; }
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }
    }
}
