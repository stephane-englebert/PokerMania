using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using System.Security.Claims;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationsService _registrationsService;
        private readonly ITournamentsListService _tournamentsListService;
        public RegistrationsController(
            IRegistrationsService registrationsService,
            ITournamentsListService tournamentsListService
        ){
            _registrationsService = registrationsService;
            _tournamentsListService = tournamentsListService;
        }

        [HttpGet]
        [Authorize(Roles ="player")]
        public IActionResult Get()
        {
            try
            {
                TournamentsListDTO trList = _tournamentsListService.GetActiveTournaments();
                IEnumerable<TournamentPlayersDTO> allRegis = this._registrationsService.GetAllRegistrations(trList);
                return Ok(allRegis);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("TOURN_NO_REGIS_AT_ALL");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{tr}")]
        [Authorize(Roles = "player")]
        public IActionResult Get(int tr)
        {
            try
            {
                return Ok(this._registrationsService.GetAllRegistrationsForOneTournament(tr));
            }
            catch (KeyNotFoundException)
            {
                return NotFound("TOURN_NO_REGIS");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{tr}/{id}")]
        [Authorize(Roles = "player")]
        public IActionResult Get(int tr, int id)
        {
            try
            {
                return Ok(this._registrationsService.IsPlayerRegistered(tr, id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound("TOURN_PLAYER_NO_REGIS");
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpDelete("{tr}")]
        [Authorize(Roles = "player")]
        public IActionResult Delete(int tr)
        {
            try
            {
                this._registrationsService.UnregisterTournament(tr, int.Parse(User.FindFirstValue("Id")));
                return NoContent();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
