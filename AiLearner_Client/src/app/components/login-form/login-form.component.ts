import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserDTO } from '../../models/UserDTO';

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.css',
})
export class LoginFormComponent {
  userEmail: string = '';
  userPassword: string = '';
  isError = false;
  hide : boolean = true;

  constructor(private userService: UserService, private router: Router) {}



  onSubmit(event: Event) {
    event.preventDefault();

    const user: UserDTO = {
      email: this.userEmail,
      password: this.userPassword,
    };

    this.userService.loginUser(user).subscribe({
      next: (response) => {
        console.log(response);
        if (response.status === 200) {
          console.log('succssfully registered user');
          this.router.navigate(['/study-hub']);
        }
      },
      error: (error) => {
        console.error('Error registering user', error);
        this.isError = true;
      },
    });
  }
}
