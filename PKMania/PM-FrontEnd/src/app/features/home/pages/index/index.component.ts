import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
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
  isLogged: boolean = false;
  tournamentsDetails!: TournamentDetails[];
  tabIdRegisteredTr: number[] = [];
  loggedUser!: LoggedUserModel;
  hubStarted!: boolean;
  formGroupRegTourn!: FormGroup;
  userId!: number;

  constructor(
    private _loginService: LoginService,
    private _tournamentsService: TournamentsService,
    private _formBuilder: FormBuilder
  ) {
    this.userId = parseInt(localStorage.getItem("id") as string);
    this._loginService.userInfos.subscribe({
      next: (response: LoggedUserModel) => {
        this.loggedUser = response;
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
      console.log("démarrage hub");
      this._hubConnection.send('getTournamentsDetails');
      this._hubConnection.send('getIdRegisteredTournaments', this.userId);
      this._hubConnection.on('msgToAll', (msg) => { console.log(msg); });
      this._hubConnection.on('sendTournamentsDetails', (details) => this.tournamentsDetails = details);
      this._hubConnection.on('sendIdRegisteredTournaments', (data) => { this.tabIdRegisteredTr = data; });
    });
  }

  ngOnInit(): void {
    this.formGroupRegTourn = this._formBuilder.group({ TournamentId: [null] });
    console.log(this);
  }

  isRegistered(trId: number) {
    if (this.tabIdRegisteredTr.indexOf(trId) > -1) { return true; } else { return false; }
  }

  registerTourn(trId: number) {
    console.log(trId);
    this.formGroupRegTourn.reset({ TournamentId: trId });
    this._tournamentsService.registerTournament(this.formGroupRegTourn.value).subscribe({
      next: () => {
        this._hubConnection.send('UpdateNecessary', 'registrations');
        this._hubConnection.send('getIdRegisteredTournaments', this.userId);
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

  joinTable() {

  }

  createTournament() {
    this._hubConnection.send('CreateTournament', new Date('2022-11-18 03:30:00'), 'Tournoi à 1 table', 2);
    this._hubConnection.send('UpdateNecessary', "tournaments");
  }

  startTournament(trId: number) {
    console.log("tournoi " + trId + " démarré...");
  }

  deleteTournament(trId: number) {
    this._hubConnection.send('DeleteTournament', trId);
    this._hubConnection.send('UpdateNecessary', "tournaments");
  }
}
