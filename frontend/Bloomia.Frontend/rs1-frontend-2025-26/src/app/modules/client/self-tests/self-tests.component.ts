import { Component, inject,OnInit } from '@angular/core';
import { SelfTestsApiService } from '../../../api-services/selfTests/selfTests-api.service';
import { Router } from '@angular/router';
import { ListAllSelfTestsQueryDto } from '../../../api-services/selfTests/selfTests-api.models';

@Component({
  selector: 'app-self-tests',
  standalone: false,
  templateUrl: './self-tests.component.html',
  styleUrl: './self-tests.component.scss',
})
export class SelfTestsComponent implements OnInit {

  private apiService=inject(SelfTestsApiService);
  private router=inject(Router);

  selfTests: ListAllSelfTestsQueryDto={
    allSelfTests:[]
  };
  isLoading=false;
  errorMessage:string|null=null;

  ngOnInit(): void {
    this.loadAllSelfTest();
  }

  loadAllSelfTest():void{
    this.isLoading=true;
    this.errorMessage=null;
    this.apiService.getAllSelfTests().subscribe({
      next:(response)=>{
        this.selfTests=response;
        this.isLoading=false;

      },
      error:(err)=>{
        this.errorMessage='Failed to load self tests';
        console.error(err);
      }
    });
  }
  showSelfTestByid(id:number){
      this.router.navigate(['client/self-tests/self-test-details',id]);
  }//ne moze se injektovati druga komponenta. samo navigacija. 
}

