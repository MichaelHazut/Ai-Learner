import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
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

  private isAuthenticated = new BehaviorSubject<boolean>(false);

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
          console.log('is authenticated: ', response.isAuthenticated);
          this.isAuthenticated.next(response.isAuthenticated);
        },
        error: () => {
          console.log('error at auth check');
          this.isAuthenticated.next(false);
        },
      });
  }

  getIsAuthenticated(): Observable<boolean> {
    return this.isAuthenticated.asObservable();
  }
}
