import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";

export interface AppointmentsForReviewDto {
    appointmentId: number;
    scheduledAtUtc: string;
}

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
