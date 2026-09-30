// src/app/core/guards/auth.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';
import { CurrentUserService } from '../services/auth/current-user.service';

export const myAuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const currentUser = inject(CurrentUserService);
  const router = inject(Router);

  const requireAuth = route.data['requireAuth'] === true;
  const requireAdmin = route.data['requireAdmin'] === true;
  const requireClient = route.data['requireClient'] === true;
  const requireTherapist = route.data['requireTherapist'] === true;

  const isAuth = currentUser.isAuthenticated();

  // 1) if the route requires auth and the user isn't logged in → login
  if (requireAuth && !isAuth) {
    router.navigate(['/auth/login']);
    return false;
  }

  // If auth isn't required → allow through (public routes)
  if (!requireAuth) {
    return true;
  }

  // 2) role check – admin > manager > employee
  const user = currentUser.snapshot;
  if (!user) {
    router.navigate(['/auth/login']);
    return false;
  }
  const role=user.role.toUpperCase();

  if (requireAdmin && role!=='ADMIN') {
    router.navigate([currentUser.getDefaultRoute()]);
    return false;
  }

  if (requireClient && role!=='CLIENT') {
    router.navigate([currentUser.getDefaultRoute()]);
    return false;
  }

  if (requireTherapist && role!=='THERAPIST') {
    router.navigate([currentUser.getDefaultRoute()]);
    return false;
  }

  return true;
};

export interface MyAuthRouteData {
  requireAuth?: boolean;
  requireAdmin?: boolean;
  requireClient?: boolean;
  requireTherapist?: boolean;
}

export function myAuthData(data: MyAuthRouteData): { auth: MyAuthRouteData } {
  return { auth: data };
}
