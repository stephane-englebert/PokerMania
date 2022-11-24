using Microsoft.AspNetCore.SignalR;
using PM_Backend.Services;
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Interfaces;
using PM_BLL.Services;
using System.Linq.Expressions;

namespace PM_Backend.Hubs
{
    public class PkHub : Hub
    {
        private readonly ITournamentsListService _tournamentsListService = new TournamentsListService();
        private readonly ITournamentsTypesService _tournamentsTypesService = new TournamentsTypesService();
        private readonly ITournamentsDetailsService _tournamentsDetailsService = new TournamentsDetailsService();
        private readonly IRegistrationsService _registrationsService = new RegistrationsService();
        private readonly ITournamentService _tournamentService = new TournamentService();
        private readonly IMembersService _membersService = new MembersService();
        private TournamentsListDTO _trList;
        private IEnumerable<TournamentsTypesDTO> _trTypes;
        private IEnumerable<TournamentDetailsDTO> _trDetails;
        private List<TournamentPlayersDTO> _trPlayers;

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
        private void SendInfosTournament(int trId, IClientProxy clt)
        {
            TournamentDTO infosTr = this._trList.Tournaments.Single(t => t.Id == trId);
            if (infosTr != null) { clt.SendAsync("sendInfosTournament", infosTr); }
        }        
        public void GetTournamentsDetails()
        {
            if(Clients != null)
            {
                Clients.Caller.SendAsync("sendTournamentsDetails", this._trDetails);
            }
        }

