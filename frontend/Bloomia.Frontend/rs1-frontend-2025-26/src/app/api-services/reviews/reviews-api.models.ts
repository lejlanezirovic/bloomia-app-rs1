import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";

export interface CreateReviewCommand {
  appointmentId: number;
  rating: number;
  comment?: string | null;
}

export interface CreateReviewResponse {
  id: number;
}

export class GetReviewsByTherapistIdRequest extends BasePagedQuery {
  therapistId!: number;
}

export interface GetReviewsByTherapistIdQueryDto {
  id: number;
  rating: number;
  comment?: string | null;
  clientInitials: string;
  createdAt: string;
}

export type GetReviewsByTherapistIdResponse =
  PageResult<GetReviewsByTherapistIdQueryDto>;