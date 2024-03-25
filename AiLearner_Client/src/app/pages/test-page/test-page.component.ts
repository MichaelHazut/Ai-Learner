import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-test-page',
  standalone: true,
  imports: [],
  templateUrl: './test-page.component.html',
  styleUrl: './test-page.component.css'
})
export class TestPageComponent {

  constructor(private userService: UserService) { }

  getUsers() {
    this.userService.getUsers().subscribe((data) => {
      console.log(data);
    });
  }
}
