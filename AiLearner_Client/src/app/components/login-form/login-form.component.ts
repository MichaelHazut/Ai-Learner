import { Component } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Router,RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserDTO } from '../../models/UserDTO';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.css',
})
export class LoginFormComponent {
  userEmail: string = '';
  userPassword: string = '';
  isError = false;
  hide: boolean = true;

  constructor(
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.userService.getIsAuthenticated().subscribe({
      next: (isAuthenticated) => {
        if (isAuthenticated) {
          this.router.navigate(['/study-hub']);
        }
      },
      error: (error) => {
        console.error('LoginForm: Error while checking authentication status: ', error);
      }
    });
    this.userService.checkAuth();
  }

  onSubmit(event: Event) {
    event.preventDefault();

    const user: UserDTO = {
      email: this.userEmail,
      password: this.userPassword,
    };

    this.userService.loginUser(user).subscribe({
      next: (response) => {
        if (response.status === 200) {
          this.router.navigate(['/study-hub']);
        }
      },
      error: () => {
        this.toastr.error('Invalid email or password');
        this.isError = true;
      },
    });
  }
}
