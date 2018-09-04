using System;
using RoadProvider.Core.Data;
using RoadProvider.Core.Entities;
using RoadProvider.Core.Services;
using System.Threading.Tasks;
using System.Linq;
using RoadProvider.Services.Services.Helpers;
using RoadProvider.Provider;

namespace RoadProvider.Services.Services
{
    public class RegionTrafficService : BaseService<RegionTraffic>, IRegionTrafficService
    {
        private readonly IRegionService _regionService;

        public RegionTrafficService(IUnitOfWork unitOfWork, IRegionService regionService) : base(unitOfWork)
        {
            _regionService = regionService;
        }

        public async Task<RegionTraffic> GetByCodeAsync(string code)
        {
            int regionId = await GetRegionIdByCode(code);

            if (regionId > 0)
            {
                var trafficRegions = await GetAllAsync();
                var traffic = trafficRegions.Where(q => q.RegionId == regionId).FirstOrDefault();

                if (traffic == null)
                {
                    string regionState = await GetTrafficFromProvider(code);

                    return await AddAsync(new RegionTraffic() { RegionId = regionId, State = regionState, Date = DateTime.Now });
                }

                if (!DateTimeHelper.IsLessThenMinute(traffic.Date))
                {
                    return await UpdateAsync(traffic, code);
                }

                return traffic;
            }
            throw new Exception(string.Format("Couldn't find region with code '{0}.", code));

        }

        private async Task<int> GetRegionIdByCode(string code)
        {
            var regions = await _regionService.GetAllAsync();

            return regions.Where(q => q.Code == code).Select(q => q.Id).FirstOrDefault();
        }

        private async Task<RegionTraffic> UpdateAsync(RegionTraffic traffic, string code)
        {
            string regionState = await GetTrafficFromProvider(code);
            traffic.Date = DateTime.Now;
            traffic.State = regionState;

            await base.UpdateAsync(traffic);

            return traffic;
        }


        private async Task UpdateAllRegionsTrafficAsync()
        {
            var traffics = await GetAllAsync();
            using (var trafficRepo = _unitOfWork.Repository<RegionTraffic>())
            {
                foreach (var traffic in traffics)
                {
                    traffic.State = await GetTrafficFromProvider(traffic.Region.Code);
                    trafficRepo.Update(traffic);
                }
            }
        }

        private async Task<string> GetTrafficFromProvider(string code)
        {
            using (var _httpProvider = new BaseHttpClient())
            {
                return await _httpProvider.GetRegion(code);
            }
        }

    }
}
