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
    [RoutePrefix("api/Regions")]
    public class RegionsController : ApiController
    {
        private readonly IRegionService _regionService;
        private readonly IRegionTrafficService _regionTrafficService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        public RegionsController(IRegionTrafficService trafficService)
        {
            _regionTrafficService = trafficService;
        }

        public RegionsController(IRegionTrafficService regiontrafficService, IRegionService regionService)
        {
            _regionService = regionService;
            _regionTrafficService = regiontrafficService;
        }

        [HttpGet]
        [Route("GetRegions")]
        public async Task<IHttpActionResult> GetRegions()
        {
            try
            {
                List<Region> regions = await _regionService.GetAllAsync();

                var regionDtos = new List<RegionDto>();
                Mapper.Map(regions, regionDtos);

                return Ok(regionDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("GetRegion/{id:int}")]
        public async Task<IHttpActionResult> GetRegion(int id)
        {
            try
            {
                Region region = await _regionService.GetByIdAsync(id);
                if (region == null)
                {
                    return NotFound();
                }

                var regionDto = new RegionDto();
                Mapper.Map(region, regionDto);

                return Ok(regionDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPut]
        public async Task<IHttpActionResult> PutRegion(int id, Region region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != region.Id)
            {
                return BadRequest();
            }

            try
            {
                region.Id = id;

                Region Region = await _regionService.GetByIdAsync(id);


                await _regionService.UpdateAsync(region);
              
                return Ok(region);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostRegion(Region region)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Region = await _regionService.AddAsync(region);

            return CreatedAtRoute("ApiRoute", new { id = Region.Id }, region);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteContact(int id)
        {
            Region region = await _regionService.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            await _regionService.DeleteAsync(region);

            return Ok(region);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _regionService.Dispose();
                _regionTrafficService.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RegionExists(int id)
        {
            return _regionService.GetByIdAsync(id) != null;
        }
    }
}