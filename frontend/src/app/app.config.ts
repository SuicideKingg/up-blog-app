import { ApplicationConfig, Provider, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors } from '@angular/common/http';
import { JWT_OPTIONS, JwtHelperService, JwtInterceptor } from '@auth0/angular-jwt';
import { environment } from '../environments/environment.development';
import { authInterceptor } from './interceptors/auth.interceptor';
import { provideStore } from '@ngrx/store';
import { authReducer } from './actions/auth.reducer';

export function jwtOptionsFactory() {
  return {
    tokenGetter: () => localStorage.getItem(environment.token_constant.access_token_label),
    allowedDomains: [environment.backend_URI],
    disallowedRoutes: []
  };
}

const jwtProviders: Provider[] = [
  { provide: JWT_OPTIONS, useFactory: jwtOptionsFactory },
  JwtHelperService, // Provide JwtHelperService manually
  { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
];

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideHttpClient(withInterceptors([authInterceptor])), ...jwtProviders, provideStore({auth:authReducer})]
};