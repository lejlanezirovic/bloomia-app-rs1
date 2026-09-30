// src/app/core/services/auth/auth-facade.service.ts
import { Injectable, inject, signal, computed } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, tap, catchError, map } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

import { AuthApiService } from '../../../api-services/auth/auth-api.service';
import {
  LoginCommand,
  LoginCommandDto,
  LogoutCommand,
  RefreshTokenCommand,
  RefreshTokenCommandDto,
} from '../../../api-services/auth/auth-api.model';

import { AuthStorageService } from './auth-storage.service';
import { CurrentUserDto } from './current-user.dto';
import { JwtPayloadDto } from './jwt-payload.dto';

/**
 * Main auth service (façade).
 * - talks to AuthApiService (HTTP)
 * - talks to AuthStorageService (localStorage)
 * - decodes the JWT and holds CurrentUser as a signal
 *
 * Used in:
 * - the interceptor (getAccessToken, refresh)
 * - the guards (isAuthenticated, isAdmin)
 * - components (login, logout, navbar)
 */
@Injectable({ providedIn: 'root' })
export class AuthFacadeService {
  private api = inject(AuthApiService);
  private storage = inject(AuthStorageService);
  private router = inject(Router);

  // === REACTIVE STATE: current user ===

  private _currentUser = signal<CurrentUserDto | null>(null);

  /** readonly signal for the UI – read as auth.currentUser() */
  currentUser = this._currentUser.asReadonly();

  /** computed signals over the current user */
  isAuthenticated = computed(() => !!this._currentUser());
  isAdmin = computed(() => (this._currentUser()?.role ?? '').toUpperCase() === 'ADMIN');
  isClient = computed(() => (this._currentUser()?.role ?? '').toUpperCase() === 'CLIENT');
  isTherapist = computed(() => (this._currentUser()?.role ?? '').toUpperCase() === 'THERAPIST');

  constructor() {
    // try to initialize from an existing access token
    this.initializeFromToken();
  }

  // =========================================================
  // PUBLIC API
  // =========================================================

  /**
   * User login (email + password).
   * Saves the tokens to storage, decodes the JWT and fills in the current user state.
   */
  login(payload: LoginCommand): Observable<void> {
    return this.api.login(payload).pipe(
      tap((response: LoginCommandDto) => {
        this.storage.saveLogin(response);           // access + refresh + expiries
        this.decodeAndSetUser(response.accessToken); // popuni _currentUser
      }),
      map(() => void 0)
    );
  }

  /**
   * User logout:
   * - clears local state and tokens
   * - tries to invalidate the refresh token on the server (no fuss on error)
   */
  logout(): Observable<void> {
    const refreshToken = this.storage.getRefreshToken();

    // 1) clear locally (optimistic logout)
    this.clearUserState();

    // 2) no refresh token → no API call either
    if (!refreshToken) {
      return of(void 0);
    }

    const payload: LogoutCommand = { refreshToken };

    // 3) try server-side logout, ignore errors
    return this.api.logout(payload).pipe(catchError(() => of(void 0)));
  }

  /**
   * Refresh the access token – uses the refresh token.
   * Called by the interceptor when it gets a 401.
   */
  refresh(payload: RefreshTokenCommand): Observable<RefreshTokenCommandDto> {
    return this.api.refresh(payload).pipe(
      tap((response: RefreshTokenCommandDto) => {
        this.storage.saveRefresh(response);           // save the new tokens
        this.decodeAndSetUser(response.accessToken);  // update the current user
      })
    );
  }

  /**
   * Utility for guards/interceptors – clears auth state and redirects to /login.
   */
  redirectToLogin(): void {
    this.clearUserState();
    this.router.navigate(['/login']);
  }

  // =========================================================
  // GETTERS FOR THE INTERCEPTOR
  // =========================================================

  /**
   * Access token for the Authorization header.
   */
  getAccessToken(): string | null {
    return this.storage.getAccessToken();
  }

  /**
   * Refresh token for the refresh call.
   */
  getRefreshToken(): string | null {
    return this.storage.getRefreshToken();
  }

  // =========================================================
  // PRIVATE HELPERS
  // =========================================================

  /**
   * On app startup (constructor) – try to restore state from an existing token.
   */
  private initializeFromToken(): void {
    const token = this.storage.getAccessToken();
    if (token) {
      this.decodeAndSetUser(token);
    }
  }

  /**
   * Decode the JWT and set the current user state.
   */
  private decodeAndSetUser(token: string): void {
    try {
      const payload = jwtDecode<JwtPayloadDto>(token);

      const role = (
      payload.role ?? 
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? 
      ''
    ).toUpperCase();

      const user: CurrentUserDto = {
        userId: Number(payload.sub) || undefined,
        therapistId: payload.therapistId ? Number(payload.therapistId) : undefined,
        nameIdentifier: payload.nameIdentifier ?? undefined,
        fullname: payload.fullname ?? undefined,
        email: payload.email ?? payload.emailAdress ?? undefined,
        role: role || 'CLIENT',
        tokenVersion: payload.ver ? Number(payload.ver) : undefined,
      };

      this._currentUser.set(user);
    } catch (error) {
      console.error('Failed to decode JWT token:', error);
      this._currentUser.set(null);
    }
  }

  /**
   * Clear the user state + all tokens from storage.
   */
  private clearUserState(): void {
    this._currentUser.set(null);
    this.storage.clear();
  }
}
