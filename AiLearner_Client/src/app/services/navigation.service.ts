import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {

  private backRoute: string[] = [];

  constructor(private router: Router) {}

  setBackRoute(route: string[]) {
    this.backRoute = route;
  }

  navigateBack() {
    this.router.navigate(this.backRoute);
  }}
