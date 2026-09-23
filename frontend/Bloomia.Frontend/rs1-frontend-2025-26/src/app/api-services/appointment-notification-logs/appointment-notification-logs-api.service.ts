import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

import { buildHttpParams } from '../../core/models/build-http-params'; 
import { ListAppointmentNotificationLogsRequest, ListAppointmentNotificationLogsResponse } from './appointment-notification-logs-api.model';


@Injectable({
  providedIn: 'root'
})
export class AppointmentNotificationLogsApiService {

  private readonly http = inject(HttpClient);

  private readonly baseUrl =
    `${environment.apiUrl}/api/AppointmentNotificationLogs`;

  list(request: ListAppointmentNotificationLogsRequest): Observable<ListAppointmentNotificationLogsResponse> {
    const params = buildHttpParams(request as any);

    return this.http.get<ListAppointmentNotificationLogsResponse>(
      this.baseUrl,
      { params }
    );
  }
}