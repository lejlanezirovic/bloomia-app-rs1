import { Component, inject, OnInit } from '@angular/core';
import { DirectChatsApiService } from '../../../../api-services/directChats/directChats-api.service';
import { ActivatedRoute } from '@angular/router';
import { DeleteMessageCommand, DeleteMessageCommandDto, GetDirectChatByIdClientQueryDto, ListDirectChatMessagesQueryDto, MessageDto, SendMessageCommand, SendMessageCommandDto, UpdateMessageCommand } from '../../../../api-services/directChats/directChats-api.models';
import { ToasterService } from '../../../../core/services/toaster.service';
import { FitConfirmDialogComponent } from '../../../shared/components/fit-confirm-dialog/fit-confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { DialogButton, DialogResult, DialogType } from '../../../shared/models/dialog-config.model';
import { ChatRealtimeService } from '../../../../core/services/chat-realtime.service';

@Component({
  selector: 'app-direct-chats-details',
  standalone: false,
  templateUrl: './direct-chats-details.component.html',
  styleUrl: './direct-chats-details.component.scss',
})
export class DirectChatsDetailsComponent implements OnInit {
  private apiService=inject(DirectChatsApiService);
  private route=inject(ActivatedRoute);
  private toasterService=inject(ToasterService);
  private dialog=inject(MatDialog);
  private chatRealtimeService=inject(ChatRealtimeService);

  therapistId:number|null=null;
  directChat:ListDirectChatMessagesQueryDto|undefined;

  listOfChats:ListDirectChatMessagesQueryDto[]=[];
  directChatDetails:GetDirectChatByIdClientQueryDto|null=null;

  newMessage:string='';
  editingMessageId:number|null=null;

  isLoading=false;
  errorMessage:string|null=null;

  messageDTO:SendMessageCommandDto| null=null;
  chatDetails:GetDirectChatByIdClientQueryDto|null=null;
  directChatID:number|null=null;

  selectedMessage: any = null;
  deletedMessageDTO:DeleteMessageCommandDto|null=null;

  messagesDTOs:MessageDto[]=[];

  async ngOnInit(): Promise<void> {
    this.therapistId=Number(this.route.snapshot.paramMap.get('therapistId'));
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


  getDirectChatByTherapistId(){
    if(this.therapistId!==null){
      const dChat=this.listOfChats.find(x=>x.therapistId===this.therapistId);
      if(dChat){
          this.directChat=dChat;
      }
    }
  }
  getAllChats(){
    this.apiService.getClientChats().subscribe({
      next:(res)=>{
          this.listOfChats=res;
          this.getDirectChatByTherapistId();

          if(this.directChat){
            this.getDirectChatDetails(this.directChat.directChatId);
          }
      },
      error:(err)=>{
        console.error(err);
      }
    });
  }

  markChatAsRead(directChatId: number): void {
    this.apiService.markChatAsRead(directChatId).subscribe({
      error: (err) => {
        console.error('Failed to mark chat as read', err);
      }
    });
  }

  getDirectChatDetails(id:number){
    this.isLoading=true;

    this.apiService.getClientChatById(id).subscribe({
      next:async (res)=>{
          this.directChatDetails=res;
          this.messagesDTOs=res.messages;
          this.isLoading=false;
          this.errorMessage=null;
          console.info(this.directChatDetails);
          console.info(res);

          await this.chatRealtimeService.joinDirectChatGroup(res.directChatId);

          this.markChatAsRead(id);
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
    const command:SendMessageCommand={
        therapistId:this.therapistId!,
        content:inputContent
      };
      this.apiService.sendMessageAsClient(command).subscribe({
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
  //when I click on a message I want to be offered two options: edit and delete
  //if edit is chosen the message goes back into the input and can be edited; clicking send or pressing enter on the keyboard updates it and sends it again
  //if delete is chosen the dialog helper pops up so I confirm I want to delete it, and once confirmed the message is deleted.

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
    const command:UpdateMessageCommand={
      newContent:content,
      directChatId:this.directChatDetails!.directChatId,
      messageId:this.editingMessageId
    };
    this.apiService.updateMessageAsClient(command).subscribe({
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
    this.apiService.deleteMessageAsClient(messageId).subscribe({
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
