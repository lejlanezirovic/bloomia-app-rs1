import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-client-layout',
  standalone: false,
  templateUrl: './client-layout.component.html',
  styleUrl: './client-layout.component.scss',
})
export class ClientLayoutComponent {

  menuOpen = false;

  constructor(private router:Router){
   this.router.events.subscribe((event)=>{
      if(event instanceof NavigationEnd){
        console.log('Current route', event.url);
      }
   }) ;
  }

  toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }

  closeMenu(): void {
    this.menuOpen = false;
  }

  onLogout(): void {
    this.router.navigate(['/logout']);
  }
}
