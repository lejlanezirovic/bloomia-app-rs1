import { Component,inject,OnInit } from '@angular/core';
import { SelfTestsApiService } from '../../../../api-services/selfTests/selfTests-api.service';
import { GetSelfTestByIdQueryDto, ListAllSelfTestsQueryDto,ListSelfTestQueryDto,ListSelfTestQuerySelfTestQuestionsDto } from '../../../../api-services/selfTests/selfTests-api.models';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-self-test-details',
  standalone: false,
  templateUrl: './self-test-details.component.html',
  styleUrl: './self-test-details.component.scss',
})
export class SelfTestDetailsComponent implements OnInit{
   private apiService=inject(SelfTestsApiService);
   private route=inject(ActivatedRoute);
  private router=inject(Router);

   test:GetSelfTestByIdQueryDto|null=null;
   isLoading=false;
   errorMessage:string|null=null;

  ngOnInit(): void {
    const id=Number(this.route.snapshot.paramMap.get('id'));
    this.getSelfTestById(id);
  }

  getSelfTestById(testId:number){
    this.isLoading=true;
    this.errorMessage=null;

    this.apiService.getSelfTestById(testId).subscribe({
      next:(response)=>{
        this.test=response;
        this.isLoading=false;
      },
      error:(err)=>{
        this.errorMessage='Failed to load self test.';
        console.error(err);
      }
    });
  }

  showSelfTestToSubmit(id:number){
    this.router.navigate(['client/self-tests/submit-self-test', id]);
  }
}
