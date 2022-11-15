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
        private TournamentsListDTO? _trList;
        private TournamentsTypesDTO[] _trTypes;
        private TournamentDetailsDTO[] _trDetails;

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
        public void GetTournamentsDetails()
        {
            Clients.Caller.SendAsync("sendTournamentsDetails", this._trDetails);
        }
        public void startTournamentsManagement()
        {
            this.SendMsgToAll("Initialisation de la TournamentsList...");
            this._trList = _tournamentsListService.GetActiveTournaments();

            this.SendMsgToAll("Initialisation des TournamentsTypes...");
            this._trTypes = _tournamentsTypesService.GetAllTournamentsTypes().ToArray();

            this.SendMsgToAll("Initialisation des TournamentsDetails...");
            this._trDetails = _tournamentsDetailsService.GetTournamentsDetails(this._trList,this._trTypes).ToArray();

            this.SendMsgToAll("Initiation de la gestion des tournois...");
            
        }
    }
}
