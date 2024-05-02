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
  // iwant to add afunction to navigate to route /privacy-policy
  constructor(private router: Router) {}

  navigateToPrivacyPolicy() {
    this.router.navigate(['/privacy-policy']);
  }
}

