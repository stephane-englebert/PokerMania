
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Data.Mappers;
using PM_BLL.Interfaces;
using PM_DAL.Data.Entities;
using PM_DAL.Interfaces;
using PM_DAL.Services;
using System.Linq;

namespace PM_BLL.Services
{
    public class RegistrationsService : IRegistrationsService
    {
        private readonly IRegistrationsRepository _registrationsRepository = new RegistrationsRepository();
        public RegistrationsService(){}
        public IEnumerable<TournamentPlayersDTO> GetAllRegistrations(TournamentsListDTO trList)
        {
            if (trList.Tournaments != null)
            {
                foreach (TournamentDTO tr in trList.Tournaments)
                {
                    IEnumerable<PlayerDTO> trPlayers = _registrationsRepository
                                                        .GetAllRegistrationsForOneTournament(tr.Id)
                                                        .Select(r => r.PlayerDalToDTO())
                                                        .OrderByDescending(t => t.Stack);
                    TournamentPlayersDTO newTrPlayersDTO = new TournamentPlayersDTO(tr.Id,this.RankPlayersByStack(trPlayers), RankPlayersByStack(trPlayers).Select(p => p.PlayerDTOToRankedPlayerDTO()));
                    yield return newTrPlayersDTO;
                }
            }
            else { throw new Exception(); }
        }

        public IEnumerable<PlayerDTO> GetAllRegistrationsForOneTournament(int trId)
        {
            return  _registrationsRepository.GetAllRegistrationsForOneTournament(trId).Select(r => r.PlayerDalToDTO());
        }
        public Boolean IsPlayerRegistered(int trId, int playerId)
        {
            return _registrationsRepository.IsPlayerRegistered(trId, playerId);
        }
        public IEnumerable<int> GetPlayerIdRegisteredTournaments(IEnumerable<TournamentPlayersDTO> trPlayers,int playerId)
        {
            if(trPlayers != null)
            {
                foreach(TournamentPlayersDTO tr in trPlayers)
                {
                    IEnumerable<PlayerDTO> playerRegistered = tr.Players.Where(p => p.Id == playerId);
                    if (playerRegistered.Any()) { yield return tr.TournamentId; }
                }
            }
            else{yield return 0;}
        }
        public IEnumerable<PlayerDTO> RankPlayersByStack(IEnumerable<PlayerDTO> trPlayers)
        {
            trPlayers.OrderByDescending(t => t.Stack);
            int cpt = 1;
            foreach (PlayerDTO player in trPlayers)
            {
                PlayerDTO newPlayerDTO = new PlayerDTO();
                newPlayerDTO = player;
                newPlayerDTO.GeneralRanking = cpt;
                cpt++;
                yield return newPlayerDTO;
            }
        }
    }
}
