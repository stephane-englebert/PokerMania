using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM_BLL.Interfaces;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsTypesController : ControllerBase
    {
        private readonly ITournamentsTypesService _tournamentsTypesService;
        public TournamentsTypesController(ITournamentsTypesService tournamentsTypesService)
        {
            _tournamentsTypesService = tournamentsTypesService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_tournamentsTypesService.GetAllTournamentsTypes());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
