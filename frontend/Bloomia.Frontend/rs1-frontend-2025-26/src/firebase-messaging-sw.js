importScripts('https://www.gstatic.com/firebasejs/10.13.2/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/10.13.2/firebase-messaging-compat.js');

firebase.initializeApp({
  apiKey: "AIzaSyCNi3zscgZiNx4zvu5Y6cp_3dDtPhVTIgY",
    authDomain: "bloomia-notifications.firebaseapp.com",
    projectId: "bloomia-notifications",
    storageBucket: "bloomia-notifications.firebasestorage.app",
    messagingSenderId: "681467025172",
    appId: "1:681467025172:web:fe1cd56423784a18145401",
    measurementId: "G-WK1TEJ1ZJ8"
});
const messaging=firebase.messaging();

messaging.onBackgroundMessage(function(payload){
    const title=payload.notification?.title || 'Bloomia';
    const options={
        body: payload.notification?.body ||'Nova notifikacija',
        icon:'/favicon.ico'
    };
    self.registration.showNotification(title, options);
});