import { HttpClient } from "@angular/common/http";
import {inject, Injectable} from "@angular/core";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";
import { DeleteMessageCommandDto, DeleteMessageTherapistCommandDto, GetDirectChatByIdClientQuery, GetDirectChatByIdClientQueryDto, GetDirectChatByIdTherapistQueryDto, ListDirectChatMessagesQueryDto, ListDirectChatMessagesTherapistQueryDto, SendMessageCommand, SendMessageCommandDto, SendMessageTherapistCommand, SendMessageTherapistCommandDto, UpdateMessageCommand, UpdateMessageCommandDto, UpdateMessageTherapistCommand, UpdateMessageTherapistCommandDto } from "./directChats-api.models";

@Injectable({
    providedIn:'root'
})
export class DirectChatsApiService{
    private baseUrl=`${environment.apiUrl}/api/DirectChat`;
    private http=inject(HttpClient);

    sendMessageAsClient(command:SendMessageCommand):Observable<SendMessageCommandDto>{
        return this.http.post<SendMessageCommandDto>(`${this.baseUrl}/client/messages/send`, command);
    }
    sendMessageAsTherapist(command:SendMessageTherapistCommand):Observable<SendMessageTherapistCommandDto>{
        return this.http.post<SendMessageTherapistCommandDto>(`${this.baseUrl}/therapist/messages/send`, command);
    }

    updateMessageAsClient(command:UpdateMessageCommand):Observable<UpdateMessageCommandDto>{
        return this.http.put<UpdateMessageCommandDto>(`${this.baseUrl}/client/messages/update-msgById`, command);
    }
    updateMessageAsTherapist(command:UpdateMessageTherapistCommand):Observable<UpdateMessageTherapistCommandDto>{
        return this.http.put<UpdateMessageTherapistCommandDto>(`${this.baseUrl}/therapist/messages/update-msgById`, command);
    }

    deleteMessageAsClient( messageId:number):Observable<DeleteMessageCommandDto>{
        return this.http.delete<DeleteMessageCommandDto>(`${this.baseUrl}/client/direct-chat/messages/delete/${messageId}`);
    }
    deleteMessageAsTherapist( messageId:number):Observable<DeleteMessageTherapistCommandDto>{
        return this.http.delete<DeleteMessageTherapistCommandDto>(`${this.baseUrl}/therapist/direct-chat/messages/delete/${messageId}`);
    }

    getClientChats():Observable<ListDirectChatMessagesQueryDto[]>{
        return this.http.get<ListDirectChatMessagesQueryDto[]>(`${this.baseUrl}/client/direct-chats`);
    }
    getTherapistChats():Observable<ListDirectChatMessagesTherapistQueryDto[]>{
        return this.http.get<ListDirectChatMessagesTherapistQueryDto[]>(`${this.baseUrl}/therapist/direct-chats`);
    }
    
    getClientChatById(directChatId:number):Observable<GetDirectChatByIdClientQueryDto>{
        return this.http.get<GetDirectChatByIdClientQueryDto>(`${this.baseUrl}/client/direct-chat/${directChatId}`);
    }
    getTherapistChatById(directChatId:number):Observable<GetDirectChatByIdTherapistQueryDto>{
        return this.http.get<GetDirectChatByIdTherapistQueryDto>(`${this.baseUrl}/therapist/direct-chat/${directChatId}`);
    }
}