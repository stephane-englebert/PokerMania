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
        Boolean CanRegister(TournamentsListDTO trList, IEnumerable<TournamentPlayersDTO> trPlayers, int trId, int playerId);
        Boolean CanUnregister(TournamentsListDTO trList, IEnumerable<TournamentPlayersDTO> trPlayers, int trId, int playerId);
        void UnregisterTournament(int trId, int playerId);
        Boolean StillFreePlacesForTournament(int trId);
        void RegisterTournament(int trId, int playerId);
    }
}
