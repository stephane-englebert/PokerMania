import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { LoginService } from '../../../tools/login/services/login.service';
import { TournamentDetails } from '../../../tools/tournaments/models/tournamentDetails';
import { TournamentsService } from '../../../tools/tournaments/services/tournaments.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  _hubConnection: HubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
  isLogged: boolean = false;
  tournamentsDetails!: TournamentDetails[];
  tabIdRegisteredTr: number[] = [];

  constructor(
    private _loginService: LoginService,
    private _tournamentsService: TournamentsService
  ) {
    this.isLogged = this._loginService.userLogged();
  }

  ngOnInit(): void {
    this._loginService.isLogged.subscribe({next: (response: boolean) => this.isLogged = response});
    this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
    this._hubConnection.start().then(() => {
      this._hubConnection.send('getTournamentsDetails');
      this._hubConnection.send('getTournamentPlayers',1);
      this._hubConnection.send('getTournamentRankedPlayers', 1);
      this._hubConnection.send('getIdRegisteredTournaments', 1);
      this._hubConnection.on('msgToAll', (msg) => { console.log(msg); });
      this._hubConnection.on('sendTournamentsDetails', (details) => this.tournamentsDetails = details);
      this._hubConnection.on('sendTournamentPlayers', (details) => {if (this.isLogged){console.log(details);}});
      this._hubConnection.on('sendTournamentRankedPlayers', (details) => { if (this.isLogged) { console.log(details); } });
      this._hubConnection.on('sendIdRegisteredTournaments', (data) => { this.tabIdRegisteredTr = data; });
    });
  }

  isRegistered(trId: number) {
    if (this.tabIdRegisteredTr.indexOf(trId) > -1) { return true; } else { return false; }
  }

  registerTourn(trId: number) {
    console.log(trId);
  }

  unregisterTourn(trId: number) {
    this._tournamentsService.unregisterTournament(trId).subscribe({
      next: () => {
        this._hubConnection.send('UpdateNecessary', "registrations");
      },
      error: (err) => {
        console.log(err.error);
      }
    });
  }

  joinTable() {

  }

}
