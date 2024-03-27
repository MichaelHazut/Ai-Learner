import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, tap, throwError } from 'rxjs';
import { MaterialRequestDTO } from '../models/MaterialRequestDTO';
import { MaterialDTO } from '../models/MaterialDTO';

@Injectable({
  providedIn: 'root',
})
export class MaterialService {
  baseUrl = 'https://localhost:7089/api/material';

  constructor(private http: HttpClient) {}

  registerMaterial(dto: MaterialRequestDTO): Observable<any> {
    return this.http
      .post(this.baseUrl , dto, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        observe: 'response',
      })
      .pipe(
        catchError((error) => {
          return throwError(() => error);
        })
      );
  }

  getMaterials(userId: string): Observable<MaterialDTO[]> {
    return this.http.get<MaterialDTO[]>(this.baseUrl + '/' +userId);
  }
}
