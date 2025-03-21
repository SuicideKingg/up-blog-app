import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/auhentication.service';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { CustomValidationsService } from '../../services/custom-validations.service';
import { RegisterFormInput } from '../../models/model-inputs/register-form-input.model';
import { catchError, of, switchMap } from 'rxjs';

@Component({
  selector: 'app-user-register',
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './user-register.component.html',
  styleUrl: './user-register.component.css'
})
export class UserRegisterComponent {
  
  registerFormGroup:FormGroup;
  formSubmitted:boolean = false;
  errorMessage:string = '';

  /**
   * Constructor.
   * @param authservice Authentication Service
   * @param fb FormBuilder
   * @param route Router
   * @param customValidation Custom Validation Service
   */
  constructor(private authservice:AuthenticationService, private fb:FormBuilder, private route:Router, private customValidation:CustomValidationsService) {
    this.registerFormGroup = this.fb.group({
      ID:[0],
      Name:['',[Validators.required]],
      Email:['',[Validators.required,Validators.email]],
      AvaterURL:[''],
      Role:[''],
      UserName:['',[Validators.required]],
      LoginPassword:['',[Validators.required, this.customValidation.patternValidator()]],
      LoginPasswordConfirm:['',[Validators.required]]
    },
    {
      validator: this.customValidation.MatchPassword('LoginPassword','LoginPasswordConfirm')
    });
  }

  /**
   * Gets the object of Form Control type of the Form Group.
   * @param controlName Name of the control from the Form Group.
   * @returns Form control of the Form Group.
   */
  getFormControl(controlName: string):FormControl{
    return this.registerFormGroup.get(controlName) as FormControl;
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
   * Event handler to go back to login page.
   */
  goBackToLoginPage(){
    this.route.navigate(['login']);
  }

  /**
   * Submit event form.
   */
  async onSubmit(){
    this.formSubmitted = true;
    if(this.registerFormGroup.valid){
      const formValue = this.registerFormGroup.value;
      // Remove the confirm password object
      delete formValue.LoginPasswordConfirm;
      
      await this.authservice.registerUser(this.registerFormGroup.value).pipe(
        switchMap(res => this.authservice.login({
          UserName: formValue.UserName,
          Password: formValue.LoginPassword
        }).pipe(
          catchError(err => {
            console.error("Error during login after registration.",err);
            this.errorMessage = "Something went wrong";
            return of(null);
          })
        ))).subscribe({
          next:res => {
            alert("Welcome to Up Blog");
            console.log(res);
            this.route.navigate(['']);
          }, 
          error:err => {
            if(err.status === 409){
              this.errorMessage = "Username already exists";
            }
            else{
              this.errorMessage = "Something went wrong";
              console.log(err);
            }
          }
      });
    }
    else{
      this.errorMessage = "Please fill the required fields";
    }
  }
}
