import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from '../models/login.model';
import { environment } from '../../environments/environment.development';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CurrentUser } from '../models/current-user.model';
import { RegisterFormInput } from '../models/model-inputs/register-form-input.model';
import { AccountUpdateFormInput } from '../models/model-inputs/account-update-form-input.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  url:string = environment.backend_URI + "/api/authentication/";
  url_token:string = environment.backend_URI + "/api/token/";

  private userSubject = new BehaviorSubject<CurrentUser | null>(null);
  user$ = this.userSubject.asObservable();
  
  constructor(private http:HttpClient, private jwtHelper:JwtHelperService) {
    // Load the values from the local storage
    console.log("Auth restarted");
    this.userSubject.next(this.getCurrentUserDataFromLocalStorage());
   }

  login(loginModel:Login){
    console.log("Auth.Login reached!");
    return this.http.post<CurrentUser>( `${this.url}clientlogin`, loginModel).pipe(
      tap((res) => {
        this.setTokensOnLocalStorage(res);
        this.userSubject.next(this.getCurrentUserDataFromLocalStorage());
      })
    );
  }

  refreshToken() {
    const userTokens = this.getTokensFromLocalStorage();
    return this.http.post<CurrentUser>(
      `${this.url_token}refresh`,
      {
        accessToken: userTokens?.accessToken,
        refreshToken: userTokens?.refreshToken,
      }
    ).pipe(
      tap((res) => {
        this.setTokensOnLocalStorage(res);
        this.userSubject.next(this.getCurrentUserDataFromLocalStorage());
      })
    );
  }

  getAccessToken() {
    const userTokens = this.getTokensFromLocalStorage();
    if(userTokens){
      return userTokens.accessToken;
    }
    else{
      return null;
    }
  }

  logout(){
    // TODO: Implement the Revoke tokens in the backend
    // const userTokens = this.getTokensFromLocalStorage();
    // this.http.post(`${this.url}revokeTokens`,{
    //   accessToken: userTokens?.accessToken,
    //   refreshToken: userTokens?.refreshToken,
    // }).pipe(
    //   tap((res) => {
    //     this.removeTokensFromLocalStorage();
    //   }));

    this.removeTokensFromLocalStorage();
  }

  getUser(){
    console.log("User ID:" + this.userSubject.value?.userId);
    return this.http.get<{id:number, name:string, email:string}>(`${this.url}get-user/${this.userSubject.value?.userId}`);
  }

  registerUser(registerFormInput:RegisterFormInput):Observable<CurrentUser>{
    return this.http.post<CurrentUser>(`${this.url}clientregister`,registerFormInput);
  }

  updateUserAccount(accountUpdateFormInput:AccountUpdateFormInput):Observable<any>{
    return this.http.post<any>(`${this.url}clientAccountUpdate`, accountUpdateFormInput).pipe(
      tap((res) => {
        this.setTokensOnLocalStorage(res);
        this.userSubject.next(this.getCurrentUserDataFromLocalStorage());
      })
    );
  }

  // Private methods.

  private getCurrentUserDataFromLocalStorage():CurrentUser | null{
    const storedCurrentUserToken = localStorage.getItem(environment.curret_user_tag);
    let tokenObject = null;
    
    if(storedCurrentUserToken){
      tokenObject = JSON.parse(storedCurrentUserToken) as CurrentUser;
    }

    return tokenObject;
  }

  private getTokensFromLocalStorage():{accessToken:string, refreshToken:string} | null{
    const storedCurrentUserToken = localStorage.getItem(environment.curret_user_tag);
    let tokenObject = null;
    if(storedCurrentUserToken){
      tokenObject = JSON.parse(storedCurrentUserToken) as CurrentUser;
    }

    if(tokenObject){
      return {accessToken:tokenObject.accessToken,refreshToken:tokenObject.refreshToken};
    }
    else{
      return null;
    }
  }

  private setTokensOnLocalStorage(user:CurrentUser) {
    localStorage.setItem(environment.curret_user_tag, JSON.stringify(user));
  }

  private removeTokensFromLocalStorage(){
    localStorage.removeItem(environment.curret_user_tag);
  }

}
