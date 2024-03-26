import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { UserDTO } from '../models/UserDTO';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = 'https://localhost:7089/api/User/';

  constructor(private http: HttpClient) {}

  registerUser(user: UserDTO): Observable<any> {
    return this.http.post(this.baseUrl + 'register', user, {
      headers: new HttpHeaders({'Content-Type': 'application/json'}),
      observe: 'response'
    }).pipe(
      catchError((error) => {
        return throwError(() => error);
      })
    );
  }

  loginUser(user: UserDTO): Observable<any> {
    return this.http.post(this.baseUrl + 'login', user, {
      headers: new HttpHeaders({'Content-Type': 'application/json'}),
      observe: 'response',
      withCredentials: true
    }).pipe(
      catchError((error) => {
        return throwError(() => error);
      })
    );
  }

  getUsers(): Observable<string> {
    return this.http.get<string>('https://localhost:7089/test');
  }
}
