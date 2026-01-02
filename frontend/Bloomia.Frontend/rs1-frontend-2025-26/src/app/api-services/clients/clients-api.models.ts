import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";
import { CreateJournalCommandDto } from "../journals/journals-api.models";
import { SubmitSelfTestCommandDto } from "../selfTests/selfTests-api.models";
import { SelfTestAnswersCommandDto } from "../selfTests/selfTests-api.models";

export interface GetClientProfileByIdQueryDTO{
    userId:number;
    clientId:number;
    firstname:string;
    lastname:string;
    username:string;
    gender:string;
    city:string;
    country:string;
    nativeLanguage:string;
    age:number;
    journals:CreateJournalCommandDto[];
    selfTests:SubmitSelfTestCommandDto[];
}