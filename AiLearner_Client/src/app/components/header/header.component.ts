import { Component } from '@angular/core';
import { NgForOf } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, NgForOf],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  navArray : string[] = ['Home', 'About', 'Contact'];
  showSidebar : boolean = false;

  toggleSidebar() {
    this.showSidebar = !this.showSidebar;
  }
}
