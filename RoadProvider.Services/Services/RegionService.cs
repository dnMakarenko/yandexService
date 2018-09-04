using RoadProvider.Core.Data;
using RoadProvider.Core.Entities;
using RoadProvider.Core.Services;

namespace RoadProvider.Services.Services
{
    public class RegionService : BaseService<Region>, IRegionService
    {
        public RegionService(IUnitOfWork unitOfWork):base(unitOfWork)
        { }
    }
}
