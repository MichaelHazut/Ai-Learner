import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserDTO } from '../../models/UserDTO';

@Component({
  selector: 'app-test-page',
  standalone: true,
  imports: [],
  templateUrl: './test-page.component.html',
  styleUrl: './test-page.component.css'
})
export class TestPageComponent {

  constructor(private userService: UserService) { }

  signUp() {
    const email = "test@gmail.com";
    const password = "password";
    let userDto: UserDTO = {email:email,password:password}
    // this.userService.signUp(userDto).subscribe((data) => {
    //   console.log(data);
    // });
  }

  getUsers() {
    this.userService.getUsers().subscribe((data) => {
      console.log(data);
    });
  }
}
