import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {
  ListArticlesQueryDto,
  ListArticlesRequest
} from '../../../../../api-services/articles/articles-api.models';
import { ArticlesApiService } from '../../../../../api-services/articles/articles-api.service';
import { BaseListPagedComponent } from '../../../../../core/components/base-classes/base-list-paged-component';
import { DialogHelperService } from '../../../../shared/services/dialog-helper.service';
import { DialogButton } from '../../../../shared/models/dialog-config.model';

@Component({
  selector: 'app-articles-list',
  standalone: false,
  templateUrl: './articles-list.component.html',
  styleUrl: './articles-list.component.scss'
})
export class ArticlesListComponent
  extends BaseListPagedComponent<ListArticlesQueryDto, ListArticlesRequest>
  implements OnInit {

  private api = inject(ArticlesApiService);
  private router = inject(Router);
  private dialogHelper = inject(DialogHelperService);

  displayedColumns: string[] = [
    'title',
    'adminName',
    'publishedAt',
    'actions'
  ];

  constructor() {
    super();

    this.request = new ListArticlesRequest();

    this.request.paging.page = 1;
    this.request.paging.pageSize = 5;
  }

  ngOnInit(): void {
    this.initList();
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.api.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading('Failed to load articles');
        console.error('Load articles error:', err);
      }
    });
  }

  onCreate(): void {
    this.router.navigate(['/admin/articles/add']);
  }

  onEdit(article: ListArticlesQueryDto): void {
    this.router.navigate(['/admin/articles', article.id, 'edit']);
  }

  onDelete(article: ListArticlesQueryDto): void {
    this.dialogHelper.article.confirmDelete(article.title).subscribe(result => {
      if (result && result.button === DialogButton.DELETE) {
        this.performDelete(article);
      }
    });
  }

  private performDelete(article: ListArticlesQueryDto): void {
    this.startLoading();

    this.api.delete(article.id).subscribe({
      next: () => {
        this.dialogHelper.article.showDeleteSuccess().subscribe();
        this.loadPagedData();
      },
      error: (err) => {
        this.stopLoading();

        this.dialogHelper.showError('DIALOGS.TITLES.ERROR', 'Failed to delete article.').subscribe();

        console.error('Delete article error:', err);
      }
    });
  }

  onSearch(): void {
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onClear(): void {
    this.request.title = null;
    this.request.content = null;
    this.request.adminName = null;
    this.request.dateFrom = null;
    this.request.dateTo = null;

    this.request.paging.page = 1;

    this.loadPagedData();
  }
}