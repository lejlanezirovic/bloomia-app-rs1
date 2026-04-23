import {inject, Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Observable, retry} from "rxjs";
import { buildHttpParams } from "../../core/models/build-http-params";
import { RegisterNotificationTokenCommand } from "./notifications-api.models";
@Injectable({
    providedIn: 'root'
})
export class NotificationTokenApiService{
    private baseUrl=`${environment.apiUrl}/api/NotificationToken`;
    private http=inject(HttpClient);
   // constructor(private httpClient:HttpClient){

 //   }

    registerNotificationToken(command:RegisterNotificationTokenCommand){

        return this.http.post(`${this.baseUrl}/register-notification-token`, command);
    }
    sendJournalReminder():Observable<string>{
        return this.http.post(`${this.baseUrl}/journal-reminder`, null, {responseType:'text'});
    }
}