using elfBeauty.App.Interfaces;
using elfBeauty.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace elfBeauty.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AestheticController : ControllerBase
    {
        private readonly IAestheticSvc _aestheticSvc;

        public AestheticController(IAestheticSvc aestheticSvc)
        {
            _aestheticSvc = aestheticSvc;
        }

        #region V1 - In-Memory Cache

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("AutocompleteV1")]
        public async Task<ActionResult<IEnumerable<string>>> AutocompleteAsnc_Cache([FromQuery] string? search)
        {
            var aesthetics = await _aestheticSvc.Autocomplete_Cache(search);
            if (!aesthetics.Any())
                return NotFound();

            return Ok(aesthetics);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("GetV1")]
        public async Task<ActionResult<IEnumerable<Aesthetic>>> GetAestheticsAsync_Cache([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] double? latitude = null, [FromQuery] double? longitude = null)
        {
            var aesthetics = await _aestheticSvc.GetAestheticsAsync_Cache(search: search, sortBy: sortBy, userLat: latitude, userLong: longitude);
            if (!aesthetics.Any())
                return NotFound();

            return Ok(aesthetics);
        }

        #endregion

        #region V2 - SQLite Database

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("AutocompleteV2")]
        public async Task<ActionResult<IEnumerable<string>>> AutocompleteAsnc_Db([FromQuery] string? search)
        {
            var aesthetics = await _aestheticSvc.Autocomplete_DB(search);
            if (!aesthetics.Any())
                return NotFound();

            return Ok(aesthetics);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("GetV2")]
        public async Task<ActionResult<IEnumerable<Aesthetic>>> GetAestheticsAsync_Db([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] double? latitude = null, [FromQuery] double? longitude = null)
        {
            var aesthetics = await _aestheticSvc.GetAestheticsAsync_DB(search: search, sortBy: sortBy, userLat: latitude, userLong: longitude);
            if (!aesthetics.Any())
                return NotFound();

            return Ok(aesthetics);
        }

        #endregion
    }
}
