import { inject } from '@angular/core';
import { Router, CanActivateFn, UrlTree } from '@angular/router';
import { UserService } from '../services/user.service';
import { Observable, of } from 'rxjs';
import { switchMap, take, } from 'rxjs/operators';

export const authGuard: CanActivateFn = (): Observable<boolean | UrlTree> => {
  const userService = inject(UserService);
  const router = inject(Router);
  return userService.getIsAuthenticated().pipe(
    take(1),
    switchMap(isAuthenticated => {
      return of(isAuthenticated ? true : router.parseUrl('/login'));
    })
  );
};
  