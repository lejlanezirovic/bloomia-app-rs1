import { Component, inject, OnInit } from '@angular/core';
import { DirectChatsApiService } from '../../../../api-services/directChats/directChats-api.service';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ChatRealtimeService } from '../../../../core/services/chat-realtime.service';
import { ToasterService } from '../../../../core/services/toaster.service';
import { ListDirectChatMessagesQueryDto, GetDirectChatByIdClientQueryDto, SendMessageCommandDto, DeleteMessageCommandDto, MessageDto, ListDirectChatMessagesTherapistQueryDto, GetDirectChatByIdTherapistQueryDto, SendMessageTherapistCommandDto, DeleteMessageTherapistCommandDto, SendMessageTherapistCommand, UpdateMessageTherapistCommand } from '../../../../api-services/directChats/directChats-api.models';
import { FitConfirmDialogComponent } from '../../../shared/components/fit-confirm-dialog/fit-confirm-dialog.component';
import { DialogType, DialogButton, DialogResult } from '../../../shared/models/dialog-config.model';

@Component({
  selector: 'app-direct-chats-details',
  standalone: false,
  templateUrl: './direct-chats-details.component.html',
  styleUrl: './direct-chats-details.component.scss',
})
export class DirectChatsDetailsComponent implements OnInit{
  private apiService=inject(DirectChatsApiService);
  private route=inject(ActivatedRoute);
  private toasterService=inject(ToasterService);
  private dialog=inject(MatDialog);
  private chatRealtimeService=inject(ChatRealtimeService);

    clientId:number|null=null;
    directChat:ListDirectChatMessagesTherapistQueryDto|undefined;
  
    listOfChats:ListDirectChatMessagesTherapistQueryDto[]=[];
    directChatDetails:GetDirectChatByIdTherapistQueryDto|null=null;
  
    newMessage:string='';
    editingMessageId:number|null=null;

    isLoading=false;
    errorMessage:string|null=null;
  
    messageDTO:SendMessageTherapistCommandDto| null=null;
    chatDetails:GetDirectChatByIdTherapistQueryDto|null=null;
    directChatID:number|null=null;
  
    selectedMessage: any = null;
    deletedMessageDTO:DeleteMessageTherapistCommandDto|null=null;
  
    messagesDTOs:MessageDto[]=[];

  async ngOnInit():Promise<void> {
    this.clientId=Number(this.route.snapshot.paramMap.get('clientId'));
     const token = localStorage.getItem('accessToken');
     if (token) {
    await this.chatRealtimeService.startConnection(token);
     }
    this.route.queryParams.subscribe(params=>{
        this.directChatID=params['directChatId']?+params['directChatId']:null;

        if(this.directChatID){
            this.getDirectChatDetails(this.directChatID);
        }else{
          this.getAllChats();
        }
    });

    this.chatRealtimeService.messageDeleted$.subscribe(payload => {
    if (!this.directChatDetails) return;
    if (payload.directChatId !== this.directChatDetails.directChatId) return;

    this.directChatDetails.messages = this.directChatDetails.messages.map(m =>
      m.messageId === payload.messageId
        ? { ...m, content: payload.content }
        : m
    );
  });
  
  // RECEIVE
  this.chatRealtimeService.messageReceived$.subscribe(payload => {
    if (!this.directChatDetails) return;
    if (payload.directChatId !== this.directChatDetails.directChatId) return;

     this.directChatDetails.messages = [
    ...this.directChatDetails.messages,{
        messageId: payload.messageId,
        senderId: payload.senderId,
        senderType: payload.senderType,
        content: payload.content,
        sentAt: payload.sentAt,
        isRead:payload.isRead ?? false
     }
    ];
  });

    // READ
   this.chatRealtimeService.messagesRead$.subscribe(payload => {
    if (!this.directChatDetails) return;
    if (payload.directChatId !== this.directChatDetails.directChatId) return;

    this.directChatDetails.messages = this.directChatDetails.messages.map(m =>
      m.senderType === 'CLIENT'
        ? { ...m, isRead: true }
        : m
    );
    });

      // UPDATE
    this.chatRealtimeService.messageUpdated$.subscribe(payload => {
      if (!this.directChatDetails) return;
      if (payload.directChatId !== this.directChatDetails.directChatId) return;

      this.directChatDetails.messages = this.directChatDetails.messages.map(m =>
        m.messageId === payload.messageId
        ? { ...m, content: payload.content }
        : m
      );
    });

  }
  
