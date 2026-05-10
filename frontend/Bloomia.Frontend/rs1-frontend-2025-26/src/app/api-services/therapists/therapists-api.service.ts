import { inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";
import { ListTherapistsRequest, ListTherapistsResponse, GetTherapistByIdQueryDto,
    ChangeTherapistPasswordCommand, UpdateTherapistCommand
 } from "./therapists-api.models";
 import { buildHttpParams } from "../../core/models/build-http-params";

@Injectable({
    providedIn: 'root'
})

export class TherapistsApiService {
    private readonly baseUrl = `${environment.apiUrl}/api/therapists`;
    private readonly availabilityBaseUrl = `${environment.apiUrl}/api/TherapistAvailability`;
    private http = inject(HttpClient);

    list(request?: ListTherapistsRequest): Observable<ListTherapistsResponse> {
        const params = request ? buildHttpParams(request as any) : undefined;
        
        return this.http.get<ListTherapistsResponse>(this.baseUrl, { params });
    }

    getById(id: number): Observable<GetTherapistByIdQueryDto> {
        return this.http.get<GetTherapistByIdQueryDto>(`${this.baseUrl}/${id}`);
    }

    update(id: number, payload: UpdateTherapistCommand): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
    }

    uploadProfileImage(file: File): Observable<{ note: string; profileImage: string }> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http.post<{ note: string; profileImage: string }>(
            `${environment.apiUrl}/api/users/upload-profile-image`,
            formData
        );
    }

    uploadTherapistDocument(file: File, documentType: number): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('documentType', documentType.toString());

        return this.http.post<any>(`${this.baseUrl}/upload-document`, formData);
    }

    changePassword(id:number, payload: ChangeTherapistPasswordCommand): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}/change-password`, payload);
    }

}