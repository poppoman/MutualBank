using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class Area
    {
        public int AreaId { get; set; }
        public string AreaCity { get; set; } = null!;
        public string AreaTown { get; set; } = null!;
    }
}
