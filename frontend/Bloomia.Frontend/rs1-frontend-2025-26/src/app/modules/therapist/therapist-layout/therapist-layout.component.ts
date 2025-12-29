import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-therapist-layout',
  standalone: false,
  templateUrl: './therapist-layout.component.html',
  styleUrl: './therapist-layout.component.scss',
})
export class TherapistLayoutComponent {
  constructor(private router: Router) { 
    this.router.events.subscribe((event) => {
      if(event instanceof NavigationEnd) {
        console.log('Current route:', event.url);
      }
    });
  }

}
