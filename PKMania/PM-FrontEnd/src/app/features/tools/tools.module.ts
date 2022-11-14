import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ToolsRoutingModule } from './tools-routing.module';
import { LoginComponent } from './login/login.component';
import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RegisterComponent } from './register/register.component';
import { FormRegisterComponent } from './register/pages/form-register/form-register.component';
import { TranslateModule } from '@ngx-translate/core';
import { HubComponent } from './hub/hub.component';
import { TournamentsComponent } from './tournaments/tournaments.component';
import { TableModule } from 'primeng/table';


@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    FormRegisterComponent,
    HubComponent,
    TournamentsComponent
  ],
  imports: [
    CommonModule,
    ToolsRoutingModule,
    PasswordModule,
    InputTextModule,
    ButtonModule,
    ReactiveFormsModule,
    FormsModule,
    TranslateModule,
    TableModule,
  ]
})
export class ToolsModule { }