  getDirectChatByClientId(){
    if(this.clientId!==null){
      const dChat=this.listOfChats.find(x=>x.clientId===this.clientId);
      if(dChat){
          this.directChat=dChat;
      }
    }
  }
  getAllChats(){
    this.apiService.getTherapistChats().subscribe({
      next:(res)=>{
          this.listOfChats=res;
          this.getDirectChatByClientId();

          if(this.directChat){
            this.getDirectChatDetails(this.directChat.directChatId);
          }
      },
      error:(err)=>{
        console.error(err);
      }
    })
  }
  getDirectChatDetails(id:number){
    this.isLoading=true;
    this.apiService.getTherapistChatById(id).subscribe({
      next:async (res)=>{
          this.directChatDetails=res;
          this.messagesDTOs=res.messages;
          this.isLoading=false;
          this.errorMessage=null;
          console.info(this.directChatDetails);
          console.info(res);
           
          await this.chatRealtimeService.joinDirectChatGroup(res.directChatId);
      },
      error:(err)=>{
        this.errorMessage="SOMETHING WENT WRONG WITH CHAT";
        console.error(err);
        this.isLoading=false;
      }
    });
  }
  sendMessage(){
    const inputContent=this.newMessage.trim();

    if(!inputContent){
      this.toasterService.error("ENTER A MESSAGE TO SEND");
      return;
    }
    if(this.editingMessageId!==null){
        this.updateMessage(inputContent);
        return;
    }

    const command:SendMessageTherapistCommand={
        clientId:this.clientId!,
        content:inputContent
      };
    this.apiService.sendMessageAsTherapist(command).subscribe({
        next:(res)=>{
          this.toasterService.success("MESSAGE SUCCESSFULLY SENT!");
          this.getDirectChatDetails(res.directChatId);
          console.info(res);
        },
        error:(err)=>{
          this.toasterService.error("SOMETHING WENT WRONG");
          console.error(err);
        }
    });
  }
  
  selectMessage(message: MessageDto) {
    this.selectedMessage = message;
  }
  editSelectedMessage(message: MessageDto){
      if(!this.selectMessage){
        return;
      }
      this.newMessage=this.selectedMessage.content;
      this.editingMessageId=this.selectedMessage.messageId;
  }
  updateMessage(content:string){
    if(this.editingMessageId===null)
      return;
    
    const command:UpdateMessageTherapistCommand={
      newContent:content,
      directChatId:this.directChatDetails!.directChatId,
      messageId:this.editingMessageId
    };
    this.apiService.updateMessageAsTherapist(command).subscribe({
      next:(res)=>{
        this.toasterService.info("MESSAGE UPDATED!");
        this.newMessage="";
        this.editingMessageId=null;
        this.selectedMessage=null;
        this.getDirectChatDetails(res.directChatId);
      },
      error: (err) => {
        this.toasterService.error('UPDATE FAILED');
        console.error(err);
      }
    });
  }
  
  cancelEdit() {
  this.editingMessageId = null;
  this.newMessage = '';
  this.selectedMessage = null;
  }
  deleteMessage(){
    if (!this.selectedMessage)
        return;
    const messageId = this.selectedMessage.messageId;
    this.apiService.deleteMessageAsTherapist(messageId).subscribe({
      next:(res)=>{
          this.deletedMessageDTO=res;
          this.toasterService.success("MESSAGE DELETED");
          this.directChatDetails!.messages = this.directChatDetails!.messages.map(m=>
              m.messageId === messageId
              ? { ...m, content: 'This message was deleted!' }
              : m
          );
          this.selectedMessage=null;
      },
      error:(err)=>{
        this.toasterService.error("Something went wrong. Try again!");
        console.error(err);
      }
    });
  }
  openConfirmDialogToRemoveTheMessage(){
    const dialogRef = this.dialog.open(FitConfirmDialogComponent, {
      width: '400px',
      data: {
      type: DialogType.WARNING,
      title: 'Confirm removing the message',
      message: 'Are you sure you want to remove this message?',
      buttons: [
        {
          type: DialogButton.CANCEL,
          color: 'primary'
        },
        {
          type: DialogButton.DELETE,
          color: 'warn',
          label: 'Yes, remove'
        }
      ]
      }
    });

    dialogRef.afterClosed().subscribe((result: DialogResult | undefined) => {
      if (result?.button === DialogButton.DELETE) {
          this.deleteMessage();
      }
    });
  }
}
