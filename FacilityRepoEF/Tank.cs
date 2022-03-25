using System;
using System.Collections.Generic;

namespace FacilityContextLib
{
    public partial class Tank
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Volume { get; set; }
        public int Maxvolume { get; set; }
        public int Unitid { get; set; }
        public Unit? Unit { get; set; }
    }
}
