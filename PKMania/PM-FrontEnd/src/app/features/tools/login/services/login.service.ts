import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { LoggedUserModel } from '../models/loggedUser';
import { UserModel } from '../models/user';
import { GlobalConst } from '../../globals/globals';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';

export interface memberInfos{
  Pseudo: string | null;
  IsAdmin: boolean;
  IsLogged: boolean;
  Bankroll: number;
  IsPlaying: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  // Observable
    isLogged: Subject<boolean> = new Subject<boolean>();
    userInfos: Subject<LoggedUserModel> = new Subject<LoggedUserModel>();

  // local variable
    private loggedUser!: LoggedUserModel;
    role: string | undefined;

  constructor(
    private GBconst: GlobalConst,
    private _http: HttpClient,
    private router: Router,
    private toastr: ToastrService,
    private translate: TranslateService
  ) {
    this.initUserLogged();
  }

  ngOnInit(): void{

  }

  loginUser(user: UserModel){
    this._http.post<LoggedUserModel>(this.GBconst.API_TOKEN,user).subscribe({
      next: (response: LoggedUserModel) => {
        this.loggedUser = response;
        localStorage.setItem('token',response.token);
        localStorage.setItem('id',response.loggedMember.id.toString());
        localStorage.setItem('role',response.loggedMember.role);
        localStorage.setItem('pseudo',response.loggedMember.pseudo);
        localStorage.setItem('bankroll',response.loggedMember.bankroll.toString());
        localStorage.setItem('isplaying',response.loggedMember.isPlaying.toString());
        localStorage.setItem('isdisconnected',response.loggedMember.isDisconnected.toString());
        localStorage.setItem('currenttournament',response.loggedMember.currentTournament.toString());
        localStorage.setItem('islogged','true');
        this.userInfos.next(this.loggedUser);
        this.isLogged.next(true);
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

  userLogged(){
    return localStorage.getItem('islogged') == "true";
  }

  initUserLogged() {
    this.loggedUser = ({
      token: "", loggedMember: {
        id: 0,
        role: "",
        pseudo: "",
        email: "",
        bankroll: 0,
        isPlaying: false,
        isDisconnected: true,
        currentTournament: 0
      }
    });
    this.loggedUser.token = localStorage.getItem('token') as string;
    this.loggedUser.loggedMember.id = parseInt(localStorage.getItem('id') as string);
    this.loggedUser.loggedMember.role = localStorage.getItem('role') as string;
    this.loggedUser.loggedMember.pseudo = localStorage.getItem('pseudo') as string;
    this.loggedUser.loggedMember.bankroll = parseInt(localStorage.getItem('bankroll') as string);
    this.loggedUser.loggedMember.isPlaying = localStorage.getItem('isplaying') as string == "true";
    this.loggedUser.loggedMember.isDisconnected = localStorage.getItem('isdisconnected') as string == "true";
    this.loggedUser.loggedMember.currentTournament = parseInt(localStorage.getItem('currenttournament') as string);
    this.userInfos.next(this.loggedUser);
  }

  getInfosUser(){
    if(this.userLogged() ){
      this.userInfos.next(this.loggedUser);
    } else {
      this.initUserLogged();
    }
  }

  logoutUser(){
    localStorage.clear();
    this.isLogged.next(false);
  }
}
