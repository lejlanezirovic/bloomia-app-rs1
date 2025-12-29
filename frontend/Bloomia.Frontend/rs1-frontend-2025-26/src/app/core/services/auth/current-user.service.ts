// src/app/core/services/auth/current-user.service.ts
import { Injectable, inject, computed } from '@angular/core';
import { AuthFacadeService } from './auth-facade.service';

@Injectable({ providedIn: 'root' })
export class CurrentUserService {
  private auth = inject(AuthFacadeService);

  /** Signal koji UI može čitati (readonly) */
  currentUser = computed(() => this.auth.currentUser());

  isAuthenticated = computed(() => this.auth.isAuthenticated());
  isAdmin = computed(() => this.auth.isAdmin());
  isClient = computed(() => this.auth.isClient());
  isTherapist = computed(() => this.auth.isTherapist());

  get snapshot() {
    return this.auth.currentUser();
  }

  /** Pravilo: admin > ostali → client */
  getDefaultRoute(): string {
    const user = this.snapshot;

    console.log('getDefaultRoute: user =', user);
    console.log('getDefaultRoute: user.role =', user?.role);
    console.log('getDefaultRoute: role.toUpperCase() =', user?.role?.toUpperCase());

    if (!user) return '/login';


    if(user.role.toUpperCase()==='ADMIN') return '/admin';
    if(user.role.toUpperCase()==='THERAPIST') return '/therapist';
    return '/client';
  }
}
