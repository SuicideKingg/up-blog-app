import { HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, filter, finalize, firstValueFrom, from, Observable, of, switchMap, take, throwError } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { AuthenticationService } from '../services/auhentication.service';
import { LoadingService } from '../services/loading.service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req:HttpRequest<unknown>, next:HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);
  const loadingService = inject(LoadingService);

  loadingService.show();
  if(req.url !== environment.backend_URI + "/api/authentication/clientlogin" && req.url !== environment.backend_URI + "/api/token/refresh"){
    
    const accessToken = authService.getAccessToken();

    // TODO: Check on Backend why it took longer to refresh than what it set(2 mins). Base on estimation it took 10 minutes instead.
    const authReq = accessToken == null ? req : req.clone({ setHeaders: { Authorization: `Bearer ${accessToken}` } })
    return next(authReq).pipe(
      catchError(error => {
        if (error.status === 401) {
          return handle401Error(authService, router, req, next);
        }
        // Force logout if its different error.
        isRefreshing = false;
        authService.logout();
        router.navigate(['/login']);
        return throwError(() => error);
      }),
      finalize(() => loadingService.hide())
    );
  }
  
  return next(req).pipe(
    finalize(() => loadingService.hide())
  );
};

const handle401Error = (authService:AuthenticationService, router:Router,req:HttpRequest<unknown>, next:HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  if(!isRefreshing){
    isRefreshing = true;
    refreshTokenSubject.next(null);
    return authService.refreshToken().pipe(
      switchMap(response => {
        isRefreshing = false;
        refreshTokenSubject.next(response.accessToken);
        return next(req.clone({ setHeaders: { Authorization: `Bearer ${response.accessToken}` } }));
      }),
      catchError(err => {
        isRefreshing = false;
        authService.logout();
        router.navigate(['/login']);
        return throwError(() => err);
      })
    );
  }
  else{
    return refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap(token => next(req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })))
    );
  }
}
