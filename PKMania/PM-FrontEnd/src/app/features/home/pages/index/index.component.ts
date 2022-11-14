import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { GlobalConst } from '../../../tools/globals/globals';
import { LoginService } from '../../../tools/login/services/login.service';
import { Tournament } from '../../../tools/tournaments/models/tournament';
import { TournamentsDetails } from '../../../tools/tournaments/models/tournamentsDetails';
import { TournamentsList } from '../../../tools/tournaments/models/tournamentsList';
import { TournamentsTypes } from '../../../tools/tournaments/models/tournamentsTypes';
import { TournamentsService } from '../../../tools/tournaments/services/tournaments.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {

  _hubConnection: HubConnection;
  isLogged: boolean;
  tournamentsList!: Tournament[];
  tournamentsTypes!: TournamentsTypes[];
  tournamentsDetails!: TournamentsDetails[];


  constructor(
    private _httpClient: HttpClient,
    private GBconst: GlobalConst,
    private _tournamentsService: TournamentsService,
    private _loginService: LoginService
  ) {
    this.isLogged = this._loginService.userLogged();
    this._tournamentsService.trTypes.subscribe({
      next: (response: TournamentsTypes[]) => {
        this.tournamentsTypes = response;
      }
    });
    this._tournamentsService.trList.subscribe({
      next: (response: TournamentsList) => {
        this.tournamentsList = response.tournaments;
      }
    });
    this._tournamentsService.trDetails.subscribe({
      next: (response: TournamentsDetails[]) => {
        this.tournamentsDetails = response;
      }
    });
    this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
    this._hubConnection.start().then(() => {
      this._hubConnection.send('SendMsgToAll', "Demande des infos tournois actifs depuis la page index");
      this._hubConnection.send('getInfosActivTournaments');
      this._hubConnection.on('sendInfosActivTournaments', () => this._tournamentsService.getActivTournamentsList());

    });
  }

  ngOnInit(): void {
  }

  registerTourn() {

  }

  unregisterTourn() {

  }

  joinTable() {

  }

}
