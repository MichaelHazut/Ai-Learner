import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReplaceSpacePipe } from '../../pipes/replace-space.pipe';
import { UserService } from '../../services/user.service';
import { UserDropdownComponent } from './user-dropdown/user-dropdown.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, CommonModule, ReplaceSpacePipe,UserDropdownComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  navArray: string[] = ['Home', 'About', 'Contact'];
  showSidebar: boolean = false;
  isAuthenticated: boolean = false;

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.userService.getIsAuthenticated().subscribe({
      next: (isAuthenticated) => {
        if (isAuthenticated) {
          this.navArray.splice(1, 0, 'Study Hub')
          this.navArray= [... new Set(this.navArray)];

          this.isAuthenticated = true;
        }
      },
    });
    this.userService.checkAuth();
  }

  toggleSidebar() {
    this.showSidebar = !this.showSidebar;
  }
}
