import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ListMyClientsRequest,
  ListMyClientsResponse
} from './my-clients-api.model';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class MyClientsApiService {

  private readonly baseUrl = `${environment.apiUrl}/api/therapists/my-clients`;
  private http = inject(HttpClient);

  list(request?: ListMyClientsRequest): Observable<ListMyClientsResponse> {
    const params = request
      ? buildHttpParams(request as any)
      : undefined;

    return this.http.get<ListMyClientsResponse>(
      this.baseUrl,
      { params }
    );
  }
}