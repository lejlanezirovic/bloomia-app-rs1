import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";

export type AppointmentStatus = 'Scheduled' | 'Cancelled';

export interface ListAppointmentsQueryDto {
    appointmentId: number;
    therapistName?: string | null;
    clientName?: string | null;
    date: string;
    time: string;
    sessionType: string;
    status: AppointmentStatus;
    scheduledAtUtc: string;
    canCancel: boolean;
}

export class ListAppointmentsRequest extends BasePagedQuery {}

export type ListAppointmentsResponse = PageResult<ListAppointmentsQueryDto>;

export enum SessionType {
    VIDEO_CALL = 0,
    CALL = 1,
    MESSAGE = 2
}

export interface CreateAppointmentCommandDto {
    note: string;
    sessionType: string;
    therapistFullname: string;
    bookedAt: string; 
    date: string;
    startTime: string;
}
