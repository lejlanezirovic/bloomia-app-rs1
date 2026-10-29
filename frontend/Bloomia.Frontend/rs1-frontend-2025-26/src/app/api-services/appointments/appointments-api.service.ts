import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  SessionType,
  CreateAppointmentCommandDto,
  ListAppointmentsRequest,
  ListAppointmentsResponse
} from './appointments-api.models';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
    providedIn: 'root'
})
export class AppointmentsApiService{
 
    private baseUrl = `${environment.apiUrl}/api/Appointment`;
    private http = inject(HttpClient);

    getMyAppointments(request: ListAppointmentsRequest): Observable<ListAppointmentsResponse> {
        const params = buildHttpParams(request as any);
        return this.http.get<ListAppointmentsResponse>(`${this.baseUrl}/get-my-appointments`, { params });
    }

    cancelAppointment(appointmentId: number): Observable<string> {
        return this.http.delete<string>(`${this.baseUrl}/delete-appointment`, {
            params: { id: appointmentId },
            responseType: 'text' as 'json'
        });
    }

    createAppointment(therapistAvailabilityId: number, sessionType: SessionType) {
        return this.http.post<CreateAppointmentCommandDto>(
            `${this.baseUrl}/create-appointment`,
            null,
            {
                params: {
                    TAid: therapistAvailabilityId,
                    sessionType
                }
            }
        );
    }

}