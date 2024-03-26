import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReplaceSpacePipe } from '../../pipes/replace-space.pipe';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, CommonModule, ReplaceSpacePipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  navArray : string[] = ['Home', 'Study Hub', 'About', 'Contact'];
  showSidebar : boolean = false;

  toggleSidebar() {
    this.showSidebar = !this.showSidebar;
  }
}
