import { Injectable, inject } from "@angular/core";
import * as signalR from '@microsoft/signalr';
import { Subject, BehaviorSubject } from "rxjs";
import { environment } from "../../../environments/environment";
import { AuthStorageService } from "./auth/auth-storage.service";

export interface ReceiveMessageEvent {
  directChatId: number;
  messageId:number;
  senderId: number;
  senderType: string;
  content: string;
  sentAt: string;
  isRead:boolean;
}
export interface MessageUpdatedEvent{
    directChatId:number;
    messageId:number;
    isDeleted:boolean;
    content:string;
}
export interface MessageDeletedEvent{
    directChatId:number;
    messageId:number;
    isDeleted:boolean;
    content:string;
}
export interface MessagesReadEvent{
   directChatId:number;
   messageIds:number[];
}
@Injectable({
  providedIn: 'root'
})
export class ChatRealtimeService{
    private authStorage=inject(AuthStorageService);
    private hubConnection:signalR.HubConnection |null=null;
    private baseUrl=`${environment.apiUrl}/chat`;

    private messageReceivedSubject=new Subject<ReceiveMessageEvent>();
    public messageReceived$=this.messageReceivedSubject.asObservable();

    private messageUpdatedSubject=new Subject<MessageUpdatedEvent>();
    public messageUpdated$=this.messageUpdatedSubject.asObservable();

    private messageDeletedSubject=new Subject<MessageDeletedEvent>();
    public messageDeleted$=this.messageDeletedSubject.asObservable();

    private messagesReadSubject = new Subject<MessagesReadEvent>();
    public messagesRead$ = this.messagesReadSubject.asObservable();

    private connectionStateSubject=new BehaviorSubject<'disconnected'| 'connecting'|'connected'>('disconnected');
    public connectionState=this.connectionStateSubject.asObservable();

    async startConnection(token:string):Promise<void>{
        if(this.hubConnection)
            return;

        this.connectionStateSubject.next('connecting');
        this.hubConnection=new signalR.HubConnectionBuilder().withUrl(this.baseUrl,{
            // Read the currently valid access token from storage on every call, not just
            // the token captured when startConnection() was first invoked. withAutomaticReconnect()
            // calls accessTokenFactory again on every reconnect attempt, so this keeps reconnects
            // in sync with tokens refreshed later by the auth interceptor.
            accessTokenFactory:()=>this.authStorage.getAccessToken() ?? token}).withAutomaticReconnect().build();

       this.registerListeners();
       await this.hubConnection.start();
       this.connectionStateSubject.next('connected');
    }
    private registerListeners():void{
        if (!this.hubConnection) return;

        this.hubConnection.on('ReceiveMessage', (payload:ReceiveMessageEvent)=>{
            this.messageReceivedSubject.next(payload);
            console.log('SIGNALR RECEIVE-> :', payload);
        });

        this.hubConnection.on('MessageUpdated', (payload:MessageUpdatedEvent)=>{
            this.messageUpdatedSubject.next(payload);
        });

        this.hubConnection.on('MessageDeleted', (payload:MessageDeletedEvent)=>{
            this.messageDeletedSubject.next(payload);
        });

        this.hubConnection.on('MessagesRead', (payload:MessagesReadEvent)=>{
            this.messagesReadSubject.next(payload);
        });
    }
    async joinDirectChatGroup(directChatId:number):Promise<void>{
        if (!this.hubConnection) return;
        await this.hubConnection.invoke('JoinDirectChatGroup', directChatId);
    }
     
    async leaveDirectChatGroup(directChatId: number): Promise<void> {
    if (!this.hubConnection) return;
    await this.hubConnection.invoke('LeaveDirectChatGroup', directChatId);
    }

    async stopConnection():Promise<void>{
        if(!this.hubConnection)
            return;
        await this.hubConnection.stop();
        this.hubConnection=null;
        this.connectionStateSubject.next('disconnected');
    }

}