using Microsoft.AspNetCore.SignalR;
using PM_Backend.Services;
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_BLL.Services;

namespace PM_Backend.Hubs
{
    public class PkHub : Hub
    {
        private readonly ITournamentsListService _tournamentsListService = new TournamentsListService();
        private readonly ITournamentsTypesService _tournamentsTypesService = new TournamentsTypesService();
        private readonly ITournamentsDetailsService _tournamentsDetailsService = new TournamentsDetailsService();
        private readonly IRegistrationsService _registrationsService = new RegistrationsService();
        private TournamentsListDTO _trList;
        private IEnumerable<TournamentsTypesDTO> _trTypes;
        private IEnumerable<TournamentDetailsDTO> _trDetails;
        private IEnumerable<TournamentPlayersDTO> _trPlayers;

        public PkHub()
        {
            this.startTournamentsManagement();
        }

        public void SendMsgToAll(string message)
        {
           if(Clients != null)
            {
                Clients.All.SendAsync("msgToAll",message);
            }
        }

        public void GetInfosTournament(int trId)
        {
            TournamentDTO infosTr = this._trList.Tournaments.Single(t => t.Id == trId);
            if (infosTr != null) { Clients.Caller.SendAsync("sendInfosTournament", infosTr); }
        }
        public void GetTournamentsDetails()
        {
            if(Clients != null)
            {
                Clients.Caller.SendAsync("sendTournamentsDetails", this._trDetails);
            }
        }

        public void GetTournamentPlayers(int trId)
        {
            if(Clients != null)
            {
                Clients.Caller.SendAsync("sendTournamentPlayers", this._trPlayers.Where(d => d.TournamentId == trId).Select(t => t.Players));
            }
        }
        public void GetIdRegisteredTournaments(int playerId)
        {
            if(Clients != null)
            {
                Clients.Caller.SendAsync("sendIdRegisteredTournaments",this._registrationsService.GetPlayerIdRegisteredTournaments(_trPlayers,playerId));
            }
        }
        public void GetTournamentRankedPlayers(int trId)
        {
            if (Clients != null)
            {
                Clients.Caller.SendAsync("sendTournamentRankedPlayers", this._trPlayers.Where(d => d.TournamentId == trId).Select(t => t.RankedPlayers));
            }
        }
        //public void IsPlayerRegistered(int trId, int playerId)
        //{
        //    Clients.Caller.SendAsync("sendIsPlayerRegistered",);
        //}
        public void startTournamentsManagement()
        {
            this.SendMsgToAll("Initialisation de la TournamentsList...");
            this._trList = _tournamentsListService.GetActiveTournaments();

            this.SendMsgToAll("Initialisation des TournamentsTypes...");
            this._trTypes = _tournamentsTypesService.GetAllTournamentsTypes();

            this.SendMsgToAll("Initialisation des TournamentsDetails...");
            this._trDetails = _tournamentsDetailsService.GetTournamentsDetails(this._trList,this._trTypes);

            this.SendMsgToAll("Initialisation des Players...");
            this._trPlayers = _registrationsService.GetAllRegistrations(_trList);

            this.SendMsgToAll("Initiation de la gestion des tournois...");
            
        }
    }
}
