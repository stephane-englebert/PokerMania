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
        private readonly ITournamentService _tournamentService = new TournamentService();
        private TournamentsListDTO _trList;
        private IEnumerable<TournamentsTypesDTO> _trTypes;
        private IEnumerable<TournamentDetailsDTO> _trDetails;
        private IEnumerable<TournamentPlayersDTO> _trPlayers;

        public PkHub()
        {
            this.startTournamentsManagement();
        }

        public void SendMsgToCaller(string msg, IClientProxy clt)
        {
            if(clt != null)
            {
                clt.SendAsync("msgToCaller", msg);
            }
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
        public void CanUnregister(int trId, int playerId)
        {
            if(Clients != null)
            {
                Clients.Caller.SendAsync("sendCanUnregister", this._registrationsService.CanUnregister(this._trList, this._trPlayers, trId, playerId));
            }
        }
        public void UpdateNecessary(string typeUpdate)
        {
            if(typeUpdate == "tournaments")
            {
                this._trList = _tournamentsListService.GetActiveTournaments();
                this._trDetails = _tournamentsDetailsService.GetTournamentsDetails(this._trList, this._trTypes);
                this.GetTournamentsDetails();
            }
            if(typeUpdate == "registrations")
            {
                this._trList = _tournamentsListService.GetActiveTournaments();
                this._trDetails = _tournamentsDetailsService.GetTournamentsDetails(this._trList, this._trTypes);
                this._trPlayers = _registrationsService.GetAllRegistrations(_trList);
                this.GetTournamentsDetails();
            }
            this.SendMsgToAll("Update necessary -> " + typeUpdate);
        }

        public void PlayerIsJoiningLobby(int trId,int playerId)
        {
            this.SendMsgToCaller("Merci d'avoir prévenu!["+playerId+"]", Clients.Caller);
            this._tournamentService.PlayerIsJoiningLobby(trId, playerId);
            // Acter la présence des joueurs (playerDTO -> 'Disconnected')
            // Vérifier si tous les joueurs présents
        }

        public void CreateTournament(DateTime startDate, string name, int type)
        {
            this._tournamentService.CreateTournament(startDate, name, type);
        }
        public void LaunchTournament(int trId)
        {
            // Basculer status tournoi en 'waitingForPlayers' (ouverture du tournoi)
            Boolean trOpened = this._tournamentService.LaunchTournament(trId);
            if (trOpened)
            {
                //=================================================================
                //                  OUVERTURE D'UN TOURNOI
                //=================================================================

                // Invitations à 'rejoindre lobby' envoyées aux joueurs inscrits               
                    this.UpdateNecessary("tournaments");
                    if(Clients != null){Clients.All.SendAsync("launchTr",trId);}

                // Décompte avant début tournoi
                    System.Timers.Timer aTimer = new System.Timers.Timer(300000);
                    aTimer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
                    {
                        // A la fin du décompte, vérifier le nombre de joueurs qui ont joints le lobby

                        // Si pas assez de joueurs => Canceled

                        // Si suffisamment de joueurs => Ongoing
                        this.StartTournament(trId);
                        // Déroulement du tournoi
                        aTimer.Stop();
                        aTimer.Dispose();
                    };
                    aTimer.AutoReset = false;
                    aTimer.Enabled = true;
                //=================================================================
            }
        }        
        public void StartTournament(int trId)
        {
            // Basculer status tournoi en 'ongoing' (start tournoi)
            Boolean trStarted = this._tournamentService.StartTournament(trId);
            if (trStarted)
            {
                //=================================================================
                //                  DEMARRAGE D'UN TOURNOI
                //=================================================================

                // Invitations à 'rejoindre lobby' envoyées aux joueurs inscrits
                    this.SendMsgToAll("Démarrage du tournoi [" + trId + "]...");
                    this.UpdateNecessary("tournaments");
                    // JL, TRxxx

                // Décompte avant début tournoi
                // A la fin du décompte, vérifier le nombre de joueurs qui ont joints le lobby

                    // Si pas assez de joueurs => Canceled


                    // Si suffisamment de joueurs => Ongoing
                    // Déroulement du tournoi




                //=================================================================
            }
        }

        public void CloseTournament(int trId)
        {
            this._tournamentService.CloseTournament(trId);
            this.UpdateNecessary("tournaments");
        }
        public void DeleteTournament(int trId)
        {
            this._tournamentService.DeleteTournament(trId);
        }
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
