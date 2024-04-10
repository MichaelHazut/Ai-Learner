import { inject } from '@angular/core';
import { Router, CanActivateFn, UrlTree } from '@angular/router';
import { UserService } from '../services/user.service';
import { Observable, of } from 'rxjs';
import { filter, switchMap, tap } from 'rxjs/operators';

export const authGuard: CanActivateFn = (): Observable<boolean | UrlTree> => {
  const userService = inject(UserService);
  const router = inject(Router);

  return userService.getIsAuthenticated().pipe(
    tap(() => userService.checkAuth()),
    filter(isAuthenticated => isAuthenticated !== null), 
    switchMap(isAuthenticated => {
      if (!isAuthenticated) {
        return of(router.parseUrl('/login'));
      }
      return of(true);
    })
  );
};
