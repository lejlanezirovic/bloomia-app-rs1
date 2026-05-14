import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";

export class ListTherapistsRequest extends BasePagedQuery {
    firstname?: string | null;
    lastname?: string | null;
    specialiation?: string | null;
    genderId?: number | null;
    sortByRatingDesc?: boolean;
}

export interface ListTherapistsQueryDto {
    id: number;
    fullname?: string | null;
    specialization?: string | null;
    ratingAvg: number;
    profileImage?: string | null;
    gender?: string | null;
}

export type ListTherapistsResponse = PageResult<ListTherapistsQueryDto>;

export interface GetTherapistByIdQueryDto {
    id: number;
    firstname?: string | null;
    lastname?: string | null;
    username?: string | null;
    fullname?: string | null;
    email?: string | null;
    phoneNumber?: string | null;
    profileImage?: string | null;
    specialization?: string | null;
    description?: string | null;
    ratingAvg: number;
    documents: TherapistDocumentDto[];
    isVerified: boolean | null;
    therapyTypes: TherapyTypeDto[],
    availability: TherapistAvailabilityDto[];
}

export interface TherapistDocumentDto {
    id: number;
    documentType: string;
    fileName: string;
    filePath: string;
}

export interface TherapistAvailabilityDto {
    id: number;
    date: string;
    startTime: string;
    isBooked?: boolean | null;
} 

export interface TherapyTypeDto {
    id: number;
    name?: string | null;
}


export interface UpdateTherapistCommand {
    firstname?: string | null;
    lastname?: string | null;
    specialization?: string | null;
    description?: string | null;
    therapyTypeIds?: number[] | null;
    email?: string | null;
    phoneNumber?: string | null;
    locationId?: number | null;
}

export interface ChangeTherapistPasswordCommand {
    id: number;
    currentPassword?: string | null;
    newPassword?: string | null;
}