
export interface SubmitSelfTestCommandDto{
    selfTestId:number;
    selfTestName:string;
    selfTestAnswers:SelfTestAnswersCommandDto[];
    testAverage:number;
    resultDescription:string;
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

    questionName:string;
    rating: number;


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