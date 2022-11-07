using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using PM_BLL.Data.DTO.Forms;
using PM_DAL.Data.Entities;
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TokenController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Post(MemberLoginFormDTO member)
        {
            try
            {
                LoggedUserDTO connectedUser = _authService.UserLogin(member);
                return Ok(connectedUser);
            }
            catch (AuthenticationException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
