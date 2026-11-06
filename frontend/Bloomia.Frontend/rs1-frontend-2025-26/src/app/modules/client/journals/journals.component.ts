import { Component, inject, OnInit } from '@angular/core';
import { JournalsApiService } from '../../../api-services/journals/journals-api.service';
import { Router } from '@angular/router';
import { CreateJournalCommand, CreateJournalCommandDto,CreateJournalAnswerCommandDto, ListQuestionsQueryDto } from '../../../api-services/journals/journals-api.models';

@Component({
  selector: 'app-journals',
  standalone: false,
  templateUrl: './journals.component.html',
  styleUrl: './journals.component.scss',
})
export class JournalsComponent implements OnInit {

  private apiService=inject(JournalsApiService);
  private router=inject(Router);

  questions:ListQuestionsQueryDto={
      listOfQuestions:[]
  }
  isLoading=false;
  errorMessage:string|null=null;
  ngOnInit(): void {
    this.showJournalQuestions();
  }
  // 1. Load the questions
  showJournalQuestions(){
    this.isLoading=true;
    this.errorMessage=null;

    this.apiService.getJournalQuestions().subscribe({
      next:(response)=>{
        this.questions=response;
        this.isLoading=false;
      },
      error:(err)=>{
        this.errorMessage="Failed to load journal questions";
        console.error(err);
      }
    });
  }

  createAJournal(){
    this.router.navigate(['client/journals/journal-details']);
  }
}
