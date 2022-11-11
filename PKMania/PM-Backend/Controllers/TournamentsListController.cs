using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM_BLL.Interfaces;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsListController : ControllerBase
    {
        private readonly ITournamentsListService _tournamentsListService;
        public TournamentsListController(ITournamentsListService tournamentsListService)
        {
            _tournamentsListService = tournamentsListService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_tournamentsListService.GetActiveTournaments());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
