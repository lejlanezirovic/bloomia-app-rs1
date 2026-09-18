import { SessionType } from '../appointments/appointments-api.models';

export interface TherapistUpcomingAppointmentDto {
  id: number;
  clientId: number;
  clientName: string;
  scheduledAtUtc: string;
  sessionType: SessionType;
}

export interface TherapistDashboardOverviewDto {
  appointmentsThisMonth: number;
  pastAppointmentsThisMonth: number;
  activeClients: number;
  averageRating: number;
  upcomingAppointments: TherapistUpcomingAppointmentDto[];
}

export interface TherapistDashboardReviewItemDto {
  rating: number;
  clientName: string;
  comment?: string | null;
  createdAtUtc: string;
}

export interface TherapistDashboardReviewsDto {
  totalReviews: number;
  fiveStarReviews: number;
  fourStarReviews: number;
  threeStarReviews: number;
  twoStarReviews: number;
  oneStarReviews: number;
  latestReviews: TherapistDashboardReviewItemDto[];
}

export interface TherapistReportListItemDto {
  id: number;
  month: number;
  year: number;
  fileName: string;
  generatedAtUtc: string;
  appointmentsCount: number;
  completedSessionsCount: number;
  activeClientsCount: number;
  averageRating: number;
  totalReviews: number;
}