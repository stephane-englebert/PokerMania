using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using PM_BLL.Data.DTO.Forms;
using PM_DAL.Data.Entities;
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using Microsoft.AspNetCore.SignalR;
using PM_BLL.Services;
using PM_Backend.Hubs;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly PkHub _hub;

        public TokenController(IAuthService authService, PkHub hub)
        {
            _authService = authService;
            _hub = hub;
        }

        [HttpPost]
        public IActionResult Post(MemberLoginFormDTO member)
        {
            try
            {
                _hub.SendMsgToAll("Recherche de l'utilisateur " + member.UserIdentifier + " en cours...");
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
