using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class Point
    {
        public int PointId { get; set; }
        public DateTime PointAddDate { get; set; }
        public int PointCaseId { get; set; }
        public bool PointNeedHelp { get; set; }
        public int PointUserId { get; set; }
        public int PointQuantity { get; set; }
        public bool PointIsDone { get; set; }

        public virtual Case PointCase { get; set; } = null!;
    }
}
