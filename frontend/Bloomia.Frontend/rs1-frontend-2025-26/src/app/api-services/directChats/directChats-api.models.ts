export interface SendMessageCommand{
    therapistId:number;
    content:string;
}
export interface SendMessageCommandDto{
    note:string;
    message:string;
    sentAt:string;
    directChatId:number;
    messageId:number;
}
export interface SendMessageTherapistCommandDto{
    note:string;
    message:string;
    sentAt:string;
    directChatId:number;
    messageId:number;
}
export interface SendMessageTherapistCommand{
    clientId:number;
    content:string;
}
export interface DeleteMessageCommand{
   directChatId:number;
    messageId:number;
}
export interface DeleteMessageCommandDto{
    isDeleted :boolean;
    message:string;
    senderType:string;
}
export interface DeleteMessageTherapistCommand{
   directChatId:number;
    messageId:number;
}
export interface DeleteMessageTherapistCommandDto{
    isDeleted :boolean;
    message:string;
    senderType:string;
}

export interface UpdateMessageCommand{ 
     directChatId:number;
     messageId:number;
     newContent :string;
}
export interface UpdateMessageCommandDto{
     directChatId:number;
     messageId:number;
     updatedContent:string;
     oldMessage:string;
     isRead:boolean;
}

export interface UpdateMessageTherapistCommand{ 
     directChatId:number;
     messageId:number;
     newContent :string;
}
export interface UpdateMessageTherapistCommandDto{
     directChatId:number;
     messageId:number;
     updatedContent:string;
     oldMessage:string;
     isRead:boolean;
}
export interface GetDirectChatByIdClientQuery{
    directChatId:number;
}

export interface MessageDto{
    messageId:number;
    senderId:number;
    senderType:string;
    content:string;
    sentAt:string;
    isRead:boolean;
}
export interface  GetDirectChatByIdClientQueryDto{
    directChatId:number;
    therapistId:number;
    therapistFullname:string;
    profileImage:string;
    messages:MessageDto[];
}
export interface GetDirectChatByIdTherapistQuery{
        directChatId:number;
}
export interface GetDirectChatByIdTherapistQueryDto{
    directChatId:number;
    clientId:number;
    clientFullname:string;
    profileImage:string;
    messages:MessageDto[];
}
export interface ListDirectChatMessagesQueryDto{
   directChatId:number;
    therapistId:number;
    profileImage:string;
    therapistFullname:string;
    isReadLAstMessage:boolean;
}
export interface ListDirectChatMessagesQuery{

}
export interface ListDirectChatMessagesTherapistQuery{
    
}
export interface ListDirectChatMessagesTherapistQueryDto{
   directChatId:number;
    clientId:number;
    profileImage:string;
    clientFullname:string;
    isReadLAstMessage:boolean;
}
