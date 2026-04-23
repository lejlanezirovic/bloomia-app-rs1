import { inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";
import { ListMyWorkingDatesAndTimesResponse } from "./therapistAvailability-api.models";
import { buildHttpParams } from "../../core/models/build-http-params";

@Injectable({
    providedIn: 'root'
})

export class TherapistAvailabilityApiService {
    private readonly baseUrl = `${environment.apiUrl}/api/TherapistAvailability`;
    private http = inject(HttpClient);


    listMyWorkingDatesAndTimes(): Observable<ListMyWorkingDatesAndTimesResponse> {
        return this.http.get<ListMyWorkingDatesAndTimesResponse>(
            `${this.baseUrl}/list-my-working-dates-and-times`
        );
    }
}