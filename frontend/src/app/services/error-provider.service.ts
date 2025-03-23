import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorProviderService {

  private errorSubject = new BehaviorSubject<string | null>(null);
  error$ = this.errorSubject.asObservable();
  constructor() { }

  putError(errorMessage:string){
    this.errorSubject.next(errorMessage);
  }

  clearError(){
    this.errorSubject.next(null);
  } 
}
