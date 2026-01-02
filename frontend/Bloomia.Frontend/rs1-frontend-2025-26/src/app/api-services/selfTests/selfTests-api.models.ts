
export interface SubmitSelfTestCommandDto{
    selfTestId:number;
    selfTestName:string;
    selfTestAnswers:SelfTestAnswersCommandDto[];
    testAverage:number;
    resultDescription:string;
}
export interface SelfTestAnswersCommandDto{
    questionId:number;
    questionName:string;
    rating: number;
}