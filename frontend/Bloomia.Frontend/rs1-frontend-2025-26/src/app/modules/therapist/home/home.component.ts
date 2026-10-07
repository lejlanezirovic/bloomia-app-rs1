import { Component, inject, OnInit } from '@angular/core';
import { TherapistDashboardApiService } from '../../../api-services/therapist-dashboard/therapist-dashboard-api.service';
import { GenerateTherapistReportCommand, TherapistDashboardOverviewDto, TherapistDashboardReviewsDto, TherapistReportListItemDto } from '../../../api-services/therapist-dashboard/therapist-dashboard-api.model';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MyClientsApiService } from '../../../api-services/therapist-my-clients/my-clients.api.service';
import { ListMyClientsQueryDto } from '../../../api-services/therapist-my-clients/my-clients-api.model';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {

  private dashboardApi = inject(TherapistDashboardApiService);
  private sanitizer = inject(DomSanitizer);
  private myClientsApi =inject(MyClientsApiService);
  private fb = inject(FormBuilder);
  
  clients: ListMyClientsQueryDto[] = [];
  overview?: TherapistDashboardOverviewDto;
  reviews?: TherapistDashboardReviewsDto;
  currentDate = new Date();
  reports: TherapistReportListItemDto[] = [];
  selectedReport: TherapistReportListItemDto | null = null;
  reportPreviewUrl: SafeResourceUrl | null = null;
  private reportObjectUrl: string | null = null;
  isGeneratingReport = false;
  isOverviewLoading = false;
  isReviewsLoading = false;

  reportForm = this.fb.group({
    clientId: [null, Validators.required],
    dateFrom: ['', Validators.required],
    dateTo: ['', Validators.required]
  });

  ngOnInit(): void {
    this.loadOverview();
    this.loadReviews();
    this.loadReports();
    this.loadClients();
  }

  loadClients(): void {

    this.myClientsApi.list().subscribe({
      next: (response) => {
        this.clients = response.items;
      },
      error: (err) => {

        console.error(
          'Failed to load clients:',
          err
        );
      }
    });
  }

  loadReports(): void {
    this.dashboardApi.listReports().subscribe({
        next: (result) => {
          this.reports = result;
        },
        error: (err) => {
          console.error('Failed to load reports:', err);
        }
      });
  }

  generateReport(): void {
    if (this.reportForm.invalid) {
      this.reportForm.markAllAsTouched();
      return;
    }

    const formValue = this.reportForm.getRawValue();

    const request: GenerateTherapistReportCommand = {
      clientId: formValue.clientId!,
      dateFrom: formValue.dateFrom!,
      dateTo: formValue.dateTo!
    };

    this.isGeneratingReport = true;

    this.dashboardApi.generateReport(request).subscribe({
        next: (report) => {
          this.isGeneratingReport = false;
          this.loadReports();
          this.openReport(report);
        },
        error: (err) => {
          this.isGeneratingReport = false;

          console.error('Failed to generate report:', err);
        }
      });
  }

  openReport(report: TherapistReportListItemDto): void {
    this.dashboardApi.getReportFile(report.id).subscribe({
        next: (blob) => {
          if (this.reportObjectUrl) {
            URL.revokeObjectURL(
              this.reportObjectUrl
            );
          }

          this.reportObjectUrl = URL.createObjectURL(blob);

          this.reportPreviewUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
                this.reportObjectUrl
              );

          this.selectedReport = report;
        },

        error: (err) => {
          console.error('Failed to open report:', err);
        }
      });
  }

  downloadReport(report: TherapistReportListItemDto): void {
    this.dashboardApi.getReportFile(report.id).subscribe({
        next: (blob) => {
          const url = URL.createObjectURL(blob);

          const link = document.createElement('a');

          link.href = url;
          link.download = report.fileName;

          link.click();

          URL.revokeObjectURL(url);
        },
        error: (err) => {
          console.error('Failed to download report:', err);
        }
      });
  }

  closeReportPreview(): void {
    if (this.reportObjectUrl) {
      URL.revokeObjectURL(
        this.reportObjectUrl
      );

      this.reportObjectUrl = null;
    }

    this.reportPreviewUrl = null;
    this.selectedReport = null;
  }

  loadReviews(): void {
    this.isReviewsLoading = true;

    this.dashboardApi.getReviews().subscribe({
      next: (response) => {
        this.reviews = response;
        this.isReviewsLoading = false;
      },
      error: () => {
        this.isReviewsLoading = false;
      }
    })
  }

  loadOverview(): void {
    this.isOverviewLoading = true;

    this.dashboardApi.getOverview().subscribe({
      next: (response) => {
        this.overview = response;
        this.isOverviewLoading = false;
      },
      error: () => {
        this.isOverviewLoading = false;
      }
    })
  }

  getSessionTypeName(sessionType: number): string {
    switch (sessionType) {
      case 0:
        return 'Video call';

      case 1:
        return 'Call';

      case 2:
        return 'Chat';

      default:
        return 'Unknown';
    }
  }

}
