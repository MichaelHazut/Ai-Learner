import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent }  from './components/header/header.component';
import { WelcomeSectionComponent } from './components/welcome-section/welcome-section.component';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,HeaderComponent, WelcomeSectionComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AiLearner_Client';
}
