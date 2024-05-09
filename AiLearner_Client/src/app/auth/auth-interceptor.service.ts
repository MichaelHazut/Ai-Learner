import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpErrorResponse,
  HttpEvent,
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { UserService } from '../services/user.service';
@Injectable({
  providedIn: 'root',
})
export class AuthInterceptor implements HttpInterceptor {
  constructor(private userService: UserService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

    // Always attach tokens even if null
    const token = localStorage.getItem('accessToken') || '';
    const refreshToken = localStorage.getItem('refreshToken') || '';

    // Clone the request to add the authorization headers
    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        'X-Refresh-Token': refreshToken,
      },
    });

    if (request.url.includes('/auth/refresh')) {
      return next.handle(request);
    }

    // Handle the request and catch errors
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        // If validation token request fails, request to refresh the token
        if (
          error.status === 401 &&
          request.url.includes('/auth/validate-token')
        ) {
          return this.handle401Error(request, next);
        }
        return throwError(() => new Error(error.message));
      })
    );
  }

  private handle401Error(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return this.userService.refreshToken().pipe(
      switchMap(() => {
        // After refreshing the token, retry the original request
        return next.handle(request);
      }),
      catchError((error) => {
        return throwError(() => error);
      })
    );
  }
}
// export class AuthInterceptor implements HttpInterceptor {
//   constructor(private userService: UserService) {}

//   intercept(
//     request: HttpRequest<any>,
//     next: HttpHandler
//   ): Observable<HttpEvent<any>> {
//     console.log('intercepted request ... ');
//     if (request.url.includes('/auth/refresh')) {
//       return next.handle(request);
//     }
//     return next.handle(request).pipe(
//       catchError((error: HttpErrorResponse) => {
//         if (
//           error.status === 401 &&
//           request.url.includes('/auth/validate-token')
//         ) {
//           return this.handle401Error(request, next);
//         }
//         return throwError(() => new Error(error.message));
//       })
//     );
//   }
//   private handle401Error(
//     request: HttpRequest<any>,
//     next: HttpHandler
//   ): Observable<HttpEvent<any>> {
//     return this.userService.refreshToken().pipe(
//       switchMap(() => {
//         // After refreshing the token, retry the original request
//         return next.handle(request);
//       }),
//       catchError((error) => {
//         return throwError(() => error);
//       })
//     );
//   }
// }
