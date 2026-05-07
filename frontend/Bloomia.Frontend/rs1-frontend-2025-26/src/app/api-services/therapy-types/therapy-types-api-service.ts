import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { TherapyTypeDto } from './therapy-types-api.models';

@Injectable({
  providedIn: 'root'
})
export class TherapyTypesApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/therapy-types`;
  private http = inject(HttpClient);

  list(): Observable<TherapyTypeDto[]> {
    return this.http.get<TherapyTypeDto[]>(this.baseUrl);
  }
}