import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavigationService } from '../../services/navigation.service';

@Component({
  selector: 'app-study-hub',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './study-hub.component.html',
  styleUrl: './study-hub.component.css',
})
export class StudyHubComponent {
  constructor(private navigationService: NavigationService) {}
  
  goBack() {
    this.navigationService.navigateBack();
  }
}
