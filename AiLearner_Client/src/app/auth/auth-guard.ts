import { inject } from '@angular/core';
import { Router, CanActivateFn, UrlTree } from '@angular/router';
import { UserService } from '../services/user.service';
import { Observable, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';

export const authGuard: CanActivateFn = (): Observable<boolean | UrlTree> => {
  const userService = inject(UserService);
  const router = inject(Router);

  // Trigger a check for the current authentication status
  userService.checkAuth();

  // Decide on the navigation based on the authentication status
  return userService.getIsAuthenticated().pipe(
    switchMap(isAuthenticated => {
      console.log("isisauthenitcated ",isAuthenticated);
      if (!isAuthenticated) {
        // If not authenticated, redirect to the login page
        console.log("redirect to login");
        return of(router.parseUrl('/login'));
      }
      // If authenticated, allow the navigation
      console.log("allow navigation");
      return of(true);
    })
  );
};
