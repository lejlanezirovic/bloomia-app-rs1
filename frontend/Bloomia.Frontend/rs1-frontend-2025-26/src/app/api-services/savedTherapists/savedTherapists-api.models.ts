import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";

export interface AddTherapistToSavedTherapistsCommand{
    therapistId: number;
}
export interface AddTherapistToSavedTherapistsCommandDto{

    therapistId:number;
    fullname:string;
    specialization:string;
    description:string;
    ratingAvg:number;
    myTherapyTypes:ListTherapistTherapyTypesQueryDto[];


}
export interface ListTherapistTherapyTypesQueryDto{

    therapistId:number;
    therapyTypeName:string;
}
export class ListSavedTherapistsQuery extends BasePagedQuery{
    fullName?: string | null;
    specialization?: string | null;
    minRating?: number | null;
    therapyType?: string | null;
    sortByRatingDesc?: boolean;
}
export type ListSavedTherapistsResponse=PageResult<ListSavedTherapistInfoDto>;


export interface ListSavedTherapistsQueryDto{
    savedTherapists:ListSavedTherapistInfoDto[];

}
export interface ListSavedTherapistInfoDto{
    therapistId:number;
    fullname:string;
    specialization:string;
    description:string;
    ratingAvg:number;
    myTherapyTypes:ListTherapistTherapyTypesQueryDto[];
}
export interface RemoveSavedTherapistCommand{
    therapistId:number;
}
export interface RemoveAllSavedTherapistsCommand{

}
export interface GetSavedTherapistByNameCommand{
    serachName:string;
}
export interface GetSavedTherapistByNameCommandDto{
    therapistId:number;
    therapistProfilePictureUrl:string;
    fullName:string;
    specialization:string;
    description:string;
    ratingAvg:number;
    myTherapyTypes:ListTherapistTherapyTypesQueryDto[];
}
export type GetSavedTherapistByNameResponse= GetSavedTherapistByNameCommandDto[];