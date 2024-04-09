import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, } from '@angular/router';
import { HeaderComponent }  from './components/header/header.component';
import { UserService } from './services/user.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,HeaderComponent, CommonModule,],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AiLearner';

  constructor(private userService: UserService) {}

  ngOnInit() {
    console.log("app init");
    this.userService.checkAuth();
    console.log("checked auth");
  }
}
