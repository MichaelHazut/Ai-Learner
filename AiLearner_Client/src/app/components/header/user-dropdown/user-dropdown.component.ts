import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-user-dropdown',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-dropdown.component.html',
  styleUrl: './user-dropdown.component.css'
})
export class UserDropdownComponent {
  userEmail: string = "email not found";
  dropdownOpen = false;

  constructor(private userService: UserService) {}
  ngOnInit() {
    this.userService.getIsAuthenticated().subscribe({
      next: (isAuthenticated) => {
        if (!isAuthenticated) {
          return;
        }
        this.userService.getEmail().subscribe({
          next: (email) => {
            if (email) {
              this.userEmail = email;
            }
          },
          error: (err) => {
            console.error('Error fetching email:', err);
          }
        });
      },
      error: (err) => {
        console.error('Error fetching authentication status:', err);
      }
    });
  }
  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }
}
