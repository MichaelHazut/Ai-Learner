import { inject } from '@angular/core';
import { Router, CanActivateFn, UrlTree } from '@angular/router';
import { UserService } from '../services/user.service';
import { Observable, of } from 'rxjs';
import { filter, switchMap, take, tap, map } from 'rxjs/operators';

export const authGuard: CanActivateFn = (): Observable<boolean | UrlTree> => {
  const userService = inject(UserService);
  const router = inject(Router);
  
  return userService.getIsAuthenticated().pipe(
    take(1),
    switchMap(isAuthenticated => {
      if (isAuthenticated !== null) {
        // Use the existing state if it's not null
        return of(isAuthenticated ? true : router.parseUrl('/login'));
      } else {
        // If the state is null, check authentication status
        return userService.checkAuth().pipe(
          map(isAuthenticated => {
            return isAuthenticated ? true : router.parseUrl('/login');
          })
        );
      }
    })
  );


  // return userService.getIsAuthenticated().pipe(
  //   take(1), // Take only the first value emitted
  //   switchMap((isAuthenticated) => {
  //     if (isAuthenticated === true) {
  //       // If isAuthenticated is true, allow the navigation
  //       console.log("authGuard Fetch check auth: ", isAuthenticated);
  //       return of(true);
  //     } 
  //     console.log("redirecting to login");
  //     return of(router.parseUrl('/login'));
  //   })
  // );


  // return userService.getIsAuthenticated().pipe(
  //   tap(() => userService.checkAuth()),
  //   filter(isAuthenticated => isAuthenticated !== null), 
  //   switchMap(isAuthenticated => {
  //     console.log("authGuard Fetch check auth: ", isAuthenticated);
  //     if (!isAuthenticated) {
  //       return of(router.parseUrl('/login'));
  //     }
  //     return of(true);
  //   })
  // );
};