        public void GetTournamentType(int typeId)
        {
            if(Clients != null)
            {
                Clients.Caller.SendAsync("sendTournamentType", this._trTypes.Where(t => t.Id == typeId));
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
        private void SendTrPlayers(int trId, IClientProxy clt)
        {
            List<PlayerDTO> infosTrPl = this._trPlayers.Single(d => d.TournamentId == trId).Players.ToList();
            if (infosTrPl != null) { clt.SendAsync("sendTournamentRankedPlayers", infosTrPl); }
        }        
        private void SendTrRankedPlayers(int trId, IClientProxy clt)
        {
            List<RankedPlayerDTO> infosTrRk = this._trPlayers.Single(d => d.TournamentId == trId).RankedPlayers.ToList();
            if (infosTrRk != null) { clt.SendAsync("sendTournamentRankedPlayers", infosTrRk); }
        }
            
        public void CanUnregister(int trId, int playerId)
        {
            if(Clients != null)
            {
                Clients.Caller.SendAsync("sendCanUnregister", this._registrationsService.CanUnregister(this._trList, this._trPlayers, trId, playerId));
            }
        }
        private void UpdateTrList()
        {
            if(this._trList.Tournaments != null)
            {
                List<TournamentDTO> backUp = this._trList.Tournaments.ToList();
                List<TournamentDTO> dbNew = _tournamentsListService.GetActiveTournaments().Tournaments.ToList();
                if(dbNew != null)
                {
                    IEnumerable<int> listIdTr = backUp.Select(t => t.Id);
                    foreach(TournamentDTO tournament in dbNew)
                    {
                        if (listIdTr.Contains(tournament.Id))
                        {
                            TournamentDTO buTr = backUp.Single(t => t.Id == tournament.Id);
                            dbNew.Single(t => t.Id == tournament.Id).CurrentLevel = buTr.CurrentLevel;
                            dbNew.Single(t => t.Id == tournament.Id).CurrentSmallBlind = buTr.CurrentSmallBlind;
                            dbNew.Single(t => t.Id == tournament.Id).CurrentBigBlind = buTr.CurrentBigBlind;
                            dbNew.Single(t => t.Id == tournament.Id).CurrentAnte = buTr.CurrentAnte;
                            dbNew.Single(t => t.Id == tournament.Id).NextLevel = buTr.NextLevel;
                            dbNew.Single(t => t.Id == tournament.Id).TimeBeforeNextLevel = buTr.TimeBeforeNextLevel;
                            dbNew.Single(t => t.Id == tournament.Id).NextSmallBlind = buTr.NextSmallBlind;
                            dbNew.Single(t => t.Id == tournament.Id).NextBigBlind = buTr.NextBigBlind;
                            dbNew.Single(t => t.Id == tournament.Id).NextAnte = buTr.NextAnte;
                        }
                    }
                    this._trList.Tournaments = dbNew;
                }
            }
        }
        private void UpdateTrDetails()
        {
            this._trDetails = _tournamentsDetailsService.GetTournamentsDetails(this._trList, this._trTypes);
        }
        private void UpdateTrPlayers()
        {
            List<TournamentPlayersDTO> backUp = this._trPlayers;
            List<TournamentPlayersDTO> newTrPlayers = this._registrationsService.GetAllRegistrations(this._trList).ToList();

            if (newTrPlayers != null)
            {
                foreach(TournamentPlayersDTO tournament in newTrPlayers)
                {
                    IEnumerable<int> listIdPl = backUp.Single(p => p.TournamentId == tournament.TournamentId).Players.Select(t => t.Id);
                    List<PlayerDTO> listBUPl = backUp.Single(p => p.TournamentId == tournament.TournamentId).Players.ToList();
                    //List<int> listIdPl = listBUPl.Select(p => p.Id).ToList();
                    List<PlayerDTO> newPlayers = tournament.Players.ToList();

                    newPlayers.ForEach(p => {
                        if (listIdPl.Contains(p.Id))
                        {
                            p.GeneralRanking = listBUPl.Single(pl => pl.Id == p.Id).GeneralRanking;
                            p.TableRanking = listBUPl.Single(pl => pl.Id == p.Id).TableRanking;
                            p.SeatNr = listBUPl.Single(pl => pl.Id == p.Id).SeatNr;
                            p.TurnToPlay = listBUPl.Single(pl => pl.Id == p.Id).TurnToPlay;
                            p.PrivateCards = listBUPl.Single(pl => pl.Id == p.Id).PrivateCards;

                            p.Eliminated = listBUPl.Single(pl => pl.Id == p.Id).Eliminated;         // A supprimer après màj db
                            p.SittingAtTable = listBUPl.Single(pl => pl.Id == p.Id).SittingAtTable; // A supprimer après màj db
                            p.Stack = listBUPl.Single(pl => pl.Id == p.Id).Stack;                   // A supprimer après màj db
                            p.Disconnected = listBUPl.Single(pl => pl.Id == p.Id).Disconnected;     // A supprimer après màj db
                            p.BonusTime = listBUPl.Single(pl => pl.Id == p.Id).BonusTime;           // A supprimer après màj db
                        }
                    });
                    tournament.Players = newPlayers;

                    IEnumerable<int> listIdPlRk = backUp.Single(p => p.TournamentId == tournament.TournamentId).RankedPlayers.Select(t => t.PlayerId);
                    List<RankedPlayerDTO> listBUPlRk = backUp.Single(p => p.TournamentId == tournament.TournamentId).RankedPlayers.ToList();
                    List<RankedPlayerDTO> newRankedPlayers = tournament.RankedPlayers.ToList();

                    newRankedPlayers.ForEach(p =>
                    {
                        if (listIdPlRk.Contains(p.PlayerId))
                        {
                            p.Rank = listBUPlRk.Single(pl => pl.PlayerId == p.PlayerId).Rank;
                            p.TableNr = listBUPlRk.Single(pl => pl.PlayerId == p.PlayerId).TableNr;         // A supprimer après màj db
                            p.Stack = listBUPlRk.Single(pl => pl.PlayerId == p.PlayerId).Stack;             // A supprimer après màj db
                            p.Eliminated = listBUPlRk.Single(pl => pl.PlayerId == p.PlayerId).Eliminated;
                        }
                    });
                    tournament.RankedPlayers = newRankedPlayers;
                }
                this._trPlayers = newTrPlayers;
            }
        }
        public void UpdateNecessary(string typeUpdate)
        {
            if(typeUpdate == "tournaments")
            {
                this.UpdateTrList();
                this.UpdateTrDetails();
                this.UpdateTrPlayers();
                this.GetTournamentsDetails();
            }
            if(typeUpdate == "registrations")
            {
                this.UpdateTrList();
                this.UpdateTrDetails();
                this.UpdateTrPlayers();
                this.GetTournamentsDetails();
            }
            this.SendMsgToAll("Update necessary -> " + typeUpdate);
        }
        public void PlayerGetRegisteredToTr(int trId, int playerId)
        {
            this.UpdateNecessary("registrations");
            if(Clients != null)
            {
                Clients.Caller.SendAsync("sendIdRegisteredTournaments", this._registrationsService.GetPlayerIdRegisteredTournaments(_trPlayers, playerId));
                if (!this._registrationsService.StillFreePlacesForTournament(trId))
                {
                    this._membersService.SetAllRegisteredMembersCurrentTournId(trId);
                    //this.PlayerIsJoiningLobby(trId,playerId);
                    //this.playerIsConnected(trId, playerId);
                    Clients.All.SendAsync("sendTournamentsDetails", this._trDetails);
                    this.LaunchTournament(trId);
                }
            }
        }


        // Méthode qui reprend les actions/vérifications faites lorsqu'un joueur
        // souhaite rejoindre le lobby d'un tournoi
        public void PlayerIsJoiningLobby(int trId,int playerId)
        {
            // Switcher le joueur dans le canal du tournoi

            // Vérifier si joueur pas déjà dans la liste
            if(!this._tournamentService.HasPlayerAlreadyJoinedLobby(trId,playerId)){
                // Acter la présence du joueur
                this.playerIsConnected(trId, playerId);
                this._tournamentService.PlayerIsJoiningLobby(trId, playerId);
                // Vérifier si tous les joueurs présents
                this.SendMsgToAll("MinPlayersConnectedReached = " + this.MinPlayersConnectedReached(trId));                
                this.SendMsgToAll("MaxPlayersConnectedReached = " + this.MaxPlayersConnectedReached(trId));
                if (this.MaxPlayersConnectedReached(trId)) { 
                    this.LaunchTournament(trId);
                }
            }
        }        

        // Retourne true/false en fct du fait que le nombre minimum de joueurs requis
        // pour jouer le tournoi ait été atteint ou non
        private Boolean MinPlayersConnectedReached(int trId)
        {
            int nbPlayersConnected = this.GetNbConnectedPlayers(trId);
            int typeId = this._trDetails.Where(t => t.Id == trId).First().TournamentType;
            int nbMinPlayers = this._trTypes.Where(t => t.Id == typeId).First().MinPlayers;
            return (nbPlayersConnected >= nbMinPlayers);
        }        

        // Retourne true/false en fct du fait que le nombre maximum de joueurs acceptable
        // pour jouer le tournoi ait été atteint ou non
        private Boolean MaxPlayersConnectedReached(int trId)
        {
            int nbPlayersConnected = this.GetNbConnectedPlayers(trId);
            int typeId = this._trDetails.Where(t => t.Id == trId).First().TournamentType;
            int nbMaxPlayers = this._trTypes.Where(t => t.Id == typeId).First().MaxPlayers;
            return (nbPlayersConnected == nbMaxPlayers);
        }

        // Permet de renseigner un joueur comme connecté au lobby d'un tournoi
        // dans trPlayers
        private void playerIsConnected(int trId, int playerId)
        {
            this._trPlayers.Single(d => d.TournamentId == trId).Players.First(p => p.Id == playerId).Disconnected = false;
        }
        
        // Permet de renseigner un joueur comme déconnecté au lobby d'un tournoi
        // dans trPlayers
        private void playerIsDisConnected(int trId, int playerId)
        {
            this._trPlayers.Single(d => d.TournamentId == trId).Players.First(p => p.Id == playerId).Disconnected = true;
        }

        // Permet de créer un nouveau tournoi manuellement dans la base de données
        // A automatiser
        public void CreateTournament(DateTime startDate, string name, int type)
        {
            int idNewTr = this._tournamentService.CreateTournament(startDate, name, type);
            TournamentPlayersDTO newTrPlayersDTO = new TournamentPlayersDTO();
            newTrPlayersDTO.TournamentId = idNewTr;
            this._trPlayers.Add(newTrPlayersDTO);
        }

        // Retourne le nombre de joueurs présents dans le lobby d'un tournoi.
        // La valeur provient de trPlayers (pas de la base de données)
        public int GetNbConnectedPlayers(int trId)
        {
            return this._trPlayers.Single(t => t.TournamentId == trId).Players.Select(p => p.Disconnected == false).Count();
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
                // Créer un canal pour le tournoi

                // Invitations à 'rejoindre lobby' envoyées aux joueurs inscrits               
                    this.UpdateNecessary("tournaments");
                    if (Clients != null) { Clients.All.SendAsync("pleaseJoinTr", trId); }
                    // Vérifier si le tournoi n'a pas été démarré avant la fin du timer
                    Boolean check = this._tournamentService.TournamentAlreadyStarted(trId);
                    if (!check)
                    {
                        // Vérifier le nombre de joueurs qui ont joints le lobby
                        if (this.MinPlayersConnectedReached(trId))
                        {
                            // Si suffisamment de joueurs => Ongoing
                            this.StartTournament(trId);
                            this.SendMsgToAll("Tournoi " + trId + " démarré");
                        }
                        else
                        {
                            // Si pas assez de joueurs => Canceled
                            //this.CloseTournament(trId);
                            this.SendMsgToAll("Tournoi " + trId + " annulé");
                        }
                    }
                // Décompte avant début tournoi
                //System.Timers.Timer aTimer = new System.Timers.Timer(50000);
                //    aTimer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
                //    {
                //        // Vérifier si le tournoi n'a pas été démarré avant la fin du timer
                //        Boolean check = this._tournamentService.TournamentAlreadyStarted(trId);
                //        if (!check)
                //        {
                //            // Vérifier le nombre de joueurs qui ont joints le lobby
                //            if(this.MinPlayersConnectedReached(trId))
                //            {
                //                // Si suffisamment de joueurs => Ongoing
                //                this.StartTournament(trId);
                //                this.SendMsgToAll("Tournoi " + trId + " démarré");
                //            }
                //            else
                //            {
                //                // Si pas assez de joueurs => Canceled
                //                //this.CloseTournament(trId);
                //                this.SendMsgToAll("Tournoi "+trId+" annulé");
                //            }
                //        }                        
                //        aTimer.Stop();
                //        aTimer.Dispose();
                //    };
                //    aTimer.AutoReset = false;
                //    aTimer.Enabled = true;
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
                
                // Initialisation des variables
                this._trList = _tournamentsListService.GetActiveTournaments();
                this._trDetails = _tournamentsDetailsService.GetTournamentsDetails(this._trList, this._trTypes);
                this._trPlayers = _registrationsService.GetAllRegistrations(_trList).ToList();
                TournamentDetailsDTO trDetails = this._trDetails.Single(x => x.Id == trId);
                TournamentsTypesDTO trType = this._trTypes.Single(t => t.Id == trDetails.TournamentType);
                TournamentPlayersDTO trPlayers = this._trPlayers.Single(p => p.TournamentId == trDetails.Id);


                // Répartition aléatoire des joueurs aux tables
                //int nbPlayers = trDetails.RegistrationsNumber;
                //int moduloPlayers = nbPlayers % trType.PlayersPerTable;
                //int nbOfTables = (nbPlayers - moduloPlayers) / trType.PlayersPerTable;
                //if(moduloPlayers > 0) { nbOfTables++; }

                //List<PlayerDTO> pList = this.ShufflePlayers(trPlayers.Players).ToList();
                //int cpt = 0;
                //while (cpt < nbOfTables) {
                //    int nbPl = 0;
                //    while (nbPl < trType.PlayersPerTable)
                //    {
                //        int indice = (cpt * trType.PlayersPerTable) + nbPl;
                //        pList[indice].SittingAtTable = cpt + 1;
                //        nbPl++;
                //    }
                //    cpt++;
                //}
                //trPlayers.Players = pList;
                List<PlayerDTO> pList = trPlayers.Players.ToList();
                pList.ForEach(p => {
                    p.SittingAtTable = 1;
                    p.GeneralRanking = 1;
                    p.TableRanking = 1;
                    
                });
                trPlayers.Players = pList;
                this._trPlayers.Single(d => d.TournamentId == trId).Players = pList;
                this.SendTrPlayers(trId, Clients.All);

                List<RankedPlayerDTO> pListRk = trPlayers.RankedPlayers.ToList();
                pListRk.ForEach(p => {
                    p.TableNr = 1;
                    p.Rank = 1;

                });
                trPlayers.RankedPlayers = pListRk;
                this._trPlayers.Single(d => d.TournamentId == trId).RankedPlayers = pListRk;
                this.SendTrRankedPlayers(trId, Clients.All);

                this.SendInfosTournament(trId, Clients.All);



                //=================================================================
            }
        }

        public IEnumerable<PlayerDTO> ShufflePlayers(IEnumerable<PlayerDTO> IEToShuffle)
        {
            List<PlayerDTO> playerList = new List<PlayerDTO>();
            var r = new Random((int)DateTime.Now.Ticks);
            var shuffledList = IEToShuffle.Select(x => new { Number = r.Next(), Item = x }).OrderBy(x => x.Number).Select(x => x.Item);
            return shuffledList.ToList();
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
        public void CleanDatabase()
        {
            this._tournamentService.CleanDatabase();
            this.UpdateNecessary("tournaments");
            this.SendMsgToAll("Database cleaned");
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
            this._trPlayers = _registrationsService.GetAllRegistrations(_trList).ToList();

            this.SendMsgToAll("Initiation de la gestion des tournois...");
            
        }
    }
}
