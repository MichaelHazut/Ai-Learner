import { Component } from '@angular/core';
import { NgForOf } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NgForOf],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  navArray : string[] = ['Home', 'About', 'Contact'];
}
