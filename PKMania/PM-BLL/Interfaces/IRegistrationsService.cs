using PM_BLL.Data.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_BLL.Interfaces
{
    public interface IRegistrationsService
    {
        IEnumerable<TournamentPlayersDTO> GetAllRegistrations(TournamentsListDTO trList);
        IEnumerable<PlayerDTO> GetAllRegistrationsForOneTournament(int trId);
        IEnumerable<int> GetPlayerIdRegisteredTournaments(IEnumerable<TournamentPlayersDTO> trPlayers, int playerId);
        Boolean IsPlayerRegistered(int trId, int playerId);
    }
}
