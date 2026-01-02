import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GetClientProfileByIdQueryDTO } from './clients-api.models';
 import { buildHttpParams } from "../../core/models/build-http-params";

@Injectable({
    providedIn: 'root'
})
export class ClientsApiService{
 
    private baseUrl=`${environment.apiUrl}/api/client`;
    private http=inject(HttpClient);

    getMyProfile(){
        return this.http.get<GetClientProfileByIdQueryDTO>(`${this.baseUrl}/my-profile`);
    }
}