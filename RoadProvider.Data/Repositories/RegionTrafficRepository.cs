using RoadProvider.Core.Data.Repositories;
using RoadProvider.Core.Entities;
using System.Data.Entity;
using System.Linq;

namespace RoadProvider.DataImporter.Repositories
{
    public class RegionTrafficRepository : BaseRepository<RegionTraffic>, IRegionTrafficRepository
    {
        private readonly IDbSet<RegionTraffic> _dbEntitySet;

        public RegionTrafficRepository(IDbContext context) : base(context)
        {
            _dbEntitySet = context.Set<RegionTraffic>();
        }

        public RegionTraffic GetByCode(string code)
        {
            return _dbEntitySet.Include(q => q.Region).Where(q => q.Region.Code == code).FirstOrDefault();
        }
    }
}
