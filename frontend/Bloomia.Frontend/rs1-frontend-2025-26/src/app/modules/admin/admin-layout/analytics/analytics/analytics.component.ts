import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AdminAnalyticsApiService } from '../../../../../api-services/admin-analytics/admin-analytics-api.service';
import { AdminAnalyticsDto, AdminAnalyticsRequest } from '../../../../../api-services/admin-analytics/admin-analytics-api.models';

@Component({
  selector: 'app-analytics',
  standalone: false,
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.scss',
})
export class AnalyticsComponent {

  private fb = inject(FormBuilder);
  private analyticsApi = inject(AdminAnalyticsApiService);

  analytics: AdminAnalyticsDto | null = null;

  isLoading = false;
  isExporting = false;
  errorMessage = '';

  filterForm = this.fb.group({
    dateFrom: [null as Date | null, Validators.required],
    dateTo: [null as Date | null, Validators.required]
  });

  loadAnalytics(): void {
    if (this.filterForm.invalid) {
      this.filterForm.markAllAsTouched();
      this.errorMessage = 'Please select both start date and end date.';
      return;
    }

    const formValue = this.filterForm.getRawValue();

    this.isLoading = true;
    this.errorMessage = '';

    this.analyticsApi.getAnalytics({
      dateFrom: this.formatDate(formValue.dateFrom!),
      dateTo: this.formatDate(formValue.dateTo!)
    }).subscribe({
      next: response => {
        this.analytics = response;
        this.isLoading = false;
      },
      error: err => {
        console.error(err);
        this.errorMessage = 'Failed to load analytics.';
        this.isLoading = false;
      }
    });
  }

  exportExcel(): void {
    if (this.filterForm.invalid) {
      this.filterForm.markAllAsTouched();
      return;
    }

    const formValue = this.filterForm.getRawValue();

    const request: AdminAnalyticsRequest = {
      dateFrom: this.formatDate(formValue.dateFrom!),
      dateTo: this.formatDate(formValue.dateTo!)
    };

    this.isExporting = true;

    this.analyticsApi.exportAnalytics(request).subscribe({
      next: blob => {
        const url = URL.createObjectURL(blob);

        const link = document.createElement('a');

        link.href = url;
        link.download = `Bloomia-Analytics-${request.dateFrom}-${request.dateTo}.xlsx`;

        link.click();

        URL.revokeObjectURL(url);

        this.isExporting = false;
      },
      error: err => {
        console.error(err);
        this.isExporting = false;
      }
    });
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

}