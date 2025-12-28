import { inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})

export class TherapistsApiService {
    private readonly baseUrl = '${environment.apiUrl}/therapists';
    private http = inject(HttpClient);
}