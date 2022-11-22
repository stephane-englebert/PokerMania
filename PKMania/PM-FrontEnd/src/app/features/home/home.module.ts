import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { IndexComponent } from './pages/index/index.component';
import { RegistrationComponent } from './pages/registration/registration.component';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormRegisterComponent } from '../tools/register/pages/form-register/form-register.component';
import { TranslateModule } from '@ngx-translate/core';
import { TableModule } from 'primeng/table';
import { GameComponent } from './pages/game/game.component';
import { RadioButtonModule } from 'primeng/radiobutton';


@NgModule({
  declarations: [
    IndexComponent,
    RegistrationComponent,
    FormRegisterComponent,
    GameComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    InputTextModule,
    ButtonModule,
    ReactiveFormsModule,
    FormsModule,
    TranslateModule,
    TableModule,
    RadioButtonModule
  ]
})
export class HomeModule { }
