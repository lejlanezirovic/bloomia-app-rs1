import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  AppointmentsForReviewDto,
  SessionType,
  CreateAppointmentCommandDto
} from './appointments-api.models';

@Injectable({
    providedIn: 'root'
})
export class AppointmentsApiService{
 
    private baseUrl = `${environment.apiUrl}/api/Appointment`;
    private http = inject(HttpClient);

    getAppointmentsForReview(therapistId: number) : Observable<AppointmentsForReviewDto[]> {
        return this.http.get<AppointmentsForReviewDto[]>(
            `${this.baseUrl}/get-appointments-for-review`,
            { params: { therapistId } }
        );
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