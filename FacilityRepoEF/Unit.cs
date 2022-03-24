using System;
using System.Collections.Generic;

namespace FacilityRepoEF
{
    public partial class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Factoryid { get; set; }
    }
}
