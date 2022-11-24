import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { LoggedUserModel } from '../../../tools/login/models/loggedUser';
import { LoginService } from '../../../tools/login/services/login.service';
import { TournamentDetails } from '../../../tools/tournaments/models/tournamentDetails';
import { TournamentsService } from '../../../tools/tournaments/services/tournaments.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  _hubConnection: HubConnection ;
  isLogged: boolean;
  tournamentsDetails!: TournamentDetails[];
  tabIdRegisteredTr!: number[];
  loggedUser!: LoggedUserModel;
  hubStarted!: boolean;
  formGroupRegTourn!: FormGroup;
  userId!: number;
  statusTr: string = "";
  trDetails!: TournamentDetails;

  constructor(
    private _loginService: LoginService,
    private _tournamentsService: TournamentsService,
    private _formBuilder: FormBuilder,
    private _router: Router
  ) {
    this.userId = parseInt(localStorage.getItem("id") as string);
    this._loginService.userInfos.subscribe({
      next: (response: LoggedUserModel) => {
        this.loggedUser = response;
        this.userId = this.loggedUser.loggedMember.id;
        this._hubConnection.send('getIdRegisteredTournaments', this.userId);
        //this._hubConnection.on('sendTournamentPlayers', (details) => { console.log(details); });
        //this._hubConnection.on('sendTournamentRankedPlayers', (details) => { if (this.isLogged) { console.log(details); } });
      }
    });
    this._loginService.isLogged.subscribe({
      next: (response: boolean) => {
        this.isLogged = response;
      }
    });
    this.isLogged = this._loginService.userLogged();
    this.hubStarted = false;
    this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
    this._hubConnection.start().then(() => {
      this.hubStarted = true;
      this._hubConnection.send('getTournamentsDetails');
      if (this.isLogged) {
        this._hubConnection.send('getIdRegisteredTournaments', this.userId);
        this._hubConnection.on('pleaseJoinTr', (trId) => {
          if (this.isRegistered(trId)) {
            this._router.navigate(['/home/game']);
          }
        });
      }
      this._hubConnection.on('msgToAll', (msg) => { console.log(msg); });
      this._hubConnection.on('msgToCaller', (msg) => { console.log(msg); });
      this._hubConnection.on('sendTournamentsDetails', (details) => this.tournamentsDetails = details);
      this._hubConnection.on('sendIdRegisteredTournaments', (data) => {
        this.tabIdRegisteredTr = data;
        this.checkOngoingTr();
      });
      //this._hubConnection.on('navigateGame', () => {
      //  this._router.navigate(['/home/game']);
      //  console.log("navigate -> ligne 69");
      //});
      //this._hubConnection.on('pleaseJoinTr', (tr) => {
      //  if (this.isRegistered(tr)) {
      //    this._router.navigate(['/home/game']);
      //    console.log("navigate -> ligne 74");
      //  }
      //});

      //
    });
  }

  ngOnInit(): void {
    this.formGroupRegTourn = this._formBuilder.group({ TournamentId: [null] });
  }

  checkOngoingTr() {
    this._tournamentsService.OngoingTournamentsForOnePlayer().subscribe({
      next: (response: boolean) => {
        if (response) {
          this._router.navigate(['/home/game']);
          console.log("navigate -> ligne 88 [" + this.userId + "]");
        }
      }
    });
  }

  isRegistered(trId: number) {
    if (this.isLogged && this.tabIdRegisteredTr != null) {
      if (this.tabIdRegisteredTr.indexOf(trId) > -1) {
        return true;
      } else { return false; }
    } else { return false; }
  }

  registerTourn(trId: number) {
    this.formGroupRegTourn.reset({ TournamentId: trId });
    this._tournamentsService.registerTournament(this.formGroupRegTourn.value).subscribe({
      next: () => {
        this._hubConnection.send('PlayerGetRegisteredToTr', trId, this.userId);
      },
      error: (err) => { console.log(err.message); console.log(err.error); }
    });
  }

  unregisterTourn(trId: number) {
    this._tournamentsService.unregisterTournament(trId).subscribe({
      next: () => {
        this._hubConnection.send('UpdateNecessary', "registrations");
        this._hubConnection.send('getIdRegisteredTournaments', this.userId);
      },
      error: (err) => {
        console.log(err.message);
      }
    });
  }

  joinLobby(trId: number) {    
    this._tournamentsService.canJoinLobby(trId, this.userId).subscribe({
      next: (response: boolean) => {
        if (response) {
          // si ok, signaler au serveur qu'il rejoint le tournoi/lobby
          this._hubConnection.send('PlayerIsJoiningLobby',trId, this.userId);
          //console.log("canJoinLobby -> réponse serveur = " + response);
          this._router.navigate(['/home/game']);
          console.log("navigate -> ligne 126");
        } else {
          console.log("Pas possible de rejoindre le lobby!");
        }
      },
      error: (err) => {
        console.log(err.message);
      }
    });
  }

  createTournament(nbPl: number) {
    if (nbPl == 2) {
      this._hubConnection.send('CreateTournament', new Date(), 'Heads-Up 2 joueurs', 1).then(() => {
        this._hubConnection.send('UpdateNecessary', "tournaments");
      });
    }
  }

  launchTournament(trId: number) {
    this._hubConnection.send('LaunchTournament', trId);
  }

  closeTournament(trId: number) {
    this._hubConnection.send('CloseTournament', trId);
  }

  cleanTournaments() {
    this._hubConnection.send('CleanDatabase');
  }
}
