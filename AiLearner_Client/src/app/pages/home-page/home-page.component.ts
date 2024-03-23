import { Component } from '@angular/core';
import { WelcomeSectionComponent } from '../../components/welcome-section/welcome-section.component';
import { InfoSectionComponent } from '../../components/info-section/info-section.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [WelcomeSectionComponent, InfoSectionComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent {

}
