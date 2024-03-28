import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { UserDTO } from '../../models/UserDTO';
import { UserService } from '../../services/user.service';
import { HttpResponse } from '@angular/common/http';
import { tap } from 'rxjs';

@Component({
  selector: 'app-signup-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './signup-form.component.html',
  styleUrl: './signup-form.component.css',
})
export class SignupFormComponent {
  userEmail: string = '';
  userPassword: string = '';
  isError = false;
  hide: boolean = true;
  errorMsg: string = '';

  constructor(private userService: UserService, private router: Router) {}

  onSubmit(event: Event) {
    event.preventDefault();

    const user: UserDTO = {
      email: this.userEmail,
      password: this.userPassword,
    };

    this.userService.registerUser(user).subscribe({
      next: (response) => {
        console.log(response);
        if (response.status === 201) {
          console.log('succssfully registered user');
          this.router.navigate(['/login']);
        }
      },
      error: (error) => {
        console.error('Error registering user', error);
        this.isError = true;
      }
    });
  }
}
