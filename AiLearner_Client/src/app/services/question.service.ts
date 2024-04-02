import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment.secret';
import { QuestionDTO } from '../models/QuestionDTO';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {
  secretUrl = environment.baseUrl;
  baseUrl = this.secretUrl + '/question';

  constructor(private http: HttpClient) { }

  getQuestions(materialId: number) : Observable<QuestionDTO[]> {
    return this.http.get<QuestionDTO[]>(this.baseUrl + '/' + materialId);
  }
}
