
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
        private readonly ITournamentRepository _tournamentRepository = new TournamentRepository();
        private readonly ITournamentsListRepository _tournamentsListRepository = new TournamentsListRepository();
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
        public Boolean CanRegister(TournamentsListDTO trList, IEnumerable<TournamentPlayersDTO> trPlayers, int trId, int playerId)
        {
            if(trList.Tournaments != null)
            {
                // Le statut du tournoi permet-il de s'inscrire?
                TournamentDTO? tournament = trList.Tournaments.FirstOrDefault(t => t.Id == trId);
                bool trStatus = ((tournament.Status == "created")||(tournament.Status == "waitingForPlayers"));

                // Le joueur est-il déjà inscrit au tournoi?
                TournamentPlayersDTO? trPlr = trPlayers.FirstOrDefault(t => t.TournamentId == trId);
                bool anyRegistered = trPlr.Players.Any(t => t.Id == playerId);

                return (trStatus && !anyRegistered);
            }
            return false;
        }
        public Boolean CanUnregister(TournamentsListDTO trList, IEnumerable<TournamentPlayersDTO> trPlayers, int trId, int playerId)
        {
            if (trList.Tournaments != null)
            {
                // Le statut du tournoi permet-il de se désinscrire?
                TournamentDTO? tournament = trList.Tournaments.FirstOrDefault(t => t.Id == trId);
                bool trStatus = ((tournament.Status == "created") || (tournament.Status == "waitingForPlayers"));

                // Le joueur est-il inscrit au tournoi?
                TournamentPlayersDTO? trPlr = trPlayers.FirstOrDefault(t => t.TournamentId == trId);
                bool anyRegistered = trPlr.Players.Any(t => t.Id == playerId);

                return (trStatus && anyRegistered);
            }
            return false;
        }
        public void UnregisterTournament(int trId, int playerId)
        {
            if(IsPlayerRegistered(trId, playerId))
            {
                if (!this._tournamentsListRepository.IsTournamentStarted(trId))
                {
                    try
                    {
                        this._registrationsRepository.UnregisterTournament(trId, playerId);
                        this._tournamentRepository.UpdateNumberPlayersRegistered(trId, -1);
                    }
                    catch(Exception e)
                    {
                        throw new Exception("UNREGISTER_TOURN_FAILURE");
                    }
                }
            }
            else
            {
                throw new Exception("UNREGISTER_TOURN_NOT_REGIS");
            }
        }
        public Boolean StillFreePlacesForTournament(int trId)
        {
            return this._registrationsRepository.StillFreePlacesForTournament(trId);
        }

        public void RegisterTournament(int trId, int playerId)
        {
            if (!IsPlayerRegistered(trId, playerId))
            {
                if (!this._tournamentsListRepository.IsTournamentStarted(trId))
                {
                    if (this.StillFreePlacesForTournament(trId))
                    {
                        try
                        {
                            this._registrationsRepository.RegisterTournament(trId, playerId);
                            this._tournamentRepository.UpdateNumberPlayersRegistered(trId, 1);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("REGISTER_TOURN_FAILURE");
                        }

                    }
                    else
                    {
                        throw new Exception("REGISTER_TOURN_NO_MORE_PLACE");
                    }
                }
            }
            else
            {
                throw new Exception("REGISTER_TOURN_ALREADY_REGIS");
            }
        }
    }
}
