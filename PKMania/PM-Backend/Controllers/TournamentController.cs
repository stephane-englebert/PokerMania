using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM_BLL.Interfaces;
using PM_BLL.Services;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService = new TournamentService();
        
        [HttpGet("lobby/{tr}/{id}")]
        [Authorize(Roles="player")]
        public Boolean Get(int tr, int id)
        {
            try
            {
                return this._tournamentService.CanJoinLobby(tr, id);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
