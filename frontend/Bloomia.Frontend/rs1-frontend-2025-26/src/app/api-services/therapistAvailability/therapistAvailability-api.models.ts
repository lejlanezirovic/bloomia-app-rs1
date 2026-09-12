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

export interface CreateTherapistAvailabilityCommand {
    availableDate: string;
    startTime: string;
}

export interface CreateTherapistAvailabilityCommandDto {
    note: string;
    date: string;
    time: string;
}