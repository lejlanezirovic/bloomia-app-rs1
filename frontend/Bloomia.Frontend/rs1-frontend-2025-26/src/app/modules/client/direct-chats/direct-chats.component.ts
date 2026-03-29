import { Component, inject, OnInit } from '@angular/core';
import { DirectChatsApiService } from '../../../api-services/directChats/directChats-api.service';
import { GetDirectChatByIdClientQueryDto, ListDirectChatMessagesQueryDto, MessageDto } from '../../../api-services/directChats/directChats-api.models';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-direct-chats',
  standalone: false,
  templateUrl: './direct-chats.component.html',
  styleUrl: './direct-chats.component.scss',
})
export class DirectChatsComponent implements OnInit{
  private apiService=inject(DirectChatsApiService);
  private router=inject(Router);

  listChats:ListDirectChatMessagesQueryDto[]=[];
  isLoading=false;
  errorMessage:string|null=null;

  messages:MessageDto[]=[];
  directChat:GetDirectChatByIdClientQueryDto|null=null;

  ngOnInit(): void {
    this.getAllDirectChats();
  }

  getAllDirectChats(){
    this.isLoading=true;
     this.errorMessage=null;

     this.apiService.getClientChats().subscribe({
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
     });
  }
  getChatById(directChatId:number){
    this.apiService.getClientChatById(directChatId).subscribe({
      next:(res)=>{
          this.directChat=res;
          this.messages=res.messages;
      },
      error:(err)=>{
        this.errorMessage="Failed to load mesages in chat";
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
  
  redirectToChatDetails(chat: ListDirectChatMessagesQueryDto){
    const therapistId=chat.therapistId;
    this.router.navigate([`client/direct-chats/${therapistId}/details`], {
      queryParams:{directChatId:chat.directChatId}
    });
  }
}
