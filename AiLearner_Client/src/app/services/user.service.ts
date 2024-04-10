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
  catchError,
  of,
  tap,
  throwError,
} from 'rxjs';
import { UserDTO } from '../models/UserDTO';
import { environment } from '../../environments/environment.secret';

/**
 * Service for managing user-related operations.
 */
@Injectable({
  providedIn: 'root',
})
export class UserService {
  secretUrl = environment.baseUrl;
  baseUrl = this.secretUrl + '/User/';

  private userIdSource = new BehaviorSubject<string | null>(null);
  userId$ = this.userIdSource.asObservable();

  private isAuthenticated = new BehaviorSubject<boolean | null>(null);

  constructor(private http: HttpClient) {}

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
      .post<{ userId: string }>(this.baseUrl + 'login', user, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        observe: 'response',
        withCredentials: true,
      })
      .pipe(
        tap((response) => {
          if (response.body?.userId) {
            this.userIdSource.next(response.body.userId);
            this.checkAuth();
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

  checkAuth() {
    this.http
      .get<{
        isAuthenticated: any;
        IsAuthenticated: boolean;
      }>(`${this.secretUrl}/auth/validate-token`, { withCredentials: true })
      .subscribe({
        next: (response) => {
          this.isAuthenticated.next(response.isAuthenticated);
        },
        error: () => {
          this.isAuthenticated.next(false);
        },
      });
  }

  getIsAuthenticated(): Observable<boolean | null> {
    return this.isAuthenticated.asObservable();
  }

  refreshToken() {
    console.log('in refresh token');
    return this.http
      .post<HttpResponse<any>>(
        `${this.secretUrl}/auth/refresh`,
        {},
        {
          withCredentials: true,
        }
      )
      .pipe(
        tap((response: HttpResponse<any>) => {
          if (response.status === 200) {
            this.isAuthenticated.next(true);
          } else {
            this.isAuthenticated.next(false);
          }
        }),
        catchError((error) => {
          this.isAuthenticated.next(false);
          return throwError(() => error);
        })
      );
  }
}
