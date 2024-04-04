import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, of, tap, throwError } from 'rxjs';
import { environment } from '../../environments/environment.secret';
import { UserAnswersDTO } from '../models/UserAnswersDTO';

@Injectable({
  providedIn: 'root',
})
export class UserAnswersService {
  secretUrl = environment.baseUrl;

  baseUrl = this.secretUrl + '/useranswer';
  constructor(private http: HttpClient) {}

  registerAnswers(materialId:number, dto: UserAnswersDTO[]): Observable<any> {
    return this.http
      .post(`${this.baseUrl}/${materialId}`, dto, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        responseType: 'text',
        observe: 'response',
        withCredentials: true,
      })
      .pipe(
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }

  getUserAnswers(materialId: number): Observable<UserAnswersDTO[]> {
    return this.http.get<UserAnswersDTO[]>(`${this.baseUrl}/${materialId}`).pipe(
      catchError(error => {
        // Check if the error status is 404, which means no user answers were found
        if (error.status === 404) {
          return of([]);
        }
        return throwError(() => error);
      })
    );
  }

  updateUserAnswers(materialId: number, dto: UserAnswersDTO[]): Observable<any> {
    return this.http
      .put(`${this.baseUrl}/${materialId}`, dto, {
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
}
