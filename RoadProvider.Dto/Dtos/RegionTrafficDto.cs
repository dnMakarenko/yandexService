using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadProvider.Dto.Dtos
{
    public class RegionTrafficDto : BaseDto
    {
        public string State { get; set; }
        public string Date { get; set; }

        public virtual RegionDto Region { get; set; }
    }
}
