import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { GlobalConst } from '../../globals/globals';
import { TranslateService } from '@ngx-translate/core';
import { TournamentsList } from '../models/tournamentsList';
import { Subject } from 'rxjs';
import { TournamentsTypes } from '../models/tournamentsTypes';
import { TournamentsDetails } from '../models/tournamentsDetails';

@Injectable({
  providedIn: 'root'
})
export class TournamentsService {

  // Observable
  trList: Subject<TournamentsList> = new Subject<TournamentsList>();
  trTypes: Subject<TournamentsTypes[]> = new Subject<TournamentsTypes[]>();
  trDetails: Subject<TournamentsDetails[]> = new Subject<TournamentsDetails[]>();

  // local variable
  private tournList!: TournamentsList;
  private tournTypes!: TournamentsTypes;
  private tournDetails!: TournamentsDetails[];

  constructor(
    private GBconst: GlobalConst,
    private _http: HttpClient,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {
    this.getActivTournamentsList();
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
        this.tournDetails.push(
          new (
            id = t.id;
            status = t.status;
          name: ;
          buyIn: ;
          rebuy: ;
          rebuyLevel: ;
          prizePool:
            playersPerTable: ;
        maxPaidPlaces: ;
        registrationsNumber: number;
        minPlayers: number;
        maxPlayers: number;
        startingStack: number;
        levelsDuration: number;
        gainsSharingNr: number;
        startedOn: string;
        finishedOn: string;
        tournamentType: number;
        realPaidPlaces: number;
        )
      );
    }
      this.trDetails.next(this.tournDetails);
    }
  }

}
