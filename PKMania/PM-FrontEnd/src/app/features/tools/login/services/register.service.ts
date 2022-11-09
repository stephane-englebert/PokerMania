import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GlobalConst } from '../../globals/globals';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(
    private _httpClient: HttpClient,
    private GBconst: GlobalConst
  ) { }

  addMember(value: any) {
    return this._httpClient.post<void>(this.GBconst.API_MEMBERS,value);
  }
}
