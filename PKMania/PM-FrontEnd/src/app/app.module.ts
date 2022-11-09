import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { GlobalConst } from './features/tools/globals/globals';
import { NavbarConnectComponent } from './features/tools/login/pages/navbar-connect/navbar-connect.component';
import { SelectButton, SelectButtonModule } from 'primeng/selectbutton';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { RegisterComponent } from './features/tools/register/register.component';

export function createTranslateLoader(httpClient: HttpClient) {
  return new TranslateHttpLoader(httpClient, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    NavbarConnectComponent,
    RegisterComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    HttpClientModule,
    PasswordModule,
    InputTextModule,
    ButtonModule,
    ReactiveFormsModule,
    SelectButtonModule,
    ToastrModule.forRoot({
    }),
    TranslateModule,
    TranslateModule.forChild(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    GlobalConst
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
