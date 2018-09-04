using AutoMapper;
using RoadProvider.Core.Entities;
using RoadProvider.Core.Services;
using RoadProvider.Dto.Dtos;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System;

namespace RoadProvider.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Traffic")]
    public class TrafficController : ApiController
    {
        private readonly IRegionTrafficService _regionTrafficService;


        public TrafficController(IRegionTrafficService trafficService)
        {
            _regionTrafficService = trafficService;
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetTraffic()
        {
            try
            {
                List<RegionTraffic> traffics = await _regionTrafficService.GetAllAsync();

                var trafficDtos = new List<RegionTrafficDto>();
                Mapper.Map(traffics, trafficDtos);

                return Ok(trafficDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("ByCode/{code:int}")]
        public async Task<IHttpActionResult> GetTraffic(int code)
        {
            try
            {
                RegionTraffic traffic = await _regionTrafficService.GetByCodeAsync(code.ToString());
                if (traffic == null)
                {
                    return NotFound();
                }
                var trafficDto = new RegionTrafficDto();
                Mapper.Map(traffic, trafficDto);

                return Ok(trafficDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _regionTrafficService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}