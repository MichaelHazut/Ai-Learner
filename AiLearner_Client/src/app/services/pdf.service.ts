import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.secret';
import { HttpClient } from '@angular/common/http';
import { tap, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PdfService {
  secretUrl = environment.baseUrl;
  baseUrl = this.secretUrl + '/material';
  constructor(private http: HttpClient) {}

  public getPDF(materialId: number) {
    return this.http.get(`${this.baseUrl}/${materialId}/download-pdf`, {
      observe: 'response',
      responseType: 'blob',
      withCredentials: true
    })
  };
}
