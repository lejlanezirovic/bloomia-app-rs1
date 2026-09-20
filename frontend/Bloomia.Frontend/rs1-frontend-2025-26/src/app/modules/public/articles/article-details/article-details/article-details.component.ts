import { Component, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../core/components/base-classes/base-component';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticlesApiService } from '../../../../../api-services/articles/articles-api.service';
import { GetArticleByIdQueryDto } from '../../../../../api-services/articles/articles-api.models';

@Component({
  selector: 'app-article-details',
  standalone: false,
  templateUrl: './article-details.component.html',
  styleUrl: './article-details.component.scss',
})

export class ArticleDetailsComponent extends BaseComponent implements OnInit {

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private articlesApi = inject(ArticlesApiService);

  returnTo: 'articles' | 'client' = 'articles';
  articleId = 0;

  article: GetArticleByIdQueryDto | null = null;

  ngOnInit(): void {
    this.startLoading();

    const from = this.route.snapshot.queryParamMap.get('from');

    if (from === 'client') {
      this.returnTo = 'client';
    }

    this.articleId = +this.route.snapshot.params['id'];

    if (!this.articleId) {
      this.stopLoading('Invalid article id.');
      return;
    }

    this.articlesApi.getById(this.articleId).subscribe({
      next: (response) => {
        this.article = response;
        this.stopLoading();
      },

      error: (err) => {
        console.error('Failed to load article:', err);

        this.stopLoading('Failed to load article.');
      }
    });
  }

  goBack(): void {
    if (this.returnTo === 'client') {
      this.router.navigate(['/client/home']);
    } else {
      this.router.navigate(['/articles']);
    }
  }
}
