import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment.secret';
import { AnswerDTO } from '../models/AnswerDTO';

@Injectable({
  providedIn: 'root'
})
export class AnswerService {
  secretUrl = environment.baseUrl;
  baseUrl = this.secretUrl + '/answer';

  constructor(private http: HttpClient) { }

  getAnswers(materialId: number): Observable<AnswerDTO[]> {
    return this.http.get<AnswerDTO[]>(this.baseUrl + '/' + materialId,{withCredentials: true,});
  }
}

