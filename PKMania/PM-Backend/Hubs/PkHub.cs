using Microsoft.AspNetCore.SignalR;
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Data.DTO.Entities.Hubs;
using PM_BLL.Interfaces;
using PM_BLL.Services;
using System.Linq;

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
        private List<RoomConnection> _trRooms;
        private List<int> sb = new List<int>(){15, 20, 25, 30, 40, 50, 60, 75, 100,125,150,200,250,300,350,400,500,600,700,800,900,1000,1200,
        1400,1600,1800,2000,2400,2800,3200,3600,4000,4500,5000,6000,7000,8000,9000,10000,12500,15000,17500,20000,22500,25000,
        30000,35000,40000,50000,60000,70000,80000,100000,125000,150000,200000,250000,300000,400000,500000,600000,700000,800000,
        1000000,1250000,1500000,2000000,2500000,3000000,4000000,5000000,6000000,7000000,8000000,10000000,12500000,15000000,20000000,25000000 };
        private List<int> bb = new List<int>(){30,40,50,60,80,100,120,150,200,250,300,400,500,600,700,800,1000,1200,1400,1600,1800,2000,2400,
        2800,3200,3600,4000,4800,5600,6400,7200,8000,9000,10000,12000,14000,16000,18000,20000,25000,30000,35000,40000,45000,50000,60000,
        70000,80000,100000,120000,140000,160000,200000,250000,300000,400000,500000,600000,800000,1000000,1200000,1400000,1600000,2000000,
        2500000,3000000,4000000,5000000,6000000,8000000,10000000,12000000,14000000,16000000,20000000,25000000,30000000,40000000,50000000};
        private List<int> ante = new List<int>() { 4,5,6,7,10,12,15,20,25,30,40,50,60,70,85,100,125,150,175,200,225,250,300,350,400,450,500,600,700,
        800,900,1000,1100,1200,1500,1800,2000,2200,2500,3100,3800,4400,5000,5600,6200,7500,8800,10000,12500,15000,17500,20000,25000,31200,37500,
        50000,62500,75000,100000,125000,150000,175000,200000,250000,312500,375000,500000,625000,750000,1000000,1250000,1500000,1750000,2000000,
        2500000,3125000,3750000,5000000,6250000};
        private List<CurrentHandDTO> _trHands = new List<CurrentHandDTO>();

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
                Clients.All.SendAsync("sendTournamentsDetails", this._trDetails);
                if (!this._registrationsService.StillFreePlacesForTournament(trId))
                {
                    this._membersService.SetAllRegisteredMembersCurrentTournId(trId);
                    this.LaunchTournament(trId);
                }
            }
        }

        private void CheckPlayersConnected(int trId)
        {
            List<int> listIdDisconnected = this._membersService.GetIdOfDisconnectedPlayers(trId).ToList();
            List<PlayerDTO> regisPlayers = this._trPlayers.Single(d => d.TournamentId == trId).Players.ToList();
            if(listIdDisconnected.Count > 0)
            {
                int cptModif = 0;
                foreach(PlayerDTO player in regisPlayers){
                    if (listIdDisconnected.Contains(player.Id)){
                        this.playerIsDisConnected(trId,player.Id);
                        cptModif++; 
                    }
                }
                if(cptModif > 0) { this.SendTrPlayersToRoomPlayers(trId); }
            }
            Console.WriteLine(regisPlayers.Count);
        }
        private Boolean CheckIfPlayerAlreadyConnected(int trId, int playerId)
        {
            if(this._trRooms != null)
            {
                return this._trRooms.SingleOrDefault(r => r.PlayerId == playerId) != null;
            }
            else { return false; }
        }
        public void JoinRoom(string roomName,int trId, int playerId)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            Console.WriteLine("on passe par joinroom serveur");
            if(this.CheckIfPlayerAlreadyConnected(trId, playerId))
            {
                int exTrId = this._trRooms.Single(r => r.PlayerId == playerId).TournamentId;
                string exConId = this._trRooms.Single(r => r.PlayerId == playerId).ConnectionId;
                string exRoomName = this._trRooms.Single(r => r.PlayerId == playerId).RoomName;
                if (exTrId != trId || exConId != Context.ConnectionId){
                    this.playerIsDisConnected(exTrId, playerId);
                    this.LeaveRoom(exRoomName, exConId);
                    if(exTrId != trId){this.QuitTournament(trId, playerId);}
                }
                this._trRooms.Single(r => r.PlayerId == playerId).RoomName = roomName;
                this._trRooms.Single(r => r.PlayerId == playerId).ConnectionId = Context.ConnectionId;
                this._trRooms.Single(r => r.PlayerId == playerId).TournamentId = trId;
            }
            else
            {
                if(this._trRooms == null){this._trRooms = new List<RoomConnection>();}
                RoomConnection newRommCon = new RoomConnection(roomName, Context.ConnectionId, trId, playerId);
                this._trRooms.Add(newRommCon);
            }
            this.playerIsConnected(trId, playerId);
            this.SendTrPlayersToRoomPlayers(trId);
            this.CheckPlayersConnected(trId);
            Clients.Caller.SendAsync("roomJoined");
            if (this.MaxPlayersConnectedReached(trId)) { this.StartTournament(trId); }
        }
        public Task LeaveRoom(string roomName, string conId)
        {
            if(conId == null) { conId = Context.ConnectionId; }
            return Groups.RemoveFromGroupAsync(conId, roomName);
        }
        private void SendTrPlayersToRoomPlayers(int trId) {

                Clients.Group("tr" + trId).SendAsync("sendTournamentPlayers", this._trPlayers.Where(d => d.TournamentId == trId).Select(t => t.Players));
                Clients.Group("tr" + trId).SendAsync("sendTournamentRankedPlayers", this._trPlayers.Where(d => d.TournamentId == trId).Select(t => t.RankedPlayers));
                Clients.Group("tr" + trId).SendAsync("testSA", this._trRooms);

        }
        public void QuitTournament(int trId, int playerId)
        {
            this._trPlayers.Single(d => d.TournamentId == trId).Players.Single(p => p.Id == playerId).Eliminated = true;
            this._trPlayers.Single(d => d.TournamentId == trId).RankedPlayers.Single(p => p.PlayerId == playerId).Eliminated = true;
            this.UpdateTrPlayers();
            this._registrationsService.EliminateFromTournament(trId, playerId);
            this.SendTrPlayersToRoomPlayers(trId);
        }

        // Méthode qui reprend les actions/vérifications faites lorsqu'un joueur
        // souhaite rejoindre le lobby d'un tournoi
        //public void PlayerIsJoiningLobby(int trId,int playerId)
        //{
        //    // Switcher le joueur dans le canal du tournoi

        //    // Vérifier si joueur pas déjà dans la liste
        //    if(!this._tournamentService.HasPlayerAlreadyJoinedLobby(trId,playerId)){
        //        // Acter la présence du joueur
        //        this.playerIsConnected(trId, playerId);
        //        this._tournamentService.PlayerIsJoiningLobby(trId, playerId);
        //        // Vérifier si tous les joueurs présents
        //        this.SendMsgToAll("MinPlayersConnectedReached = " + this.MinPlayersConnectedReached(trId));                
        //        this.SendMsgToAll("MaxPlayersConnectedReached = " + this.MaxPlayersConnectedReached(trId));
        //        if (this.MaxPlayersConnectedReached(trId)) { 
        //            this.LaunchTournament(trId);
        //        }
        //    }
        //}        

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
            this._membersService.SetPlayerIsConnected(playerId);
        }
        
        // Permet de renseigner un joueur comme déconnecté au lobby d'un tournoi
        // dans trPlayers
        private void playerIsDisConnected(int trId, int playerId)
        {
            this._trPlayers.Single(d => d.TournamentId == trId).Players.First(p => p.Id == playerId).Disconnected = true;
            this._membersService.SetPlayerIsDisconnected(playerId);
            Console.WriteLine("Déconnexion joueur [" + playerId +"] du tournoi ["+trId+"]");
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
                this._trList.Tournaments.Single(t => t.Id == trId).StartedOn = DateTime.Now;
                this._trList.Tournaments.Single(t => t.Id == trId).CurrentLevel = 1;

                // Envoyer ces infos aux joueurs participant au tournoi
                this.SendInfosTournament(trId, Clients.Group("tr"+trId));

                //=================================================================
                // au hasard, choisir le premier joueur à la parole
                Random rand = new Random();
                int firstPl = rand.Next(1, 10) % 2;
                int secondPl = (firstPl == 0) ? 1 : 0;
                int idFirstPlayer = trPlayers.Players.ToList()[firstPl].Id;
                int idSecondPlayer = trPlayers.Players.ToList()[secondPl].Id;

                // démarrer la main
                this.startHand(trId,idFirstPlayer,idSecondPlayer);

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
        public void startHand(int trId, int idFirstPlayer, int idSecondPlayer)
        {
            // récupération des données des joueurs
            IEnumerable<PlayerDTO> trPlayers = this._trPlayers.Single(t => t.TournamentId == trId).Players;

            // créer une nouvelle main
            CurrentHandDTO hand = new CurrentHandDTO();
            hand.TournamentId = trId;
            hand.TableNr = 1;
            hand.StartedOn = DateTime.Now;           

            // placer le bouton (playerId 1er joueur)
            hand.SeatNrButton = idFirstPlayer;

            // positionner les blinds (SB & BB) à table
            hand.SeatNrSmallBlind = idFirstPlayer; // playerId 1er joueur
            hand.SeatNrBigBlind = idSecondPlayer; // playerId 2ème joueur

            // Positionner les joueurs à la table 1
            hand.SeatNrPlayerToPlay = hand.SeatNrSmallBlind;
            this.UpdateFieldPlayerDTO("sittingAtTable",1,trId,hand.SeatNrSmallBlind);
            this.UpdateFieldPlayerDTO("sittingAtTable",1,trId,hand.SeatNrBigBlind);

            // Préciser qui doit démarrer la main
            this.UpdateFieldPlayerDTO("turnToPlay", 1, hand.TournamentId, hand.SeatNrSmallBlind);
            this.UpdateFieldPlayerDTO("turnToPlay", 0, hand.TournamentId, hand.SeatNrBigBlind);

            // adapter les niveaux de SB, BB & Ante en fonction du level en cours
            int crtLvl = this._trList.Tournaments.Single(t => t.Id == trId).CurrentLevel;
            this._trList.Tournaments.Single(t => t.Id == trId).CurrentSmallBlind = this.sb[crtLvl];
            this._trList.Tournaments.Single(t => t.Id == trId).CurrentBigBlind = this.bb[crtLvl];
            this._trList.Tournaments.Single(t => t.Id == trId).CurrentAnte = this.ante[crtLvl];            
            this._trList.Tournaments.Single(t => t.Id == trId).NextLevel = crtLvl + 1;
            this._trList.Tournaments.Single(t => t.Id == trId).NextSmallBlind = this.sb[crtLvl + 1];
            this._trList.Tournaments.Single(t => t.Id == trId).NextBigBlind = this.bb[crtLvl + 1];
            this._trList.Tournaments.Single(t => t.Id == trId).NextAnte = this.ante[crtLvl + 1];

            // percevoir les blinds
            hand = this.TakingBlinds(hand);

            // màj data 
            this._trHands.Add(hand);

            // prendre un nouveau jeu de cartes et le mélanger
            this.ShuffleCards(hand);

            // distribuer les cartes privées aux joueurs
            this.DealPrivateCards(hand);

            // transmettre infos aux joueurs
            this.SendTrPlayersToRoomPlayers(hand.TournamentId);
            this.SendInfosTournament(trId, Clients.Group("tr" + trId));
            Clients.Group("tr" + hand.TournamentId).SendAsync("sendHand", hand);
            SpeakDTO firstSpeak = new SpeakDTO(hand.Guid);
            firstSpeak.IdTr = trId;
            firstSpeak.Guid = hand.Guid;
            firstSpeak.TurnSpeak = 0;
            firstSpeak.IdPlay = idFirstPlayer;
            firstSpeak.IdNext = idSecondPlayer;
            firstSpeak.Choice = "raise";
            firstSpeak.NewPlayerBet = trPlayers.Single(p => p.Id == idSecondPlayer).MoneyInPot;
            Clients.Group("tr" + hand.TournamentId).SendAsync("takeDecision", firstSpeak);
        }
        public void TakingDecision(SpeakDTO speak)
        {
            // Màj données joueurs/tournoi
            IEnumerable<PlayerDTO> trPlayers = this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players;
            int moneyInPotFirstPlayer = trPlayers.Single(p => p.Id == speak.IdPlay).MoneyInPot;
            int moneyInPotSecondPlayer = trPlayers.Single(p => p.Id == speak.IdNext).MoneyInPot;
            int stackFirstPlayer = this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players.Single(p => p.Id == speak.IdPlay).Stack;
            if(stackFirstPlayer < speak.NewPlayerBet) { speak.NewPlayerBet = stackFirstPlayer; } // All in
            int handProgress = this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid).Progress;
            this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players.Single(p => p.Id == speak.IdPlay).Stack -= speak.NewPlayerBet;
            this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players.Single(p => p.Id == speak.IdPlay).MoneyInPot += speak.NewPlayerBet;
            this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid).Pot += speak.NewPlayerBet;
            this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players.Single(p => p.Id == speak.IdPlay).TurnToPlay = false;
            this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players.Single(p => p.Id == speak.IdNext).TurnToPlay = true;

            if (speak.Choice == "fold")
            {
                this.SharingPot(speak,false,speak.IdNext,speak.IdPlay);
            }
            if(speak.Choice == "raiseCheck")
            {
                // 1er check après le flop, la turn ou la river
                if(speak.TurnSpeak == 1)
                {
                    // lorsque l'on vient de distribuer les cartes privées, la Big Blind
                    // a le droit de raise même si la Small Blind a "raiseCheck"
                    // (uniquement dans le cas de la toute première prise de parole de la Big Blind)
                    if ((moneyInPotFirstPlayer + speak.NewPlayerBet) == moneyInPotSecondPlayer)
                    {
                        // La Big Blind décide de checker et de découvrir le Flop
                        this.OkGoFurther(speak, "check");
                    }
                    else
                    {
                        if ((moneyInPotFirstPlayer + speak.NewPlayerBet) > moneyInPotSecondPlayer)
                        {
                            // La Big Blind décide de surenchérir
                            speak.Choice = "raise";
                            this.SendDecisionToOpponent(speak);
                        }
                        else
                        {
                            // ou est All-In...
                            this.SharingPot(speak, true, 0, 0);
                        }
                    }
                }
                else
                {
                    // dans les autres cas, pas possible de re-raise si votre adversaire a accepté votre mise
                    this.OkGoFurther(speak, "check");
                }
            }else if(speak.Choice == "check")
            {
                // On passe à l'étape suivante (Flop -> Turn -> River)
                // Premier de parole à compter du Flop
                speak.Choice = "proposeCheck";
                this.SendDecisionToOpponent(speak);
            }else if(speak.Choice == "checkAccepted")
            {
                // Check proposé par l'adversaire accepté
                // On passe à l'étape suivante (Flop -> Turn -> River)
                this.OkGoFurther(speak, "check");
            }else if(speak.Choice == "raise")
            {
                if (stackFirstPlayer < speak.NewPlayerBet)
                {
                    // Le joueur se met All-in
                    this.SharingPot(speak, true, 0, 0);
                }
                else
                {
                    // Si le raise correspond à simplement égaliser la mise précédente,
                    if ((moneyInPotFirstPlayer + speak.NewPlayerBet) == moneyInPotSecondPlayer)
                    {
                        // Alors on considère que cela équivaut à un simple check (raiseCheck)                        
                        speak.Choice = "raiseCheck";
                    }
                    else if((moneyInPotFirstPlayer + speak.NewPlayerBet) > moneyInPotSecondPlayer)
                    {
                        // Sinon, il s'agit d'un vrai 'raise' ou 're-raise'
                        speak.Choice = "raise";
                    }             
                    this.SendDecisionToOpponent(speak);
                }
            }
        }
        private void OkGoFurther(SpeakDTO speak, string choice)
        {
            this.ProgressHand(speak);
            speak.Choice = choice;
            this.SendDecisionToOpponent(speak);
        }
        private void SendDecisionToOpponent(SpeakDTO speak)
        {
            int temp = speak.IdPlay;
            speak.IdPlay = speak.IdNext;
            speak.IdNext = temp;
            speak.TurnSpeak += 1;
            this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid).SeatNrPlayerToPlay = speak.IdPlay;
            this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid).SeatNrSmallBlind = speak.IdPlay;
            this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid).SeatNrBigBlind = speak.IdNext;
            CurrentHandDTO hand = this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid);
            //this.SaveHand(hand);
            this.SendTrPlayersToRoomPlayers(hand.TournamentId);
            this.SendInfosTournament(speak.IdTr, Clients.Group("tr" + speak.IdTr));
            Clients.Group("tr" + hand.TournamentId).SendAsync("sendHand", hand);
            Clients.Group("tr" + speak.IdTr).SendAsync("takeDecision", speak);
        }

        private void ProgressHand(SpeakDTO speak)
        {
            this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid).Progress += 1;
            CurrentHandDTO hand = this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid);
            int newProgress = hand.Progress;
            if(newProgress == 1)
            {
                this.DealFlop(hand);

            }
            if(newProgress == 2)
            {
                this.DealTurn(hand);

            }
            if(newProgress == 3)
            {
                this.DealRiver(hand);
            }
            if(newProgress == 4)
            {
                this.SharingPot(speak, true, 0, 0);
            }
        }

        public void SharingPot(SpeakDTO speak, Boolean showDown, int idWinner, int idLooser)
        {
            // récupération des données des joueurs
            IEnumerable<PlayerDTO> trPlayers = this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players;
            int stackFirstPlayer = trPlayers.Single(p => p.Id == speak.IdPlay).Stack;
            int stackSecondPlayer = trPlayers.Single(p => p.Id == speak.IdNext).Stack;

            if (showDown)
            {
                // Comparaison des mains afin de décider qui gagne en cas de Showdown
                CurrentHandDTO hand = this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid);
                List<CardDTO> flop = hand.Flop.ToList();
                CardDTO turn = hand.Turn;
                CardDTO river = hand.River;
                List<CardDTO> privateCardsFirstPlayer = trPlayers.Single(p => p.Id == speak.IdPlay).PrivateCards.ToList();
                List<CardDTO> privateCardsSecondPlayer = trPlayers.Single(p => p.Id == speak.IdNext).PrivateCards.ToList();
                int handResult = this.whoWins(speak.IdPlay, speak.IdNext, flop, turn, river, privateCardsFirstPlayer, privateCardsSecondPlayer);
                if(handResult == -1)
                {
                    // Stricte égalité - pot partagé

                }
                else
                {
                    // 1 seul gagnant
                    idWinner = handResult;
                    idLooser = (idWinner == speak.IdPlay) ? speak.IdNext : speak.IdPlay;

                    int moneyInPotWinner = trPlayers.Single(p => p.Id == idWinner).MoneyInPot;
                    int moneyInPotLooser = trPlayers.Single(p => p.Id == idLooser).MoneyInPot;
                    this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players.Single(p => p.Id == idWinner).MoneyInPot = 0;
                    this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players.Single(p => p.Id == idLooser).MoneyInPot = 0;
                    this._trPlayers.Single(t => t.TournamentId == speak.IdTr).Players.Single(p => p.Id == idWinner).Stack += moneyInPotWinner + moneyInPotLooser;
                    if (speak.IdPlay == idWinner) { stackFirstPlayer += moneyInPotWinner + moneyInPotLooser; } else { stackSecondPlayer += moneyInPotWinner + moneyInPotLooser; }

                    // Vérifier si le tournoi est terminé ou non (1 des 2 joueurs avec une stack == 0
                    if (stackFirstPlayer == 0 || stackSecondPlayer == 0)
                    {
                        // Annonce du gagnant
                        if (stackFirstPlayer > 0) { idWinner = speak.IdPlay; } else { idWinner = speak.IdNext; }
                        Clients.Group("tr" + speak.IdTr).SendAsync("endTournament", idWinner);
                    }
                }
            }

            if (stackFirstPlayer != 0 && stackSecondPlayer != 0)
            {
                // Finaliser la main
                this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid).FinishedOn = DateTime.Now;
                this._trHands.Single(h => h.TournamentId == speak.IdTr && h.Guid == speak.Guid).Pot = 0;
                // Mettre à jour la 'Ranked' List
                this._trPlayers.Single(t => t.TournamentId == speak.IdTr).RankedPlayers.Single(p => p.PlayerId == speak.IdPlay).Stack = stackFirstPlayer;
                this._trPlayers.Single(t => t.TournamentId == speak.IdTr).RankedPlayers.Single(p => p.PlayerId == speak.IdNext).Stack = stackSecondPlayer;
                int rankFirstPlayer = (stackFirstPlayer >= stackSecondPlayer) ? 1 : 2;
                int rankSecondPlayer = (rankFirstPlayer == 1) ? 2 : 1;
                this._trPlayers.Single(t => t.TournamentId == speak.IdTr).RankedPlayers.Single(p => p.PlayerId == speak.IdPlay).Rank = rankFirstPlayer;
                this._trPlayers.Single(t => t.TournamentId == speak.IdTr).RankedPlayers.Single(p => p.PlayerId == speak.IdNext).Rank = rankSecondPlayer;
                List<RankedPlayerDTO> orderedRkList = this._trPlayers.Single(t => t.TournamentId == speak.IdTr).RankedPlayers.ToList().OrderBy(p => p.Rank).ToList();
                this._trPlayers.Single(t => t.TournamentId == speak.IdTr).RankedPlayers = orderedRkList;
                this.SendTrRankedPlayers(speak.IdTr, Clients.Group("tr" + speak.IdTr));
                // transmettre infos aux joueurs
                //this.SendTrPlayersToRoomPlayers(speak.IdTr);
                //this.SendInfosTournament(speak.IdTr, Clients.Group("tr" + speak.IdTr));
                // Démarrer une nouvelle main en inversant l'ordre de parole
                this.startHand(speak.IdTr,speak.IdNext,speak.IdPlay);
            }
        }
        private int whoWins(int idP1, int idP2,List<CardDTO> flop, CardDTO turn, CardDTO river, List<CardDTO> privateCardsFirstPlayer, List<CardDTO> privateCardsSecondPlayer)
        {
            List<CardDTO> sevenCardsP1 = new List<CardDTO>();
            List<CardDTO> sevenCardsP2 = new List<CardDTO>();
            sevenCardsP1 = flop.ToList();
            sevenCardsP1.Add(turn);
            sevenCardsP1.Add(river);
            sevenCardsP2 = sevenCardsP1.ToList();
            sevenCardsP1.AddRange(privateCardsFirstPlayer.ToList());
            sevenCardsP2.AddRange(privateCardsSecondPlayer.ToList());
            Clients.Group("tr3004").SendAsync("whowins", sevenCardsP1, sevenCardsP2);
            int handLevelP1 = this.whichHandLevel(sevenCardsP1);
            int handLevelP2 = this.whichHandLevel(sevenCardsP2);
            if(handLevelP1 > handLevelP2) { return idP1;}
            if(handLevelP2 > handLevelP1) { return idP2;}
            return this.CompareTwoHandsOfSameLevel(idP1,idP2,handLevelP1,sevenCardsP1,sevenCardsP2);
        }
        private int CompareTwoHandsOfSameLevel(int idP1, int idP2, int handLevel, List<CardDTO> cardsP1, List<CardDTO> cardsP2)
        {
            List<int> NbOfCardsPerFamilyP1 = this.GetNbOfCardsPerFamily(cardsP1);
            int MaxNbOfCardsPerFamilyP1 = NbOfCardsPerFamilyP1.Max();
            List<int> NbOfCardsPerValueP1 = this.GetNbOfCardsPerValues(cardsP1);
            int MaxNbOfCardsPerValueP1 = NbOfCardsPerValueP1.Max();

            List<int> NbOfCardsPerFamilyP2 = this.GetNbOfCardsPerFamily(cardsP2);
            int MaxNbOfCardsPerFamilyP2 = NbOfCardsPerFamilyP2.Max();
            List<int> NbOfCardsPerValueP2 = this.GetNbOfCardsPerValues(cardsP2);
            int MaxNbOfCardsPerValueP2 = NbOfCardsPerValueP2.Max();

            switch (handLevel)
            {
                case 9:
                    List<CardDTO> quinteFlushCardsP1 = this.GetFlushCards(cardsP1);
                    List<CardDTO> quinteFlushCardsP2 = this.GetFlushCards(cardsP2);
                    return this.CompareTwoFlushes(idP1, idP2, quinteFlushCardsP1, quinteFlushCardsP2);
                    break;
                case 8: return this.CompareTwoCarre(idP1, idP2, NbOfCardsPerValueP1, NbOfCardsPerValueP2); break;
                case 7:
                    List<CardDTO> fullThreesP1 = GetThreeOfAKind(cardsP1);
                    List<CardDTO> fullThreesP2 = GetThreeOfAKind(cardsP2);
                    int result = this.CompareTwoThrees(idP1, idP2, fullThreesP1, fullThreesP2);
                    if(result == -1)
                    {
                        List<CardDTO> fullPairsP1 = GetPairs(cardsP1);
                        List<CardDTO> fullPairsP2 = GetPairs(cardsP2);
                        return this.CompareTwoPairs(idP1, idP2, fullPairsP1, fullPairsP2);
                    }
                    return -1;
                    break;
                case 6:
                    List<CardDTO> flushCardsP1 = this.GetFlushCards(cardsP1);
                    List<CardDTO> flushCardsP2 = this.GetFlushCards(cardsP2);
                    return this.CompareTwoFlushes(idP1,idP2,flushCardsP1,flushCardsP2);
                    break;
                case 5: return this.CompareTwoQuinte(idP1,idP2,cardsP1,cardsP2); break;
                case 4:
                    List<CardDTO> threesP1 = GetThreeOfAKind(cardsP1);
                    List<CardDTO> threesP2 = GetThreeOfAKind(cardsP2);
                    return this.CompareTwoThrees(idP1, idP2, threesP1, threesP2);
                    break;
                case 3:
                    List<CardDTO> twoPairsP1 = GetPairs(cardsP1);
                    List<CardDTO> twoPairsP2 = GetPairs(cardsP2);
                    return this.CompareTwoOrThreePairs(idP1, idP2, twoPairsP1, twoPairsP2);
                    break;
                case 2:
                    List<CardDTO> pairsP1 = GetPairs(cardsP1);
                    List<CardDTO> pairsP2 = GetPairs(cardsP2);
                    return this.CompareTwoPairs(idP1, idP2, pairsP1, pairsP2); 
                    break;
                case 1: return this.CompareTwoHigherCard(idP1, idP2, NbOfCardsPerValueP1, NbOfCardsPerValueP2); break;
                default: return idP1;
            }
        }
        private int CompareTwoFlushes(int idP1, int idP2, List<CardDTO> flushP1, List<CardDTO> flushP2)
        {
            string typeCards = "akqjt98765432";
            List<CardDTO> flushCardsP1 = this.OrderCardsInList(flushP1);
            List<CardDTO> flushCardsP2 = this.OrderCardsInList(flushP2);
            for(int i=0; i < flushCardsP1.Count(); i++)
            {
                if (typeCards.IndexOf(flushCardsP1[i].Abbreviation.Last()) < typeCards.IndexOf(flushCardsP2[i].Abbreviation.Last())) { return idP1; }
                if (typeCards.IndexOf(flushCardsP2[i].Abbreviation.Last()) < typeCards.IndexOf(flushCardsP1[i].Abbreviation.Last())) { return idP2; }
            }
            return -1;
        }
        private List<CardDTO> OrderCardsInList(List<CardDTO> cards)
        {
            string typeCards = "akqjt98765432";
            List<CardDTO> newCards = new List<CardDTO>();
            for(int i=0; i<13; i++)
            {
                foreach(CardDTO card in cards)
                {
                    if(card.Abbreviation.Last() == typeCards[i])
                    {
                        newCards.Add(card);
                    }
                }
            }
            return newCards;
        }
        private List<CardDTO> DeleteCardInList(List<CardDTO> cards, CardDTO toDel)
        {
            List<CardDTO> newCards = new List<CardDTO>();
            foreach (CardDTO card in cards)
            {
                if (toDel != card) { newCards.Add(card); }
            }
            return newCards;
        }
        private List<CardDTO> GetFlushCards(List<CardDTO> cards)
        {
            List<char> cardFamilies = new List<char>() { 'h', 'd', 'c', 's' };
            List<int> NbOfCardsPerFamily = this.GetNbOfCardsPerFamily(cards);
            int MaxNbOfCardsPerFamily = NbOfCardsPerFamily.Max();
            char family = this.GetMostRepresentedFamily(NbOfCardsPerFamily);
            return this.GetCardsOfOneFamily(cards, family);
        }
        private char GetMostRepresentedFamily(List<int> NbOfCardsPerFamily)
        {
            List<char> cardFamilies = new List<char>() { 'h', 'd', 'c', 's' };
            int cpt = 0;
            for (int i= 1; i < 4; i++)
            {
                if(NbOfCardsPerFamily[i] > cpt) { cpt = i; }
            }
            return cardFamilies[cpt];
        }
        private int CompareTwoThrees(int idP1, int idP2, List<CardDTO> threesP1, List<CardDTO> threesP2)
        {
            string typeThrees = "akqjt98765432";
            int indThreesP1 = 0;
            int indThreesP2 = 0;
            if(threesP1.Count > 3) {
                int indFirstThrees = typeThrees.IndexOf(threesP1[0].Abbreviation.Last());
                int indSecondThrees = typeThrees.IndexOf(threesP1[3].Abbreviation.Last());
                if (indFirstThrees < indSecondThrees){ indThreesP1 = indFirstThrees; } else { indThreesP1 = indSecondThrees; }
            }
            if(threesP2.Count > 3) {
                int indFirstThrees = typeThrees.IndexOf(threesP2[0].Abbreviation.Last());
                int indSecondThrees = typeThrees.IndexOf(threesP2[3].Abbreviation.Last());
                if (indFirstThrees < indSecondThrees){ indThreesP2 = indFirstThrees; } else { indThreesP2 = indSecondThrees; }
            }
            if (threesP1.Count == 3){indThreesP1 = typeThrees.IndexOf(threesP1[0].Abbreviation.Last());}
            if (threesP2.Count == 3){indThreesP2 = typeThrees.IndexOf(threesP2[0].Abbreviation.Last());}
            if (indThreesP1 < indThreesP2) { return idP1; }
            if (indThreesP2 < indThreesP1) { return idP2; }
            return -1;
        }
        private int CompareTwoOrThreePairs(int idP1, int idP2, List<CardDTO> pairsP1, List<CardDTO> pairsP2)
        {
            List<CardDTO> orderedPairsP1 = this.OrderPairsList(pairsP1);
            List<CardDTO> orderedPairsP2 = this.OrderPairsList(pairsP2);
            int idWin = this.CompareTwoPairs(idP1, idP2, orderedPairsP1, orderedPairsP2);
            if (idWin == -1)
            {
                orderedPairsP1 = this.DeletePairInList(orderedPairsP1, orderedPairsP1[0]);
                orderedPairsP2 = this.DeletePairInList(orderedPairsP2, orderedPairsP2[0]);
                idWin = this.CompareTwoPairs(idP1, idP2, orderedPairsP1, orderedPairsP2);
            }
            return idWin;
        }
        private List<CardDTO> OrderPairsList(List<CardDTO> cards)
        {
            List<CardDTO> newCards = new List<CardDTO>();
            for (int i=0; i<cards.Count()/2;i++)
            {
                CardDTO hgCard = this.GetHigherPairInPairsList(cards);
                newCards.Add(hgCard); // 1ère carte de la paire
                hgCard = this.GetHigherPairInPairsList(cards);
                newCards.Add(hgCard); // 2ème carte de la paire
                cards = this.DeletePairInList(cards, hgCard);
            }
            return newCards;
        }
        private List<CardDTO> DeletePairInList(List<CardDTO> pairs, CardDTO toDel)
        {
            List<CardDTO> newPairs = new List<CardDTO>();
            foreach(CardDTO pair in pairs)
            {
                if(toDel.Abbreviation.Last() != pair.Abbreviation.Last()) { newPairs.Add(pair); }
            }
            return newPairs;
        }
        private CardDTO GetHigherPairInPairsList(List<CardDTO> pairs)
        {
            string typePairs = "akqjt98765432";
            CardDTO hgPair = pairs[0];
            int posHgPair = typePairs.IndexOf(hgPair.Abbreviation.Last());
            foreach (CardDTO pair in pairs)
            {
                int posHgPairCrt = typePairs.IndexOf(pair.Abbreviation.Last());
                if (posHgPairCrt < posHgPair) { posHgPair = posHgPairCrt; }
            }
            return hgPair;
        }
        private List<CardDTO> GetPairs(List<CardDTO> cards)
        {
            string typePairs = "akqjt98765432";
            List<CardDTO> pairs = new List<CardDTO>();
            List<int> nbValues = this.GetNbOfCardsPerValues(cards);

            for (int i=0; i < 13; i++) { 
                if (nbValues[i] == 2) { 
                    foreach(CardDTO c in cards)
                    {
                        if(c.Abbreviation.Last() == typePairs[i])
                        {
                            pairs.Add(c);
                        }
                    }
                } 
            }
            return pairs;
        }
        private List<CardDTO> GetThreeOfAKind(List<CardDTO> cards)
        {
            string typeThrees = "akqjt98765432";
            List<CardDTO> threes = new List<CardDTO>();
            List<int> nbValues = this.GetNbOfCardsPerValues(cards);

            foreach (int i in nbValues)
            {
                if (i == 3)
                {
                    threes.Add(cards.Single(p => p.Abbreviation.Last() == typeThrees[i]));
                }
            }
            return threes;
        }
        private int CompareTwoPairs(int idP1, int idP2, List<CardDTO> pairsP1, List<CardDTO> pairsP2)
        {
            string typePairs = "akqjt98765432";
            int posPairP1 = typePairs.IndexOf(pairsP1[0].Abbreviation.Last());
            int posPairP2 = typePairs.IndexOf(pairsP2[0].Abbreviation.Last());
            if(posPairP1 < posPairP2) { return idP1; }
            if(posPairP2 < posPairP1) { return idP2; }
            return -1;
        }
        private int CompareTwoHigherCard(int idP1, int idP2, List<int> NbOfCardsPerValueP1, List<int> NbOfCardsPerValueP2)
        {
            for (int i=0; i<7; i++)
            {
                if (NbOfCardsPerValueP1[i]>0 && NbOfCardsPerValueP2[i] == 0) { return idP1; }
                if (NbOfCardsPerValueP1[i]==0 && NbOfCardsPerValueP2[i] > 0) { return idP2; }
            }
            return -1;
        }
        private int CompareTwoCarre(int idP1, int idP2, List<int> NbOfCardsPerValueP1, List<int> NbOfCardsPerValueP2)
        {
            int posHigherCardP1 = NbOfCardsPerValueP1.IndexOf(4);
            int posHigherCardP2 = NbOfCardsPerValueP2.IndexOf(4);
            return (posHigherCardP1 < posHigherCardP2) ? idP1 : idP2;
        }
        private int CompareTwoQuinte(int idP1, int idP2, List<CardDTO> cardsP1, List<CardDTO> cardsP2)
        {
            string typeQuinte = "akqjt98765432";
            string quinteP1 = this.WhichQuinte(cardsP1);
            string quinteP2 = this.WhichQuinte(cardsP2);
            int posHigherCardP1 = typeQuinte.IndexOf(quinteP1[0]);
            int posHigherCardP2 = typeQuinte.IndexOf(quinteP2[0]);
            if (posHigherCardP1 < posHigherCardP2) { return idP1; }
            if (posHigherCardP2 < posHigherCardP1) { return idP2; }
            return -1;
        }
        private int whichHandLevel(List<CardDTO> sevenCards)
        {
            int handLvl = 0;
            List<int> NbOfCardsPerFamily = this.GetNbOfCardsPerFamily(sevenCards);
            int MaxNbOfCardsPerFamily = NbOfCardsPerFamily.Max();
            List<int> NbOfCardsPerValue = this.GetNbOfCardsPerValues(sevenCards);
            int MaxNbOfCardsPerValue =NbOfCardsPerValue.Max();
            string aQuinte = this.WhichQuinte(sevenCards);
            int nbPairs = this.HowManyPairs(NbOfCardsPerValue);

            if (MaxNbOfCardsPerFamily >= 5)
            {
                if ( aQuinte != "")
                {
                    if (aQuinte[0] == 'a'){handLvl = 10;}       // Vérification level 10 - Quinte Flush Royale
                    else{handLvl = 9;}                          // Vérification level 9 - Quinte Flush
                }
                else{handLvl = 6;}                              // Vérification level 6 - Couleur
            }else{
                if(aQuinte != ""){handLvl = 5;}                 // Vérification level 5 - Quinte
                else
                {
                    if(MaxNbOfCardsPerValue == 4){handLvl = 8;} // Vérification level 8 - Carré
                    else
                    {
                        if (this.IsThereAThreeOfAKind(NbOfCardsPerValue))
                        {
                            if (this.IsThereAPair(NbOfCardsPerValue))
                            {handLvl = 7;}                      // Vérification level 7 - Full
                            else { handLvl = 4;}                // Vérification level 4 - Brelan
                        }
                        else
                        {
                            if(nbPairs == 0) { handLvl = 1;}    // Vérification level 1 - Carte haute
                            else
                            {
                                if(nbPairs >= 2){handLvl = 3;}  // Vérification level 3 - Double paire
                                else { handLvl = 2;}            // Vérification level 2 - Paire
                            }
                        }
                    }
                }
            }
            return handLvl;
        }
        private int HowManyPairs(List<int> NbOfCardsPerValue)
        {
            int cptPairs = 0;
            foreach(int i in NbOfCardsPerValue) { if(i == 2) { cptPairs++; } }
            return cptPairs;
        }
        private Boolean IsThereAPair(List<int> NbOfCardsPerValue)
        {
            return NbOfCardsPerValue.Contains(2);
        }
        private Boolean IsThereAThreeOfAKind(List<int> NbOfCardsPerValue)
        {
            return NbOfCardsPerValue.Max() == 3;
        }
        private string WhichQuinte(List<CardDTO> cards)
        {
            string quinte = "";
            string typeQuinte = "akqjt98765432";
            string strValues = "";
            List<int> nbValues = this.GetNbOfCardsPerValues(cards);
            foreach (int nb in nbValues)
            {
                if(nb > 0) { strValues += "1"; } else { strValues += "0"; }
            }
            Boolean isThereAQuinte = strValues.Contains("11111");
            if (isThereAQuinte)
            {
                int pos = strValues.IndexOf("11111");
                quinte = typeQuinte.Substring(pos,5);
            }
            return quinte;
        }
        private List<CardDTO> GetCardsOfOneFamily(List<CardDTO> cards,char family)
        {
            List<CardDTO> result = new List<CardDTO>();
            return (List<CardDTO>)cards.Select(c => c.Abbreviation.ToString().First() == family);
        }
        private List<int> GetNbOfCardsPerFamily(List<CardDTO> cards)
        {
            // Récupération du nombre de cartes dans une main pour chaque famille
            // Ordre des totaux -> Hearts - Diamonds - Clubs - Spades
                List<char> cardFamilies = new List<char>() { 'h', 'd', 'c', 's' };
                List<int> nbCardFamilies = new List<int>() { 0, 0, 0, 0 };
                string strFamilies = this.GetFirstCharOfCardAbbreviation(cards);
                foreach (char c in strFamilies)
                {
                    int pos = cardFamilies.FindIndex(x => x == c);
                    nbCardFamilies[pos] += 1;
                }
                return nbCardFamilies;
        }
        private List<int> GetNbOfCardsPerValues(List<CardDTO> cards)
        {
            // Récupération du nombre de cartes dans une main pour chaque valeur de carte
            // Ordre des totaux -> Ace - King - Queen - Jack - Ten - 9 - 8 - 7 - 6 - 5 - 4 - 3 - 2
                List<char> cardValues = new List<char>() { 'a','k','q','j','t','9','8','7','6','5','4','3','2'};
                List<int> nbCardValues = new List<int>() { 0,0,0,0,0,0,0,0,0,0,0,0,0};
                string strValues = this.GetLastCharOfCardAbbreviation(cards);
                foreach (char c in strValues)
                {
                    int pos = cardValues.FindIndex(x => x == c);
                    nbCardValues[pos] += 1;
                }
                return nbCardValues;
        }
        private string GetFirstCharOfCardAbbreviation(List<CardDTO> cards)
        {
            string firstLetters = "";
            foreach (CardDTO card in cards)
            {
                firstLetters += card.Abbreviation.First();
            }
            return firstLetters;
        }
        private string GetLastCharOfCardAbbreviation(List<CardDTO> cards)
        {
            string lastLetters = "";
            foreach (CardDTO card in cards)
            {
                lastLetters += card.Abbreviation.Last();
            }
            return lastLetters;
        }
        private void SaveHand(CurrentHandDTO hand)
        {
            if (this._trHands != null)
            {
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).TableNr = hand.TableNr;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).StartedOn = hand.StartedOn;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).FinishedOn = hand.FinishedOn;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).Pot = hand.Pot;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).Players = hand.Players;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).SeatNrPlayerToPlay = hand.SeatNrPlayerToPlay;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).SeatNrButton = hand.SeatNrButton;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).SeatNrSmallBlind = hand.SeatNrSmallBlind;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).SeatNrBigBlind = hand.SeatNrBigBlind;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).CardsPack = hand.CardsPack;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).Flop = hand.Flop;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).Turn = hand.Turn;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).River = hand.River;
                    this._trHands.Single(h => h.TournamentId == hand.TournamentId && h.Guid == hand.Guid).HandHistory = hand.HandHistory;

            }
        }

        private CurrentHandDTO GetHand(int trId)
        {
            return this._trHands.Single(h => h.TournamentId == trId);
        }

        public CurrentHandDTO MovingButton(CurrentHandDTO hand)
        {
            int backup = hand.SeatNrButton;
            hand.SeatNrButton = hand.SeatNrBigBlind;
            hand.SeatNrSmallBlind = hand.SeatNrBigBlind;
            hand.SeatNrBigBlind = backup;
            return hand;
        }
        private CurrentHandDTO TakingBlinds(CurrentHandDTO hand)
        {
            List<PlayerDTO> listPl = this._trPlayers.Single(d => d.TournamentId == hand.TournamentId).Players.ToList();
            int stackSB = listPl.Single(p => p.Id == hand.SeatNrSmallBlind).Stack;
            int crtSB = this._trList.Tournaments.Single(t => t.Id == hand.TournamentId).CurrentSmallBlind;
            int newCrtSB = (stackSB - crtSB<0) ? stackSB : crtSB; // ALL IN
            int newStackSB = stackSB - newCrtSB;
            this.UpdateFieldPlayerDTO("stack", newStackSB, hand.TournamentId, hand.SeatNrSmallBlind);
            this.UpdateFieldPlayerDTO("moneyInPot",newCrtSB,hand.TournamentId, hand.SeatNrSmallBlind);
            hand.Pot += newCrtSB;
            int stackBB = listPl.Single(p => p.Id == hand.SeatNrBigBlind).Stack;
            int crtBB = this._trList.Tournaments.Single(t => t.Id == hand.TournamentId).CurrentBigBlind;
            int newCrtBB = (stackBB - crtBB<0) ? stackBB : crtBB; // ALL-IN
            int newStackBB = stackBB - newCrtBB;
            this.UpdateFieldPlayerDTO("stack", newStackBB, hand.TournamentId, hand.SeatNrBigBlind);
            this.UpdateFieldPlayerDTO("moneyInPot",newCrtBB,hand.TournamentId, hand.SeatNrBigBlind);
            hand.Pot += newCrtBB;
            return hand;
        }
        private void UpdateFieldPlayerDTO(string field, int fieldValue, int trId, int playerId)
        {
            if(field == "stack") { this._trPlayers.Single(d => d.TournamentId == trId).Players.Single(p => p.Id == playerId).Stack = fieldValue; }
            if(field == "moneyInPot") { this._trPlayers.Single(d => d.TournamentId == trId).Players.Single(p => p.Id == playerId).MoneyInPot += fieldValue; }
            if(field == "turnToPlay") { this._trPlayers.Single(d => d.TournamentId == trId).Players.Single(p => p.Id == playerId).TurnToPlay = fieldValue==1; }
            if(field == "sittingAtTable") { this._trPlayers.Single(d => d.TournamentId == trId).Players.Single(p => p.Id == playerId).SittingAtTable = fieldValue; }
        }
        private void ShuffleCards(CurrentHandDTO hand)
        {
            List<ShuffleCardsDTO> listShuf = new List<ShuffleCardsDTO>();
            List<string> cardsValues = new List<string>() { "ace", "king", "queen", "jack","ten","nine","eight","seven","six","five","four","three","two" };
            List<string> cardsFamilies = new List<string>() { "hearts", "clubs", "diamonds", "spades" };
            string cardsShortcuts = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int cpt = 0;
            foreach (char c in cardsShortcuts)
            {
                CardDTO aCard = new CardDTO(cardsValues[cpt%13], cardsFamilies[(cpt - cpt%13)/13], ((CardsEnum)((byte)c)).ToString());
                ShuffleCardsDTO newShufDTO = new ShuffleCardsDTO(aCard);
                listShuf.Add(newShufDTO);
                cpt++;
            }
            hand.CardsPack.Pack = listShuf.OrderBy(c => c.shuffleGuid).Select(l => l.Card).ToList();
            this.SaveHand(hand);            
        }
        private void BurnCard(CurrentHandDTO hand)
        {
            hand.CardsPack.IndexCrtCard++;
            this.SaveHand(hand);
        }
        private CardDTO DealCard(CurrentHandDTO hand)
        {
            CardDTO card = hand.CardsPack.Pack.ToList()[hand.CardsPack.IndexCrtCard];
            hand.CardsPack.IndexCrtCard++;
            this.SaveHand(hand);
            return card;
        }
        private void DealPrivateCards(CurrentHandDTO hand)
        {
            List<CardDTO> cardsSB = new List<CardDTO>();
            cardsSB.Add(this.DealCard(hand));
            cardsSB.Add(this.DealCard(hand));
            List<CardDTO> cardsBB = new List<CardDTO>();
            cardsBB.Add(this.DealCard(hand));
            cardsBB.Add(this.DealCard(hand));
            this._trPlayers.Single(d => d.TournamentId == hand.TournamentId).Players.Single(p => p.Id == hand.SeatNrSmallBlind).PrivateCards = cardsSB;
            this._trPlayers.Single(d => d.TournamentId == hand.TournamentId).Players.Single(p => p.Id == hand.SeatNrBigBlind).PrivateCards = cardsBB;
        }
        private void DealFlop(CurrentHandDTO hand)
        {
            this.BurnCard(hand);
            List<CardDTO> cardsFlop = new List<CardDTO>();
            cardsFlop.Add(this.DealCard(hand));
            cardsFlop.Add(this.DealCard(hand));
            cardsFlop.Add(this.DealCard(hand));
            hand.Flop = cardsFlop;
            this.SaveHand(hand);
        }
        private void DealTurn(CurrentHandDTO hand)
        {
            this.BurnCard(hand);
            hand.Turn = this.DealCard(hand);
            this.SaveHand(hand);
        }
        private void DealRiver(CurrentHandDTO hand)
        {
            this.BurnCard(hand);
            hand.River = this.DealCard(hand);
            this.SaveHand(hand);
        }

        private void AddToHandHistory(CurrentHandDTO hand)
        {

        }

    }
}
