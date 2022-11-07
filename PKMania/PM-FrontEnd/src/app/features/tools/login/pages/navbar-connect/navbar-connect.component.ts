import { Component, OnInit } from '@angular/core';
import { FormBuilder,FormGroup,Validators } from '@angular/forms';
import { LoggedUserModel } from '../../models/loggedUser';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-navbar-connect',
  templateUrl: './navbar-connect.component.html',
  styleUrls: ['./navbar-connect.component.css']
})
export class NavbarConnectComponent implements OnInit {

  formGroupLogin!: FormGroup;
  userIdentifier: string ="";
  password: string = "";
  isLogged: boolean;
  loggedUser!: LoggedUserModel;

  constructor(
    private _formBuilder: FormBuilder,
    private _loginService: LoginService
  ) { 
    this.isLogged = this._loginService.userLogged();
  }

  ngOnInit(): void {
    this.formGroupLogin = this._formBuilder.group({
      UserIdentifier: [null, [Validators.required, Validators.maxLength(150)]],
      Password: [null, [Validators.required, Validators.maxLength(30)]]
    });
    this._loginService.isLogged.subscribe({
      next: (res) => {this.isLogged = res;}
    });
    this._loginService.userInfos.subscribe({
      next: (response: LoggedUserModel) => {
        this.loggedUser = response;
      }
    });
    this._loginService.getInfosUser();
  }

  submit(){
     if(this.formGroupLogin.invalid){
       return;
     }
    this._loginService.loginUser(this.formGroupLogin.value);
  }

  logout(){
    this._loginService.logoutUser();
  }

}
