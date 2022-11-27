using Microsoft.AspNetCore.SignalR;
using PM_BLL.Data.DTO.Entities;
using PM_BLL.Data.DTO.Entities.Hubs;
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
                this.startHand(trId, trPlayers.Players);

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
        public void startHand(int trId, IEnumerable<PlayerDTO> trPlayers)
        {
            // créer une nouvelle main
            CurrentHandDTO hand = new CurrentHandDTO();
            hand.TournamentId = trId;
            hand.TableNr = 1;
            hand.StartedOn = DateTime.Now;
            
            // au hasard, choisir le premier joueur à la parole
            Random rand = new Random();
            int firstPl = rand.Next(1,10) % 2;
            int secondPl = (firstPl == 0) ? 1 : 0;

            // placer le bouton (playerId 1er joueur)
            hand.SeatNrButton = trPlayers.ToList()[firstPl].Id;

            // positionner les blinds (SB & BB) à table
            hand.SeatNrSmallBlind = trPlayers.ToList()[firstPl].Id; // playerId 1er joueur
            hand.SeatNrBigBlind = trPlayers.ToList()[secondPl].Id; // playerId 2ème joueur

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

            // distribuer les cartes (joueurs,flop,turn,river)
            this.DealPrivateCards(hand);
            this.DealFlop(hand);
            this.DealTurn(hand);
            this.DealRiver(hand);

            // transmettre infos aux joueurs
            this.SendTrPlayersToRoomPlayers(hand.TournamentId);
            this.SendInfosTournament(trId, Clients.Group("tr" + trId));
            Clients.Group("tr" + hand.TournamentId).SendAsync("sendHand", hand);
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
