using RoadProvider.Core.Data.Repositories;
using RoadProvider.Core.Entities;

namespace RoadProvider.DataImporter.Repositories
{
    public class RegionRepository : BaseRepository<Region>, IRegionRepository
    {
        public RegionRepository(IDbContext context) : base(context)
        { }
    }
}
