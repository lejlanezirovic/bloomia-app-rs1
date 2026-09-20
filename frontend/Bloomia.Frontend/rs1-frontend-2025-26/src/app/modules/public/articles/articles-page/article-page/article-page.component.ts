import { Component, inject, OnInit } from '@angular/core';
import { ArticlesApiService } from '../../../../../api-services/articles/articles-api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ListArticlesQueryDto, ListArticlesRequest } from '../../../../../api-services/articles/articles-api.models';
import { BaseListPagedComponent } from '../../../../../core/components/base-classes/base-list-paged-component';

@Component({
  selector: 'app-article-page',
  standalone: false,
  templateUrl: './article-page.component.html',
  styleUrl: './article-page.component.scss',
})
export class ArticlePageComponent extends BaseListPagedComponent<ListArticlesQueryDto, ListArticlesRequest> implements OnInit {

  private articlesApi = inject(ArticlesApiService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  returnToClient = false;

  constructor() {
    super();
    this.request = new ListArticlesRequest();
    this.request.paging.pageSize = 4;
  }

  ngOnInit(): void {

    const from = this.route.snapshot.queryParamMap.get('from');
    this.returnToClient = from === 'client';

    this.initList();
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.articlesApi.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        console.error('Failed to load articles:', err);
        this.stopLoading('Failed to load articles.');
      }
    });
  }

  openArticle(id: number): void {
    this.router.navigate( ['/articles', id],
      {
        queryParams: this.returnToClient
          ? { from: 'client' }
          : {}
      }
    );
  }

}
