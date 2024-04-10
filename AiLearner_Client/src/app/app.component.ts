import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, } from '@angular/router';
import { HeaderComponent }  from './components/header/header.component';
import { UserService } from './services/user.service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,HeaderComponent, CommonModule,],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AiLearner';
  isAuthenticated: boolean | null = null;

  constructor(private userService: UserService) {}
  
  ngOnInit() {
    this.userService.checkAuth();
    }

}
