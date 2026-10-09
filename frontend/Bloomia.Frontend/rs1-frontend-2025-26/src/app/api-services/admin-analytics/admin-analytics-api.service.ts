import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {AdminAnalyticsDto, AdminAnalyticsRequest} from './admin-analytics-api.models';

@Injectable({
  providedIn: 'root'
})
export class AdminAnalyticsApiService {

  private readonly baseUrl = `${environment.apiUrl}/api/AdminAnalytics`;
  private http = inject(HttpClient);

  getAnalytics(request: AdminAnalyticsRequest): Observable<AdminAnalyticsDto> {
    return this.http.get<AdminAnalyticsDto>(
      this.baseUrl,
      {
        params: {
          dateFrom: request.dateFrom,
          dateTo: request.dateTo
        }
      }
    );
  }

  exportAnalytics(request: AdminAnalyticsRequest): Observable<Blob> {
    return this.http.get(
      `${this.baseUrl}/export`,
      {
        params: {
          dateFrom: request.dateFrom,
          dateTo: request.dateTo
        },
        responseType: 'blob'
      }
    );
  }
}