import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseFormComponent } from '../../../../../core/components/base-classes/base-form-component';
import { GetArticleByIdQueryDto, UpdateArticleCommand } from '../../../../../api-services/articles/articles-api.models';
import { ArticlesApiService } from '../../../../../api-services/articles/articles-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';
import { ArticleFormService } from '../services/article-form-service'; 

@Component({
  selector: 'app-articles-edit',
  standalone: false,
  templateUrl: './articles-edit.component.html',
  styleUrl: './articles-edit.component.scss',
  providers: [ArticleFormService]
})
export class ArticlesEditComponent
  extends BaseFormComponent<GetArticleByIdQueryDto>
  implements OnInit {

  private api = inject(ArticlesApiService);
  private formService = inject(ArticleFormService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private toaster = inject(ToasterService);

  articleId!: number;

  ngOnInit(): void { 
    this.articleId = +this.route.snapshot.params['id'];

    this.initForm(true);
  }

  protected loadData(): void {

    this.startLoading();

    this.api.getById(this.articleId).subscribe({
        next: (article) => {
          this.model = article;
          this.form = this.formService.createArticleForm(article);

          this.stopLoading();
        },
        error: (err) => {
          this.stopLoading('Failed to load article');
          this.toaster.error('Article not found');

          console.error('Load article error:', err);

          this.router.navigate(['/admin/articles']);
        }

      });
  }

  protected save(): void {
    if (this.form.invalid || this.isLoading) {
      return;
    }

    this.startLoading();

    const command: UpdateArticleCommand = this.form.getRawValue();

    this.api.update(this.articleId, command).subscribe({
        next: () => {
          this.stopLoading();
          this.toaster.success('Article updated successfully');

          this.router.navigate(['/admin/articles']);
        },
        error: (err) => {
          this.stopLoading('Failed to update article');

          console.error('Update article error:', err);
        }

      });
  }

  onCancel(): void {
    this.router.navigate(['/admin/articles']);
  }

  getErrorMessage(controlName: string): string {
    return this.formService.getErrorMessage(this.form, controlName);
  }
}