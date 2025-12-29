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

    changePassword(id:number, payload: ChangeTherapistPasswordCommand): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}/change-password`, payload);
    }
}