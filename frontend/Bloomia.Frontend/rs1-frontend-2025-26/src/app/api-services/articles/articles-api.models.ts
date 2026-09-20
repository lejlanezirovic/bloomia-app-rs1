import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export class ListArticlesRequest extends BasePagedQuery {
  search?: string | null;
}

export interface ListArticlesQueryDto {
  id: number;
  title: string;
  excerpt: string;
  publishedAt: string;
  adminName: string;
}

export interface GetArticleByIdQueryDto {
  id: number;
  title: string;
  content: string;
  publishedAt: string;
  adminName: string;
}

export type ListArticlesResponse =
  PageResult<ListArticlesQueryDto>;