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