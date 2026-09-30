import { inject, Injectable } from "@angular/core";
import{initializeApp} from 'firebase/app';
import {getMessaging, getToken, isSupported, onMessage} from 'firebase/messaging';
import { environment } from "../../../environments/environment";
import { NotificationTokenApiService } from "../../api-services/notifications/notifications-api.service";

@Injectable({
    providedIn:'root'
})
export class NotificationsService{
    private firebaseApp=initializeApp(environment.firebase); //without this we can't use messaging
    //firebase needs to know which project our Bloomia app belongs to
    private notificationTokenApiService=inject(NotificationTokenApiService);

    async requestPermissionAndGetToken(): Promise<string | null> {
      const supported = await isSupported(); //this checks whether the browser supports web messaging

      if (!supported) {
        console.warn('Firebase messaging is not supported in this browser.');
        return null;
      }
      const permission = await Notification.requestPermission();//the browser must first ask the user for permission to show push notifications

      if (permission !== 'granted') {
        console.warn('Notification permission was not granted.');
        return null;
      }
      const registration = await navigator.serviceWorker.register('/firebase-messaging-sw.js');
      //service worker registration, firebase-messaging-sw.js receives messages in the background
      //without it the browser can't receive a push message when the user isn't active on the Bloomia tab

      const messaging = getMessaging(this.firebaseApp);//this is how we access the Firebase Cloud Messaging service
      const token = await getToken(messaging, {
        vapidKey: environment.vapidKey, //web push uses the vapid key to confirm the app is legitimate
        serviceWorkerRegistration: registration  //we want the token to be tied to the service worker
      });//this generates the FCM token for this specific browser; this token is the address that journal reminder notifications will later be sent to

    // if (!token) {
      // console.warn('No FCM token available.');
    //   return null;
    //  }
      if(token){
          console.log('FCM token:', token);
          this.notificationTokenApiService.registerNotificationToken({
              token:token
          }).subscribe({
              next:()=>{
                  console.log("Token saved!");
              },
              error:(err)=>{
                  console.error("Error while saving the token",err);
              }
              
          });
          return token;
      }
    return null;
  }

 async listenForForegroundMessages(): Promise<void> {
    const supported = await isSupported();

    if (!supported) {
      return;
    }

    const messaging = getMessaging(this.firebaseApp);
    //this is used for foreground messages, i.e. messages received while we're in the app
    onMessage(messaging, (payload) => {
      console.log('Foreground message received:', payload);

      if (payload.notification) {
        alert(`${payload.notification.title}\n${payload.notification.body}`);
      }
    });
  }
}