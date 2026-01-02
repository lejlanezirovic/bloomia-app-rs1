
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