import { Component } from '@angular/core';
import { LanguageService } from './features/tools/language/language.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'PM-FrontEnd';

  constructor(
    private languageService: LanguageService
  ) {

  }
}
