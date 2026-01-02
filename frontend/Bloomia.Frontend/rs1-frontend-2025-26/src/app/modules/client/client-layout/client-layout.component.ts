import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-client-layout',
  standalone: false,
  templateUrl: './client-layout.component.html',
  styleUrl: './client-layout.component.scss',
})
export class ClientLayoutComponent {

  constructor(private router:Router){
   this.router.events.subscribe((event)=>{
      if(event instanceof NavigationEnd){
        console.log('Current route', event.url);
      }
   }) ;
  }
}
//samo sam napravila servis, dodala neku rutu u client-routing-module.ts pocela navbar ali nista jos konretno nema na stranici
