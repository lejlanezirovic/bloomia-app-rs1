import { inject, Injectable } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { GetArticleByIdQueryDto } from '../../../../../api-services/articles/articles-api.models';

@Injectable()
export class ArticleFormService {

  private fb = inject(FormBuilder);

  createArticleForm(article?: GetArticleByIdQueryDto): FormGroup {

    return this.fb.group({

      title: [
        article?.title ?? '',
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(150)
        ]
      ],

      content: [
        article?.content ?? '',
        [
          Validators.required,
          Validators.minLength(20),
          Validators.maxLength(5000)
        ]
      ]

    });
  }

  getErrorMessage(form: FormGroup, controlName: string): string {

    const control =
      form.get(controlName);

    if (!control) {
      return '';
    }

    if (control.hasError('required')) {
      return 'This field is required.';
    }

    if (control.hasError('minlength')) {

      const length = control.getError('minlength').requiredLength;

      return `Minimum ${length} characters required.`;
    }

    if (control.hasError('maxlength')) {

      const length = control.getError('maxlength').requiredLength;

      return `Maximum ${length} characters allowed.`;
    }

    return '';
  }
}