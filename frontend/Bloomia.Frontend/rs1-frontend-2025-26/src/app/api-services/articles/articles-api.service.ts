import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  GetArticleByIdQueryDto,
  ListArticlesRequest,
  ListArticlesResponse
} from './articles-api.models';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class ArticlesApiService {

  private readonly baseUrl = `${environment.apiUrl}/api/articles`;
  private http = inject(HttpClient);

  list(request?: ListArticlesRequest): Observable<ListArticlesResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;

    return this.http.get<ListArticlesResponse>(this.baseUrl,
      { params }
    );
  }

  getById(id: number): Observable<GetArticleByIdQueryDto> {
    return this.http.get<GetArticleByIdQueryDto>(`${this.baseUrl}/${id}`);
  }
}