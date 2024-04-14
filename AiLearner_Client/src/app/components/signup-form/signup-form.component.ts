import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

import { UserDTO } from '../../models/UserDTO';
import { UserService } from '../../services/user.service';


@Component({
  selector: 'app-signup-form',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './signup-form.component.html',
  styleUrl: './signup-form.component.css',
})
export class SignupFormComponent {
  userEmail: string = '';
  userPassword: string = '';
  isError = false;
  hide: boolean = true;
  errorMsg: string = '';
  isLoading = false;

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit() {
    this.userService.getIsAuthenticated().subscribe((isAuthenticated) => {
      if (isAuthenticated) {
        this.router.navigate(['/study-hub']);
      }
    });
  }
  onSubmit(event: Event) {
    event.preventDefault();
    this.isLoading = true;

    const user: UserDTO = {
      email: this.userEmail,
      password: this.userPassword,
    };

    this.userService.registerUser(user).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.status === 201) {
          this.router.navigate(['/login']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.isError = true;
      }
      
    });
  }
}
