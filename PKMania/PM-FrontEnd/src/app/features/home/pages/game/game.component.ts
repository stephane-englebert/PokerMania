import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { LoggedUserModel } from '../../../tools/login/models/loggedUser';
import { LoginService } from '../../../tools/login/services/login.service';
import { CurrentHand } from '../../../tools/tournaments/models/currentHand';
import { Player } from '../../../tools/tournaments/models/player';
import { RankedPlayer } from '../../../tools/tournaments/models/rankedPlayer';
import { Tournament } from '../../../tools/tournaments/models/tournament';
import { TournamentsTypes } from '../../../tools/tournaments/models/tournamentsTypes';
import { GameService } from '../../../tools/tournaments/services/game.service';
import { TournamentsService } from '../../../tools/tournaments/services/tournaments.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  _hubConnection: HubConnection;
  isLogged: boolean = false;
  loggedUser!: LoggedUserModel;
  hubStarted!: boolean;
  formGroupRegTourn!: FormGroup;
  userId!: number;
  nbAllPlayers: number = 0;     // nombre de participants au tournoi
  nbPlayers: number = 2;        // nombre de joueurs à la table
  menuOption: number = 1;       // option du menu latéral active
  trStatus: string = "unknown"; // statut du tournoi 
  msgToPlayer: string = "Vous ne pouvez pas rejoindre ce tournoi.";
  seatsPlayersIds: number[] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];   // Id des joueurs assis à table
  tablePlayers: Player[] = [];              // infos complètes sur les joueurs assis à table (infos 'vivantes' uniquement via le Hub)
  rankedPlayers: RankedPlayer[] = [];       // infos sommaires sur les joueurs (classement général)
  rankedTablePlayers: RankedPlayer[] = [];  // infos sommaires sur les joueurs assis à table (classement table)
  infosTournament!: Tournament;             // infos courantes sur le tournoi en cours (infos 'vivantes' uniquement via le Hub)
  tournamentType!: TournamentsTypes;      // infos générales sur les types de tournois existants
  avatarPlayer!: Player;        // infos complètes sur le joueur connecté (Heads Up)
  avatarOpponent!: Player;      // infos complètes sur son adversaire (Heads Up)
  roomJoined: Boolean = false;
  currentHand!: CurrentHand;
  flopCards: string[] = ["", "", ""];
  turnCard: string = "";
  riverCard: string = "";
  privateCard1: string = "";
  privateCard2: string = "";
  pot: number = 0;
  timerPlayer: number = 0;
  timerOpponent: number = 0;

  constructor(
      private _loginService: LoginService,
      private _tournamentsService: TournamentsService,
      private _gameService: GameService,
      private _formBuilder: FormBuilder,
      private _router: Router,
      private _toastr: ToastrService
  ) {

      // Récupération infos user courant
        this.userId = parseInt(localStorage.getItem("id") as string);
        this._loginService.userInfos.subscribe({
          next: (response: LoggedUserModel) => {
            this.loggedUser = response;
          }
        });
        this._loginService.isLogged.subscribe({
          next: (response: boolean) => {
            this.isLogged = response;
          }
        });
        this.isLogged = this._loginService.userLogged();

    // Initiation de la communication via Hub
        this.hubStarted = false;
        this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
        this._hubConnection.start().then(() => {
          this.hubStarted = true;
          // Vérification accès à l'interface de jeu autorisé
          this._gameService.getPlayerCurrentTournamentId().subscribe({
            next: (data) => {
              if (data > 0) {
                // Si ok -> Récupération des infos sur le tournoi
                this._hubConnection.send("GetInfosTournament", data);
                this._hubConnection.send("GetTournamentPlayers", data);
                this._hubConnection.send("GetTournamentRankedPlayers", data);
                this.msgToPlayer = "Le tournoi démarrera lorsque tous les joueurs auront rejoint le lobby.";
              } else {
                this.msgToPlayer = "Vous n'avez aucun tournoi en cours.";
              }
            }
          });
          this._hubConnection.on('msgToAll', (msg) => { console.log(msg); });
          this._hubConnection.on('msgToCaller', (msg) => { console.log(msg); });
          this._hubConnection.on('sendInfosTournament', (infos) => {
            this.infosTournament = infos;
            this.trStatus = this.infosTournament.status;
            this.nbPlayers = this.infosTournament.registrationsNumber;
            this._hubConnection.send("GetTournamentType", this.infosTournament.tournamentType);
            if (!this.roomJoined) { this._hubConnection.send("JoinRoom", "tr" + this.infosTournament.id, this.infosTournament.id, this.userId); }
          });
          this._hubConnection.on('sendTournamentType', (data) => {
            this.tournamentType = data;
          });
          this._hubConnection.on('sendTournamentPlayers', (data) => {
            this.tablePlayers = data[0];
            if (this.tablePlayers[0].id == this.userId) {
              this.avatarPlayer = this.tablePlayers[0];
              this.avatarOpponent = this.tablePlayers[1];
            } else {
              this.avatarPlayer = this.tablePlayers[1];
              this.avatarOpponent = this.tablePlayers[0];
            }
            this.privateCard1 = this.avatarPlayer.privateCards[0].abbreviation;
            this.privateCard2 = this.avatarPlayer.privateCards[1].abbreviation;
            this.timerPlayer = this.avatarPlayer.bonusTime;
            this.timerOpponent = this.avatarOpponent.bonusTime;
            console.log(this.tablePlayers);
          });
          this._hubConnection.on('sendTournamentRankedPlayers', (data) => {
            this.rankedPlayers = data[0];
            this.rankedTablePlayers = data[0];
            this.nbAllPlayers = this.rankedPlayers.length;
          });
          this._hubConnection.on('roomJoined', () => this.roomJoined = true);
          this._hubConnection.on('sendHand', (hand) => {
            console.log(hand);
            this.currentHand = hand;
            this.flopCards[0] = this.currentHand.flop[0].abbreviation;
            this.flopCards[1] = this.currentHand.flop[1].abbreviation;
            this.flopCards[2] = this.currentHand.flop[2].abbreviation;
            this.turnCard = this.currentHand.turn.abbreviation;
            this.riverCard = this.currentHand.river.abbreviation;
            this.pot = this.currentHand.pot;
          })
        });
  }
    
  ngOnInit(): void {
  }

  switchMenu(nbMenu: number) {
    this.menuOption = nbMenu;
  }

  isMenuOptionActive(nbOption: number) {
    if (this.menuOption == nbOption) { return "active" } else { return ""; }
  }

  raiseHand() {
    console.log("Raise hand!!!");
  }
   
  chooseRaiseAmount(amount: number) {
    console.log(amount);
  }

  getRankPlayer(searchTable: RankedPlayer[], playerId: number) {
    if (this.rankedTablePlayers.length > 0) {
      return (searchTable.findIndex(p => p.playerId == playerId)) + 1;
    } else { return 0;}
  }

}
