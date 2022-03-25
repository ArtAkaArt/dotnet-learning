using System;
using System.Collections.Generic;

namespace FacilityContextLib
{
    public partial class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Factoryid { get; set; }
        public Factory? Factory { get; set; }
        public List<Tank>? Tanks { get; set; }
    }
}
