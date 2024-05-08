import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpResponse,
} from '@angular/common/http';
import {
  BehaviorSubject,
  Observable,
  OperatorFunction,
  catchError,
  filter,
  map,
  of,
  tap,
  throwError,
} from 'rxjs';
import { UserDTO } from '../models/UserDTO';
import { environment } from '../../environments/environment.secret';
import { Router } from '@angular/router';

/**
 * Service for managing user-related operations.
 */
@Injectable({
  providedIn: 'root',
})
export class UserService {
  secretBaseUrl = environment.baseUrl;
  secretDnsUrl = environment.baseUrl;

  baseUrl = this.secretBaseUrl + '/User/';
  dnsUrl = this.secretDnsUrl + '/User/';

  private userIdSource = new BehaviorSubject<string | null>(null);
  userId$ = this.userIdSource.asObservable();

  private isAuthenticated = new BehaviorSubject<boolean | null>(null);

  constructor(private http: HttpClient, private router: Router) {}

  /**
   * Registers a new user.
   * @param user - The user data to be registered.
   * @returns An observable that emits the HTTP response.
   */
  registerUser(user: UserDTO): Observable<any> {
    return this.http
      .post(this.baseUrl + 'register', user, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        observe: 'response',
        withCredentials: true,
      })
      .pipe(
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }

  /**
   * Logs in a user.
   * @param user - The user data to be logged in.
   * @returns An observable that emits the HTTP response.
   */
  loginUser(user: UserDTO): Observable<any> {
    return this.http
      .post<{ userId: string }>(this.dnsUrl + 'login', user, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        observe: 'response',
        withCredentials: true,
      })
      .pipe(
        tap((response) => {
          if (response.body?.userId) {
            this.userIdSource.next(response.body.userId);
            this.checkAuth().subscribe({
              next: (isAuthenticated) => {
                if (isAuthenticated) {
                  this.isAuthenticated.next(true);
                }
              },
            });
          }
        }),
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }

  /**
   * Retrieves a list of users.
   * @returns An observable that emits the list of users.
   */
  getUsers(): Observable<string> {
    return this.http.get<string>('https://localhost:7089/test');
  }
  getEmail(): Observable<string> {
    return this.http.get(this.baseUrl + 'email', {
      withCredentials: true,
      responseType: 'text',
    });
  }

  testGetUserFromAzure(): Observable<any> {
    return this.http.get(this.baseUrl + 'test', {
      withCredentials: true,
    });
  }

  logout() {
    this.http
      .delete(`${this.dnsUrl}/auth/logout`, { withCredentials: true })
      .subscribe({
        next: () => {
          this.userIdSource.next(null);
          this.isAuthenticated.next(false);
          this.router.navigate(['/login']);
        },
        error: () => {
          this.userIdSource.next(null);
          this.isAuthenticated.next(false);
          this.router.navigate(['/']);
        },
      });
  }
  checkAuth(): Observable<boolean> {
    return this.http
      .get<{ isAuthenticated: boolean }>(
        `${this.dnsUrl}/auth/validate-token`,
        { withCredentials: true }
      )
      .pipe(
        tap({
          next: (response) => {
            if (this.isAuthenticated.value) {
              return;
            }
            this.isAuthenticated.next(response.isAuthenticated);
          },
          error: (error) => {
            this.isAuthenticated.next(false);
          },
        }),
        map((response) => response.isAuthenticated),
        catchError(() => {
          this.isAuthenticated.next(false);
          return of(false);
        })
      );
  }


  getIsAuthenticated(): Observable<boolean> {
    // Only emit non-null values
    return this.isAuthenticated.asObservable().pipe(
      filter(isAuthenticated => isAuthenticated !== null) as OperatorFunction<boolean | null, boolean>
    );
  }

  refreshToken(): Observable<boolean> {
    return this.http
      .post(
        `${this.dnsUrl}/auth/refresh`,
        {},
        {
          observe: 'response',
          responseType: 'text',
          withCredentials: true,
        }
      )
      .pipe(
        map((response) => {
          if (response.status === 200) {
            this.isAuthenticated.next(true);
            return true;
          } else {
            this.isAuthenticated.next(false);
            return false;
          }
        }),
        catchError((error) => {
          this.isAuthenticated.next(false);
          return of(false); // Use `of` to return an Observable<boolean>
        })
      );

    
  }
}
