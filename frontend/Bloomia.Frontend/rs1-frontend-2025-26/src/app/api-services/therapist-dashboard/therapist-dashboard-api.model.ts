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
  clientId: number;
  clientName: string; 
  dateFrom: string;
  dateTo: string; 
  fileName: string;
  generatedAtUtc: string; 
  appointmentsCount: number;
  completedSessionsCount: number;
  averageRating: number;
}

export interface GenerateTherapistReportCommand {
  clientId: number;
  dateFrom: string;
  dateTo: string;
}