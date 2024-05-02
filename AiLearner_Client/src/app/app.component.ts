import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'AiLearner';
  isAuthenticated: boolean | null = null;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.userService.checkAuth().subscribe((res) => {
      if(res) {
        this.isAuthenticated = true;
      }
    });
  }
}
