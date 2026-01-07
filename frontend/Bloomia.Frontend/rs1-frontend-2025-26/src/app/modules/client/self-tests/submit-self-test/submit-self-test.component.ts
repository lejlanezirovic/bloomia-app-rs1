import { Component, inject, OnInit } from '@angular/core';
import { SelfTestsApiService } from '../../../../api-services/selfTests/selfTests-api.service';
import { ActivatedRoute, Router} from '@angular/router';
import { SubmitSelfTestCommand,SubmitSelfTestCommandDto,SelfTestAnswersCommandDto, GetSelfTestByIdQueryDto } from '../../../../api-services/selfTests/selfTests-api.models';

@Component({
  selector: 'app-submit-self-test',
  standalone: false,
  templateUrl: './submit-self-test.component.html',
  styleUrl: './submit-self-test.component.scss',
})
export class SubmitSelfTestComponent implements OnInit {

  private apiService=inject(SelfTestsApiService);
  private router=inject(Router);
  private activeRoute=inject(ActivatedRoute);

  selfTest:GetSelfTestByIdQueryDto|null=null;
  answersMap:{[questionId:number]:number}={};// mapiranje pitanje- ocjena (q_id) 1: 5 (rating)

  selfTestAnswers:SelfTestAnswersCommandDto[]=[];

  errorMessage:string|null=null;
  isLoading=false;

  submitSuccess:boolean=false;
  submitResponse: SubmitSelfTestCommandDto|null=null;

  ngOnInit(): void {
    const testId=Number(this.activeRoute.snapshot.paramMap.get('id'));
    this.showSelfTestById(testId);
  }
  showSelfTestById(testId:number){
    this.isLoading=true;
    this.errorMessage=null;

    this.apiService.getSelfTestById(testId).subscribe({
      next: (response)=>{
        this.selfTest=response;
        this.isLoading=false;
      
      },
      error:(err)=>{
        this.errorMessage='failed to load self test for implementation.';
        console.error(err);
      }
    });
  }

  onRatingChange(questionId:number, event:Event){
      const rating=Number((event.target as HTMLInputElement).value);//ocjena iz inputa
      if(rating>=1 && rating<=5){
        this.answersMap[questionId]=rating;
      }
  }//cuvamo stanje

  submitSelfTest(){
    if(!this.selfTest)
      return;

     const unansweredQuestions=this.selfTest.selfTestQuestions.filter(q=> this.answersMap[q.questionId]==null);
     if(unansweredQuestions.length>0){
      this.errorMessage='Please enter rate for all questions!';
      return;
     }


    const answers:SelfTestAnswersCommandDto[]=this.selfTest?.selfTestQuestions.map(q=> ({
       questionId: q.questionId,
       questionName:q.question,
       rating:this.answersMap[q.questionId]??1
    }) );

    const command:SubmitSelfTestCommand={
      testId:this.selfTest.id,
      testName:this.selfTest.selfTestName,
      testAnswers:answers
    };

    this.apiService.submitSelfTest(command).subscribe({
      next: (response)=>{
        this.submitResponse=response;
        this.submitSuccess=true;
        console.log('rezultati testa:', response);
      },
      error:(err)=>{
        this.errorMessage='Failed to  submit self test.';
      }
    });
  }
  isFormValid(): boolean {
    if (!this.selfTest) return false;

    return this.selfTest.selfTestQuestions.every(
      q => this.answersMap[q.questionId] != null
    );
  }
//e sada trebamo uzeti (on change maybe na inputu?) sve ratinge iz inputa 
//moram kreirari answerDto preuzeti jedan po jedan rating i q_id

//1. trebamo mapirati qid i rating
//slusati i reagovati na event -> kad neko upise ocjenu onda mapirati
}