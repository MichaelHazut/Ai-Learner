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
    console.log("dropdown component initialized");
    this.userService.getEmail().subscribe({
      next: (email) => {
        if (email) {
          console.log(email);
          this.userEmail = email;
        }
      },
      error: (err) => {
        console.error('Error fetching email:', err);
        // Handle the error appropriately
      }
    });

  }
  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }
}
