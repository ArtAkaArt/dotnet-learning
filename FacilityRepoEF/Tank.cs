using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityContextLib
{
    public class Tank
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Volume { get; set; }
        public int Maxvolume { get; set; }
        public int UnitId { get; set; }
        public Unit? Unit { get; set; }
    }
}
