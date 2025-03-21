import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from '../../../services/auhentication.service';
import { ArticleEndpointsService } from '../../../services/article-enpoints.service';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-create-article',
  imports: [ReactiveFormsModule,NgIf],
  templateUrl: './create-article.component.html',
  styleUrl: './create-article.component.css'
})
export class CreateArticleComponent {
  articleForm:FormGroup;
  userid:number=0;
  errorMessage:string='';
  formSubmitted: boolean=false;

  /**
   * Constructor
   * @param fb Form Builder
   * @param authService Authentication Service.
   * @param articleService Article Service.
   * @param router Router
   */
  constructor(private fb:FormBuilder, private authService:AuthenticationService, private articleService:ArticleEndpointsService, private router: Router) {
    // Get the current user logged in.
    this.authService.user$.subscribe({
      next: res => {
        console.log("Create Article reaced.")
        console.log(res);
        if(res?.userId){
          this.userid = res?.userId;
        }    
      },
      error: err => {
        console.log("Error when getting the user id.");
        console.log(err);
      }
    });
    
    // Initialize the form.
    this.articleForm = this.fb.group({
      id:[0],
      title:['',[Validators.required]],
      content:['',[Validators.required]],
      userId:[0]
    });
  }

  /**
   * Gets the object of Form Control type of the Form Group.
   * @param controlName Name of the control from the Form Group.
   * @returns Form control of the Form Group.
   */
  getFormControl(controlName: string):FormControl{
    return this.articleForm.get(controlName) as FormControl;
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
  
  /**
   * Event handler to go back to list.
   */
  goBackToList(){
    this.router.navigate(['']);
  }

  /**
   * Submit event form.
   */
  async onSubmit(){
    // Get the form value and modify the UserId only.
    this.formSubmitted = true;
    if(this.articleForm.valid){
      const formValue = {...this.articleForm.value, userId:this.userid};

      await this.articleService.creteArticle(formValue).subscribe({
        next: res => {
          this.router.navigate(['']);
        },
        error: (err) => {
          this.errorMessage = "Something went wrong!";
          console.log("Error when saving the article");
          console.log(err);
        },
      });
    }
    else{
      this.errorMessage = "Please fill in the required fields!";
    }
    
  }

}
