import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from '../../services/auhentication.service';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-user-login-register',
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './user-login-register.component.html',
  styleUrl: './user-login-register.component.css'
})
export class UserLoginRegisterComponent {

  loginForm:FormGroup;
  loginFailed:boolean = false;
  errorMessage:string = '';
  formValid:boolean = true;
  formSubmitted:boolean = false;

  /**
   * Constructor
   * @param fb Form Builder
   * @param authService Authentication Service
   * @param route Router
   */
  constructor(private fb:FormBuilder, private authService:AuthenticationService, private route:Router){
    this.loginForm = this.fb.group({
      username:['',[Validators.required]],
      password:['',[Validators.required]]
    });
  }

  /**
   * Event handler to go to the register page.
   */
  goToRegisterPage(){
    this.route.navigate(['register']);
  }

  /**
   * Gets the object of Form Control type of the Form Group.
   * @param controlName Name of the control from the Form Group.
   * @returns Form control of the Form Group.
   */
  getFormControl(controlName: string):FormControl{
    return this.loginForm.get(controlName) as FormControl;
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
   * Submit event form.
   */
  async onSubmit(){
    this.formSubmitted = true;
    if(this.loginForm.valid)
    {
      await this.authService.login(this.loginForm.value).subscribe({
        next:res => {
          console.log(res);
          this.route.navigate(['']);
        }, 
        error:err => {
          if(err.status === 401){
            this.loginFailed = true;
            this.errorMessage = "Login Failed! Username or password incorrect";
          }
          else{
            this.errorMessage = "Login Failed! Something went wrong. Please try again";
          }
          console.log(err);
        }
      });
    }
    else{
      this.errorMessage = "Please fill the required fields";
    }
  }
}
