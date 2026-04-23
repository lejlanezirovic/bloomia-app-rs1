import { inject, Injectable } from "@angular/core";
import{initializeApp} from 'firebase/app';
import {getMessaging, getToken, isSupported, onMessage} from 'firebase/messaging';
import { environment } from "../../../environments/environment";
import { NotificationTokenApiService } from "../../api-services/notifications/notifications-api.service";

@Injectable({
    providedIn:'root'
})
export class NotificationsService{
    private firebaseApp=initializeApp(environment.firebase); //bez ovoga ne mozemo koristiti messaging
    //firebase mora znati kojem projektu pripada nasa Bloomia app
    private notificationTokenApiService=inject(NotificationTokenApiService);
    async requestPermissionAndGetToken(): Promise<string | null> {
    const supported = await isSupported(); //ovo je za provjeru da browser podrzava web messaging

    if (!supported) {
      console.warn('Firebase messaging is not supported in this browser.');
      return null;
    }
    const permission = await Notification.requestPermission();//browser prvo mora pitati za dozvolu korisnika za prikaz push notifikacija

    if (permission !== 'granted') {
      console.warn('Notification permission was not granted.');
      return null;
    }
    const registration = await navigator.serviceWorker.register('/firebase-messaging-sw.js');
    //registracija service workera, firebase-messaging-sw.js u pozadini prima poruke
    //bez njega browser ne moze primiti push poruku kad korisnik nije aktivan na Bloomia tabu

    const messaging = getMessaging(this.firebaseApp);//ovim pristupamo Firebase Cloud Messaging servisu
     const token = await getToken(messaging, {
      vapidKey: environment.vapidKey, //web push koristi vapid key da potvrdi da je aplikacija legitimna
      serviceWorkerRegistration: registration  //zelimo da token bude vezan za service worker
    });//ovim generisemo FCM token za konkretan browser, ovaj token nam je adresa na koju ce se kasnije slati journal reminder notfikacije

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
                console.log("Token spasen!");
            },
            error:(err)=>{
                console.error("Greska prilikom spasavanja tokena",err);
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
    //ovo nam sluzi za foreground poruke tj poruke dok smo u aplikaciji
    onMessage(messaging, (payload) => {
      console.log('Foreground message received:', payload);

      if (payload.notification) {
        alert(`${payload.notification.title}\n${payload.notification.body}`);
      }
    });
  }
}