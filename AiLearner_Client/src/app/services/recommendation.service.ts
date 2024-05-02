import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.secret';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RecommendationDTO } from '../models/RecommendationDTO';

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
  secretUrl = environment.baseUrl;
  baseUrl = this.secretUrl + '/Recommendation';
  constructor(private http: HttpClient) { }

  getRecommendations(materialId: number): Observable<RecommendationDTO[]> {
    return this.http.get<RecommendationDTO[]>(this.baseUrl + '/' + materialId,{withCredentials: true,});
  }
}
