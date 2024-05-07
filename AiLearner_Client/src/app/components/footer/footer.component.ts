import { Component } from '@angular/core';
import {  Router } from '@angular/router';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.css'
})
export class FooterComponent {
  constructor(private router: Router) {}

  navigateToPrivacyPolicy() {
    this.router.navigate(['/privacy-policy']).then(() => {
      window.scrollTo(0, 0)});
  }
  navigateToHome() {
    this.router.navigate(['/home']).then(() => {
      window.scrollTo(0, 0)});
  }
}

