import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";


export interface ListMyWorkingDatesAndTimesResponse {
    therapistId: number;
    workingDates: WorkingDateDto[];
}

export interface WorkingDateDto {
    date: string;
    allSlotsOfDate: WorkingTimeSlotsDto[];
}

export interface WorkingTimeSlotsDto {
    therapistAvailabilityId: number;
    startTime: string;
    isBooked: boolean;
}