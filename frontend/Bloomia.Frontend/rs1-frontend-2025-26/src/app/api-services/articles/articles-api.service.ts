import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreateArticleCommand,
  GetArticleByIdQueryDto,
  ListArticlesRequest,
  ListArticlesResponse,
  UpdateArticleCommand
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

  create(command: CreateArticleCommand): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(
      this.baseUrl,
      command
    );
  }

  update(id: number, command: UpdateArticleCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, command);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/${id}`
    );
  }
}