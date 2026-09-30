import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { PageResult } from "../../core/models/paging/page-result";

export interface SubmitSelfTestCommandDto{
    selfTestId:number;
    selfTestName:string;
    selfTestAnswers:SelfTestAnswersCommandDto[];
    testAverage:number;
    resultDescription:string;
    clientNote?:string;
}
export interface SelfTestAnswersCommandDto{
    questionId:number;
    questionName?:string;
    rating: number;
}

export interface SubmitSelfTestCommand{
    testId:number;
    testName?:string;
    testAnswers:SelfTestAnswersCommandDto[];
    clientNote?:string|null;
}

export interface  ListAllSelfTestsQueryDto{
    allSelfTests:ListSelfTestQueryDto[];
}
export interface ListSelfTestQueryDto{
    testId:number;
    selfTestName:string;
    selfTestQuestions:ListSelfTestQuerySelfTestQuestionsDto[];
}
export interface ListSelfTestQuerySelfTestQuestionsDto{
    question:string;
    questionId:number;
}

export interface GetSelfTestByIdQueryDto{
    id:number;
    selfTestName:string;
    selfTestQuestions:GetSelfTestByIdQueryQuestionsDto[];
}
export interface GetSelfTestByIdQueryQuestionsDto{
    questionId:number;
    question:string;
}

export class ListMySelfTestResultsQuery extends BasePagedQuery {
  testName?: string | null = null;
  completedFrom?: string | null = null;
  completedTo?: string | null = null;
  minAverage?: number | null = null;
  maxAverage?: number | null = null;
  sortByDateDesc?: boolean = true;
}

export interface ListMySelfTestResultDto {
  resultId: number;
  selfTestId: number;
  selfTestName: string;
  completedAt: string;
  averageScore: number;
  description?: string;
  clientNote?: string;
}

export type ListMySelfTestResultsResponse = PageResult<ListMySelfTestResultDto>;

export interface GetMySelfTestResultByIdDto {
  resultId: number;
  selfTestId: number;
  selfTestName: string;
  completedAt: string;
  averageScore: number;
  description?: string;
  clientNote?: string;
  answers: SelfTestAnswersCommandDto[];
}

export interface UpdateSelfTestResultNoteCommand {
  clientNote: string;
}

export interface UpdateSelfTestResultNoteCommandDto {
  resultId: number;
  clientNote: string;
}