import { Component, inject, OnInit } from '@angular/core';
import { ClientsApiService } from '../../../api-services/clients/clients-api.service';
import { CreateJournalCommandDto } from '../../../api-services/journals/journals-api.models';


@Component({
  selector: 'app-my-journals',
  standalone: false,
  templateUrl: './my-journals.component.html',
  styleUrl: './my-journals.component.scss',
})
export class MyJournalsComponent implements OnInit {

  private apiService=inject(ClientsApiService);

  journals:CreateJournalCommandDto[]=[];
  isLoading=true;
  errorMessage:string|null=null;

  ngOnInit(): void {
    this.loadJournals();
  }
  loadJournals():void{
    this.isLoading=true;
    this.errorMessage=null;

    this.apiService.getMyProfile().subscribe({
      next: (client)=>{
        this.journals=client.journals?? [];
        this.isLoading=false;
      },
      error:(err)=>{
        this.errorMessage='Failed to load your journals. ';
        console.error(err);
      }
    });
  }
}
