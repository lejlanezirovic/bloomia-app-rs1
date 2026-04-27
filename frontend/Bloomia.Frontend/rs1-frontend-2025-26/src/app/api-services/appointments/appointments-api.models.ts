import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";

export interface AppointmentsForReviewDto {
    appointmentId: number;
    scheduledAtUtc: string;
}