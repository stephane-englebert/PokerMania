import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { GlobalConst } from '../../globals/globals';
import { TranslateService } from '@ngx-translate/core';
import { TournamentsList } from '../models/tournamentsList';
import { Subject } from 'rxjs';
import { TournamentsTypes } from '../models/tournamentsTypes';
import { TournamentDetails } from '../models/tournamentDetails';

@Injectable({
  providedIn: 'root'
})
export class TournamentsService {

  // Observable
  trList: Subject<TournamentsList> = new Subject<TournamentsList>();
  trTypes: Subject<TournamentsTypes[]> = new Subject<TournamentsTypes[]>();
  trDetails: Subject<TournamentDetails[]> = new Subject<TournamentDetails[]>();

  // local variable
  private tournList!: TournamentsList;
  private tournTypes!: TournamentsTypes[];
  private tournDetails: TournamentDetails[] = [];

  constructor(
    private GBconst: GlobalConst,
    private _http: HttpClient,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {
  }
  
  registerTournament(value: any) {
    return this._http.post<void>(this.GBconst.API_REGISTRATIONS, value);
  }
  
  unregisterTournament(value: any) {
    return this._http.delete<void>(this.GBconst.API_REGISTRATIONS+'/'+ value);
  }

  getActivTournamentsList() {
    this._http.get<TournamentsList>(this.GBconst.API_TOURNAMENTS_LIST).subscribe({
      next: (response: TournamentsList) => {
        this.tournList = response;
        this.trList.next(this.tournList);
        this.getAllTournamentsTypes();
      },
      error: (err) => {
        this.translate.get(err.error).subscribe(toastMsg => {
          this.toastr.error(toastMsg);
        });
      },
      complete: () => {

      }
    });
  }

  getAllTournamentsTypes(){
    this._http.get<TournamentsTypes[]>(this.GBconst.API_TOURNAMENTS_TYPES).subscribe({
      next: (response: TournamentsTypes[]) => {
        this.trTypes.next(response);
        this.tournTypes = response;
        this.getTournamentsDetails();
      },
      error: (err) => {
        this.translate.get(err.error).subscribe(toastMsg => {
          this.toastr.error(toastMsg);
        });
      },
      complete: () => {

      }
    });
  }

  getTournamentsDetails() {
    if (this.tournList != null && this.tournTypes != null) {
      this.tournList.tournaments.forEach(t => {
        const trType: TournamentsTypes[] = this.tournTypes.filter(x => x.id == t.tournamentType);
        const obj: TournamentDetails = {
          id: t.id,
          status: t.status,
          name: t.name,
          buyIn: trType[0].buyIn,
          rebuy: trType[0].rebuy,
          rebuyLevel: trType[0].rebuyLevel,
          prizePool: t.prizePool,
          playersPerTable: trType[0].playersPerTable,
          maxPaidPlaces: trType[0].maxPaidPlaces,
          registrationsNumber: t.registrationsNumber,
          minPlayers: trType[0].minPlayers,
          maxPlayers: trType[0].maxPlayers,
          startingStack: trType[0].startingStack,
          levelsDuration: trType[0].levelsDuration,
          gainsSharingNr: t.gainsSharingNr,
          startedOn: t.startedOn,
          finishedOn: t.finishedOn,
          tournamentType: t.tournamentType,
          realPaidPlaces: t.realPaidPlaces
        };
        this.tournDetails.push(obj);
      });
      this.trDetails.next(this.tournDetails);
    }
  }

  canJoinLobby(trId: number, playerId: number) {
    return this._http.get<boolean>(this.GBconst.API_TOURNAMENT + '/lobby/' + trId + '/' + playerId);
  }

  StillFreePlacesForTournament(trId: number) {
    return this._http.get<boolean>(this.GBconst.API_REGISTRATIONS + '/full/' + trId);
  }

  OngoingTournamentsForOnePlayer() {
    return this._http.get<boolean>(this.GBconst.API_TOURNAMENTS_LIST + '/ongoing');
  }

}
