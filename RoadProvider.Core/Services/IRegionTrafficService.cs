using RoadProvider.Core.Entities;
using System.Threading.Tasks;

namespace RoadProvider.Core.Services
{
    public interface IRegionTrafficService : IService<RegionTraffic>
    {
        Task<RegionTraffic> GetByCodeAsync(string code);
    }
}
