export const environment = {
  production: false,
  apiUrl: 'https://localhost:7260',

  firebase:{
    apiKey: "AIzaSyCNi3zscgZiNx4zvu5Y6cp_3dDtPhVTIgY",
    authDomain: "bloomia-notifications.firebaseapp.com",
    projectId: "bloomia-notifications",
    storageBucket: "bloomia-notifications.firebasestorage.app",
    messagingSenderId: "681467025172",
    appId: "1:681467025172:web:fe1cd56423784a18145401",
    measurementId: "G-WK1TEJ1ZJ8"
  },
  //dodan public vapid key odnosno web push kljuc
  //vazan jer kada angular browser trazi FCM token mora dokazati da je aplikacija povezana sa firebase projektom i legitimna
  vapidKey:'BKMs_kQTxBeeaWNpjo-pWiJ8LidoaE7NNhAsIpNeR3bs6SeqTInVgJAaeHi12q5SDISLinX0Cs6S4g0xdB1R3UE'
};
