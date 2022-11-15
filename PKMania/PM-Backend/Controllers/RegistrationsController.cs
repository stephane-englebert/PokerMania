using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM_BLL.Interfaces;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationsService _registrationsService;
        public RegistrationsController(IRegistrationsService registrationsService)
        {
            _registrationsService = registrationsService;
        }

        [HttpGet("{tr}")]
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
    }
}
