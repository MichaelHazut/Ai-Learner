import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserDTO } from '../../models/UserDTO';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-test-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './test-page.component.html',
  styleUrl: './test-page.component.css',
})
export class TestPageComponent {
  userId: string | null = null;

  constructor(private userService: UserService,private toastr: ToastrService) {
    this.userService.userId$.subscribe((id) => {
      this.userId = id;
      console.log('User ID updated:', id);
    });
  }

  loginUser() {
    const user: UserDTO = {
      // Assuming your UserDTO has these fields; adjust accordingly
      email: 'Michael1mic1@gmail.com',
      password: 'abcd1234',
    };

    this.userService.loginUser(user).subscribe({
      next: (response) => {
        console.log('Logged in successfully');
        // No need to manually set userId here, as it's handled by the subscription
      },
      error: (error) => {
        console.error('Login error:', error);
      },
    });
  }

  signUp() {
    const email = 'test@gmail.com';
    const password = 'password';
    let userDto: UserDTO = { email: email, password: password };
    // this.userService.signUp(userDto).subscribe((data) => {
    //   console.log(data);
    // });
  }

  getUsers() {
    this.userService.getUsers().subscribe((data) => {
      console.log(data);
    });
  }
  showSuccess() {
    this.toastr.warning('there was a problem loading material!');
  }
}
