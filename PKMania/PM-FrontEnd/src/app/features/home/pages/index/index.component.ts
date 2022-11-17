import { Component, OnInit } from '@angular/core';
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

  _hubConnection: HubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
  isLogged: boolean = false;
  tournamentsDetails!: TournamentDetails[];
  tabIdRegisteredTr: number[] = [];
  loggedUser!: LoggedUserModel;

  constructor(
    private _loginService: LoginService,
    private _tournamentsService: TournamentsService
  ) {
    this.isLogged = this._loginService.userLogged();
  }

  ngOnInit(): void {

    this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
    this._hubConnection.start().then(() => {
      console.log("démarrage hub");
      this._hubConnection.send('getTournamentsDetails');
      this._hubConnection.on('msgToAll', (msg) => { console.log(msg); });
      this._hubConnection.on('sendTournamentsDetails', (details) => this.tournamentsDetails = details);
      this._hubConnection.on('sendIdRegisteredTournaments', (data) => { this.tabIdRegisteredTr = data; });

      this._loginService.userInfos.subscribe({
        next: (response: LoggedUserModel) => {
          this.loggedUser = response;
          this._hubConnection.send('getIdRegisteredTournaments', this.loggedUser.loggedMember.id);
          //this._hubConnection.on('sendTournamentPlayers', (details) => { console.log(details); });
          //this._hubConnection.on('sendTournamentRankedPlayers', (details) => { if (this.isLogged) { console.log(details); } });
        }
      });
      this._loginService.isLogged.subscribe({
        next: (response: boolean) => {
          this.isLogged = response;
        }
      });

    });
    console.log(this);
  }

  isRegistered(trId: number) {
    if (this.tabIdRegisteredTr.indexOf(trId) > -1) { return true; } else { return false; }
  }

  registerTourn(trId: number) {
    console.log(trId);
    this._tournamentsService.registerTournament(trId).subscribe({
      next: () => {
        this._hubConnection.send('UpdateNecessary', 'registrations');
        this._hubConnection.send('getIdRegisteredTournaments', this.loggedUser.loggedMember.id);
        console.log("test");
      },
      error: (err) => { console.log(err.message); }
    });
  }

  unregisterTourn(trId: number) {
    this._tournamentsService.unregisterTournament(trId).subscribe({
      next: () => {
        this._hubConnection.send('UpdateNecessary', "registrations");
        this._hubConnection.send('getIdRegisteredTournaments', this.loggedUser.loggedMember.id);
      },
      error: (err) => {
        console.log(err.message);
      }
    });
  }

  joinTable() {

  }

}
