import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TherapistDashboardOverviewDto, TherapistDashboardReviewsDto, TherapistReportListItemDto } from './therapist-dashboard-api.model'; 

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

  getReviews(): Observable<TherapistDashboardReviewsDto> {
    return this.http.get<TherapistDashboardReviewsDto>(
      `${this.baseUrl}/reviews`
    );
  }

  generateReport(): Observable<TherapistReportListItemDto> {
  return this.http.post<TherapistReportListItemDto>(
    `${this.baseUrl}/reports/generate`,
    {}
  );
}

listReports(): Observable<TherapistReportListItemDto[]> {
  return this.http.get<TherapistReportListItemDto[]>(
    `${this.baseUrl}/reports`
  );
}

getReportFile(id: number): Observable<Blob> {
  return this.http.get(
    `${this.baseUrl}/reports/${id}/file`,
    {
      responseType: 'blob'
    }
  );
}

}