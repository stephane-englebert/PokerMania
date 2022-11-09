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


@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    FormRegisterComponent
  ],
  imports: [
    CommonModule,
    ToolsRoutingModule,
    PasswordModule,
    InputTextModule,
    ButtonModule,
    ReactiveFormsModule,
    FormsModule,
    TranslateModule
  ]
})
export class ToolsModule { }
