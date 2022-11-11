using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM_DAL.Interfaces;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GainsController : ControllerBase
    {
        private readonly IGainsRepository _gainsRepository;
        public GainsController(IGainsRepository gainsRepository)
        {
            _gainsRepository = gainsRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_gainsRepository.GetAllGainsSharings());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
