import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export enum AppointmentNotificationStatus {
  Pending = 1,
  Sent = 2,
  Failed = 3
}

export enum AppointmentNotificationType {
  BookingConfirmation = 1,
  AppointmentReminder = 2
}

export class ListAppointmentNotificationLogsRequest
  extends BasePagedQuery {

  search?: string | null;
  status?: number | null;
  notificationType?: number | null;
}

export interface ListAppointmentNotificationLogsQueryDto {
  id: number;
  appointmentId: number;
  recipientEmail: string;
  notificationType: string;
  status: string;
  createdAtUtc: string;
  sentAtUtc: string | null;
  errorMessage: string | null;
  clientName: string;
  therapistName: string;
  scheduledAtUtc: string;
  sessionType: string;
}

export type ListAppointmentNotificationLogsResponse = PageResult<ListAppointmentNotificationLogsQueryDto>;