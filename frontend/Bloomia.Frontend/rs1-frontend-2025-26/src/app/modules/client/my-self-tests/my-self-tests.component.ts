import { Component, inject,OnInit } from '@angular/core';
import { ClientsApiService } from '../../../api-services/clients/clients-api.service';
import { SubmitSelfTestCommandDto } from '../../../api-services/selfTests/selfTests-api.models';

@Component({
  selector: 'app-my-self-tests',
  standalone: false,
  templateUrl: './my-self-tests.component.html',
  styleUrl: './my-self-tests.component.scss',
})
export class MySelfTestsComponent implements OnInit {

  private apiService=inject(ClientsApiService);

  selfTests:SubmitSelfTestCommandDto[]=[];
  isLoading=true;
  errorMessage:string| null=null;

  ngOnInit(): void {
    this.loadSelfTests();
  }
  loadSelfTests():void{
    this.isLoading=true;
    this.errorMessage=null;

      this.apiService.getMyProfile().subscribe({
        next:(client)=>{
          this.selfTests=client.selfTests??[];
          this.isLoading=false;
        },
        error:(err)=>{
          this.errorMessage='Failed to load self tests :/';
          this.isLoading=false;
          console.error(err);
        }
      });
  }
}
