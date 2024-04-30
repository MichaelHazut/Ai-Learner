import { CommonModule } from '@angular/common';
import { Component, HostListener, ElementRef } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-user-dropdown',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-dropdown.component.html',
  styleUrl: './user-dropdown.component.css',
})
export class UserDropdownComponent {
  userEmail: string = 'email not found';
  dropdownOpen = false;

  constructor(
    private userService: UserService,
    private router: Router,
    private eRef: ElementRef
  ) {}

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
          },
        });
      },
      error: (err) => {
        console.error('Error fetching authentication status:', err);
      },
    });
  }
  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }

  //Close dropdown when clicked outside
  @HostListener('document:click', ['$event'])
  clickout(event: Event): void {
    if (!this.eRef.nativeElement.contains(event.target as Node)) {
      this.dropdownOpen = false;
    }
  }

  logout() {
    this.router.navigate(['/signout']);
  }
}
