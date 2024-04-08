import { inject } from '@angular/core';
import { Router, CanActivateFn, UrlTree } from '@angular/router';
import { map } from 'rxjs/operators';
import { UserService } from './services/user.service';
import { Observable } from 'rxjs';

export const authGuard: CanActivateFn = (): Observable<boolean | UrlTree> => {
  const userService = inject(UserService);
  const router = inject(Router);

  return userService.userId$.pipe(
    map(userId => {
      return userId ? true : router.parseUrl('/login');
    })
  );
};