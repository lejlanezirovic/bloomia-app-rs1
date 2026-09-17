import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TherapistDashboardOverviewDto } from './therapist-dashboard-api.model'; 

@Injectable({
  providedIn: 'root',
})
export class TherapistDashboardApiService {

  private readonly baseUrl =
    `${environment.apiUrl}/api/therapist-dashboard`;

  private http = inject(HttpClient);

  
  getOverview(): Observable<TherapistDashboardOverviewDto> {
    return this.http.get<TherapistDashboardOverviewDto>(
      `${this.baseUrl}/overview`
    );
  }
}