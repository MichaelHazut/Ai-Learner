import { Component, HostListener, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReplaceSpacePipe } from '../../pipes/replace-space.pipe';
import { UserService } from '../../services/user.service';
import { UserDropdownComponent } from './user-dropdown/user-dropdown.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, CommonModule, ReplaceSpacePipe, UserDropdownComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  navArray: string[] = ['Home', 'About', 'privacy policy'];
  showSidebar: boolean = false;
  isAuthenticated: boolean = false;

  constructor(private userService: UserService,private eRef: ElementRef) {}

  ngOnInit() {
    this.userService.getIsAuthenticated().subscribe({
      next: (isAuthenticated) => {
        if (isAuthenticated) {
          this.navArray.splice(1, 0, 'Study Hub');
          this.navArray = [...new Set(this.navArray)];

          this.isAuthenticated = true;
        }
        if (!isAuthenticated) {
          const index = this.navArray.indexOf('Study Hub');
          if (index !== -1) {
            this.navArray.splice(index, 1);
          }
          this.isAuthenticated = false;
        }
      },
    });
  }

  @HostListener('document:click', ['$event'])
  clickout(event: Event): void {
    if (!this.eRef.nativeElement.contains(event.target as Node)) {
      this.showSidebar = false;
    }
  }
  toggleSidebar() {
    this.showSidebar = !this.showSidebar;
  }
}
