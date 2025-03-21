import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Article } from '../models/article.model';
import { environment } from '../../environments/environment.development';
import { ArticleFormInput } from '../models/model-inputs/article-form-input.model';

@Injectable({
  providedIn: 'root'
})
export class ArticleEndpointsService implements OnInit{

  url:string = environment.backend_URI + "/api/article/";
  constructor(private httpClient: HttpClient) { }
  
  ngOnInit(){
  }

  getArticles(): Observable<Article[]>{
    return this.httpClient.get<Article[]>(`${this.url}getall`);
  }

  getArticle(id:Number):Observable<Article>{
    return this.httpClient.get<Article>(`${this.url}getSingleArticle/${id}`);
  }

  creteArticle(article:ArticleFormInput):Observable<Object>{
    return this.httpClient.post(`${this.url}createArticle`,article);
  }
}
