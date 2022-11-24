using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM_BLL.Interfaces;
using System.Security.Claims;

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

        [HttpGet("ongoing")]
        [Authorize(Roles="player")]
        public IActionResult GetOngoing()
        {
            try
            {
                return Ok(this._tournamentsListService.IsThereOngoingTrForOnePlayer(int.Parse(User.FindFirstValue("Id"))));
            }catch(Exception e)
            {
                throw;
            }
        }
    }
}
