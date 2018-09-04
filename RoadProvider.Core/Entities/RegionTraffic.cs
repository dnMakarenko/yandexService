using System;

namespace RoadProvider.Core.Entities
{
    public class RegionTraffic : BaseEntity
    {
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }

        public string State { get; set; }
        public DateTime Date { get; set; }
    }
}
