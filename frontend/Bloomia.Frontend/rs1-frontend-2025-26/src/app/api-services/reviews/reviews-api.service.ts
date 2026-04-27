import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreateReviewCommand,
  CreateReviewResponse,
  GetReviewsByTherapistIdResponse
} from './reviews-api.models';
import { PageRequest } from '../../core/models/paging/page-request';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root'
})
export class ReviewsApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/Reviews`;
  private http = inject(HttpClient);

  getByTherapistId(
    therapistId: number,
    paging?: PageRequest
  ): Observable<GetReviewsByTherapistIdResponse> {
    const params = paging ? buildHttpParams(paging as any) : undefined;

    return this.http.get<GetReviewsByTherapistIdResponse>(
      `${this.baseUrl}/therapists/${therapistId}/reviews`,
      { params }
    );
  }

  create(payload: CreateReviewCommand): Observable<CreateReviewResponse> {
    return this.http.post<CreateReviewResponse>(this.baseUrl, payload);
  }
}