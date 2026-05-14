import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { TherapyTypeOptionDto } from './therapy-types-api.models';

@Injectable({
  providedIn: 'root'
})
export class TherapyTypesApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/therapy-types`;
  private http = inject(HttpClient);

  list(): Observable<TherapyTypeOptionDto[]> {
    return this.http.get<TherapyTypeOptionDto[]>(this.baseUrl);
  }
}