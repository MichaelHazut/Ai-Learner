import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'AiLearner';
  isAuthenticated: boolean | null = null;

  constructor(private userService: UserService) {}

  ngOnInit() {
    console.log("AppComponent ngOnInit");
    this.userService.checkAuth().subscribe((res) => {
      console.log("AppComponent checkAuth subscribe res: ", res);
      if(res) {
        console.log("AppComponent checkAuth subscribe res is true");
        this.isAuthenticated = true;
      }
    });
  }
}
