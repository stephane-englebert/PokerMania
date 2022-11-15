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

  constructor(
    private _loginService: LoginService,
    private _tournamentsService: TournamentsService
  ) {
  }

  ngOnInit(): void {
    this._loginService.isLogged.subscribe({next: (response: boolean) => this.isLogged = response});
    this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
    this._hubConnection.start().then(() => {
      this._hubConnection.send('getTournamentsDetails');
      this._hubConnection.on('sendTournamentsDetails', (details) => this.tournamentsDetails = details);

    });
  }

  registerTourn(trId: number) {
    console.log(trId);
  }

  unregisterTourn() {

  }

  joinTable() {

  }

}
