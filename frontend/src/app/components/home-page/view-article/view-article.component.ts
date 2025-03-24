import { Component, OnInit } from '@angular/core';
import { Article } from '../../../models/article.model';
import { NgFor, NgIf } from '@angular/common';
import { ArticleEndpointsService } from '../../../services/article-enpoints.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommentEndpointsService } from '../../../services/comment-endpoints.service';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../services/auhentication.service';

@Component({
  selector: 'app-view-article',
  imports: [NgFor, NgIf, ReactiveFormsModule,RouterLink],
  templateUrl: './view-article.component.html',
  styleUrl: './view-article.component.css'
})
export class ViewArticleComponent implements OnInit {

  formSubmitted:boolean = false;
  article:Article=new Article();
  commentFormGroup:FormGroup;
  isCommenting:boolean=false;
  currentArticleId:number=0;
  currentUserId:number=0;
  articleFound:boolean=true;
  

  constructor(private articleService:ArticleEndpointsService, private commentService:CommentEndpointsService, private authService:AuthenticationService, private activatedRoute:ActivatedRoute, private fb:FormBuilder, private router:Router) {
    this.commentFormGroup = this.fb.group({
      ID:[0],
      Content:['',[Validators.required]],
      ArticleID:[0],
      UserID:[0]
    });
  }
  
  async ngOnInit(){
    await this.loadArticle();
    await this.authService.user$.subscribe({
      next: res => {
        if(res?.userId)
          this.currentUserId = res?.userId;
      },
      error: err => {
        console.log(err);
      }
    })
  }

  /**
   * Gets the object of Form Control type of the Form Group.
   * @param controlName Name of the control from the Form Group.
   * @returns Form control of the Form Group.
   */
  getFormControl(controlName: string):FormControl{
    return this.commentFormGroup.get(controlName) as FormControl;
  }

  /**
   * Checks if the Form Control is valid.
   * @param controlName Name of the control from the Form Group.
   * @returns A boolean value if the form control is valid.
   */
  isControlInvalid(controlName: string):boolean{
    const ctrlToCheck = this.getFormControl(controlName);
    return (ctrlToCheck.touched || this.formSubmitted) && ctrlToCheck.invalid;
  }

  refreshForm(){
    this.commentFormGroup = this.fb.group({
      ID:[0],
      Content:['',[Validators.required]],
      ArticleID:[0],
      UserID:[0]
    });
  }

  async loadArticle(){
    const param_id = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    await this.articleService.getArticle(param_id).subscribe({
      next: data =>{
        this.article = data
        this.currentArticleId = param_id;
      },
      error: err => {
        if(err.status === 404){
          this.articleFound = false;
        }
        console.log(err);
      }
    });
  }

  createComment(){
    this.isCommenting=true;
  }

  async onSubmitCommentForm(){
    this.formSubmitted = true;
    if(this.commentFormGroup.valid){
      await this.commentService.createComment({...this.commentFormGroup.value, UserId:this.currentUserId, ArticleID: this.currentArticleId}).subscribe({
        next: res => {
          this.isCommenting=false;
          this.refreshForm();
          this.loadArticle();
        },
        error: err => {
          console.log("Error when saving comment");
          console.log(err);
          this.isCommenting=false;
        } 
      });
    }
  }

  cancelComment(){
    this.formSubmitted = false;
    this.isCommenting=false;
  }

}
