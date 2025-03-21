import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommentFormInput } from '../models/model-inputs/comment-form-input.model';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CommentEndpointsService {

  apiUrl = `${environment.backend_URI}/api/comment/`;
  constructor(private http:HttpClient) { }

  createComment(commentFormInput:CommentFormInput){
    return this.http.post(`${this.apiUrl}createComment`, commentFormInput);
  }
}
