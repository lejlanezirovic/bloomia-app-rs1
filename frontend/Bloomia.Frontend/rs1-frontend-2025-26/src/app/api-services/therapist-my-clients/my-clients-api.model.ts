import { BasePagedQuery } from "../../core/models/paging/base-paged-query"
import { PageResult } from "../../core/models/paging/page-result";

export class ListMyClientsRequest extends BasePagedQuery {
    search?: string | null;
}

export interface ListMyClientsQueryDto {
    clientId: number;
    fullName: string;
    email?: string | null;
    profileImage?: string | null;
    nextAppointmentAtUtc?: string | null;
}

export type ListMyClientsResponse = PageResult<ListMyClientsQueryDto>