import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseFormComponent } from '../../../../../core/components/base-classes/base-form-component';
import { ArticlesApiService } from '../../../../../api-services/articles/articles-api.service';
import {
  CreateArticleCommand,
  GetArticleByIdQueryDto
} from '../../../../../api-services/articles/articles-api.models';
import { ToasterService } from '../../../../../core/services/toaster.service';
import { ArticleFormService } from '../services/article-form-service'; 

@Component({
  selector: 'app-articles-add',
  standalone: false,
  templateUrl: './articles-add.component.html',
  styleUrl: './articles-add.component.scss',
  providers: [ArticleFormService]
})
export class ArticlesAddComponent
  extends BaseFormComponent<GetArticleByIdQueryDto>
  implements OnInit {

  private api = inject(ArticlesApiService);
  private formService = inject(ArticleFormService);
  private router = inject(Router);
  private toaster = inject(ToasterService);

  ngOnInit(): void {
    this.initForm(false);
  }

  protected loadData(): void {
    // Not needed in add mode
  }

  protected override initForm(isEdit: boolean): void {
    super.initForm(isEdit);
    this.form = this.formService.createArticleForm();
  }

  protected save(): void {
    if (this.form.invalid || this.isLoading) {
      return;
    }

    this.startLoading();

    const command: CreateArticleCommand = {
      title: this.form.value.title,
      content: this.form.value.content
    };

    this.api.create(command).subscribe({
      next: () => {
        this.stopLoading();
        this.toaster.success('Article created successfully');
        this.router.navigate(['/admin/articles']);
      },
      error: (err) => {
        this.stopLoading('Failed to create article');
        console.error('Create article error:', err);
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