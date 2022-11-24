import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { GlobalConst } from '../../globals/globals';
import { RankedPlayer } from '../models/rankedPlayer';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  // Observable
  trRankedPlayers: Subject<RankedPlayer> = new Subject<RankedPlayer>();

  // Local variable
  private tournRankedPlayers!: RankedPlayer;

  constructor(
    private GBconst: GlobalConst,
    private _http: HttpClient,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {
  }

  getPlayerCurrentTournamentId() {
    return this._http.get<number>(this.GBconst.API_REGISTRATIONS + '/crt');
  }

}
