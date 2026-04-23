import { Component, inject, OnInit } from '@angular/core';
import { DirectChatsApiService } from '../../../api-services/directChats/directChats-api.service';
import { Router } from '@angular/router';
import { GetDirectChatByIdTherapistQueryDto, ListDirectChatMessagesTherapistQuery, ListDirectChatMessagesTherapistQueryDto, MessageDto } from '../../../api-services/directChats/directChats-api.models';

@Component({
  selector: 'app-direct-chats',
  standalone: false,
  templateUrl: './direct-chats.component.html',
  styleUrl: './direct-chats.component.scss',
})
export class DirectChatsComponent implements OnInit {
  private apiService=inject(DirectChatsApiService);
  private router=inject(Router);

  listChats:ListDirectChatMessagesTherapistQueryDto[]=[];
  isLoading=false;
  errorMessage:string|null=null;

  messages:MessageDto[]=[];
  directChat:GetDirectChatByIdTherapistQueryDto|null=null;

  ngOnInit(): void {
   this.getAllDirectChatsForTherapist();
  }

  getAllDirectChatsForTherapist(){
    this.isLoading=true;
     this.errorMessage=null;

     this.apiService.getTherapistChats().subscribe({
      next:(res)=>{
        this.listChats=res;
        this.isLoading=false;
        this.errorMessage=null;
      },
      error:(err)=>{
        this.errorMessage="Failed to load your chats";
        this.isLoading=false;
        console.error(err);
      }
     })
  }
  getChatByIdForTherapist(directChatId:number){
    this.apiService.getTherapistChatById(directChatId).subscribe({
      next:(res)=>{
          this.directChat=res;
          this.messages=res.messages;
      },
      error:(err)=>{
        this.errorMessage="Failed to load chat";
        this.isLoading=false;
        console.error(err);
      }
    });
  }
  getInitials(fullname: string): string {
    return fullname
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map(part => part[0]?.toUpperCase() ?? '')
      .join('');
  }

  getPreviewText(isReadLastMessage: boolean): string {
    return isReadLastMessage
      ? 'Open the conversation and continue where you left off.'
      : 'You have a new message waiting.';
  }
  redirectToChatDetails(chat: ListDirectChatMessagesTherapistQueryDto){
      const clientId=chat.clientId;
      this.router.navigate([`therapist/direct-chats/${clientId}/direct-chats-details`], {
        queryParams:{directChatId:chat.directChatId}
      });
    }
}
