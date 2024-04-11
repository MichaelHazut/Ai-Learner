import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-logout',
  standalone: true,
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {

    constructor(
    private userService: UserService,
  ) { }
  
  ngOnInit() {
    this.userService.logout()
  }
}
