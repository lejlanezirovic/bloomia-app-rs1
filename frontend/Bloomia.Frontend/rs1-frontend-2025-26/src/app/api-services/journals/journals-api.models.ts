
export interface CreateJournalCommandDto{
    journalId:number;
    journalName:string;
    createdAt:string;
    journalAnswers:CreateJournalAnswerCommandDto[];
}
export interface CreateJournalAnswerCommandDto{
    questionId:number;
    questionText:string;
    answerText:string;

}
export interface CreateJournalCommand{
    title:string;
    clientsAnswers:CreateJournalAnswerCommandDto[];
}
export interface ListQuestionsQuery{

}
export interface ListQuestionsQueryDto{
    listOfQuestions:ListQuestions[];
}
export interface ListQuestions{
    questionId:number;
    question:string;

}