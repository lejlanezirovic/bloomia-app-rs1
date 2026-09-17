import { Component, inject, OnInit } from '@angular/core';
import { TherapistDashboardApiService } from '../../../api-services/therapist-dashboard/therapist-dashboard-api.service';
import { TherapistDashboardOverviewDto, TherapistDashboardReviewsDto } from '../../../api-services/therapist-dashboard/therapist-dashboard-api.model';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {

  private dashboardApi = inject(TherapistDashboardApiService);

  overview?: TherapistDashboardOverviewDto;
  reviews?: TherapistDashboardReviewsDto;
  isLoading = false;
  currentDate = new Date();

  ngOnInit(): void {
    this.loadOverview();
    this.loadReviews();
  }

  loadReviews(): void {
    this.isLoading = true;

    this.dashboardApi.getReviews().subscribe({
      next: (response) => {
        this.reviews = response;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    })
  }

  loadOverview(): void {
    this.isLoading = true;

    this.dashboardApi.getOverview().subscribe({
      next: (response) => {
        this.overview = response;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
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
