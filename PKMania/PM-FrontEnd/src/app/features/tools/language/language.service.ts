import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  private keyLanguage = 'userLanguage';
  private _userLanguage = '';

  private supportedLanguages = ['en', 'fr'];

  constructor(
    private translate: TranslateService
  ) {
    this.initLanguage();
    this.translate.use(this._userLanguage);
  }

  initLanguage() {
    const keyLg = localStorage.getItem(this.keyLanguage);
    if (keyLg != null) {
      this._userLanguage = keyLg;
    } else {
      const browserLanguage = navigator.language.split('-')[0];
      this._userLanguage = 'en';
      if (this.supportedLanguages.includes(browserLanguage)) {
        this._userLanguage = browserLanguage;
        localStorage.setItem(this.keyLanguage, browserLanguage);
      }
    }
  }

  setLanguage(language: string ) {
    this._userLanguage = language;
    localStorage.setItem(this.keyLanguage, this._userLanguage);
    this.translate.use(this._userLanguage);
  }

  getUserLanguage() {
    return this._userLanguage;
  }

  getAvailableLanguages() {
    return this.supportedLanguages;
  }
}
