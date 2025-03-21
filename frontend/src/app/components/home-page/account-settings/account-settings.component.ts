import { NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../services/auhentication.service';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomValidationsService } from '../../../services/custom-validations.service';

@Component({
  selector: 'app-account-settings',
  imports: [NgIf,ReactiveFormsModule],
  templateUrl: './account-settings.component.html',
  styleUrl: './account-settings.component.css'
})
export class AccountSettingsComponent implements OnInit{

  errorMessage:string = '';
  updateAccountForm:FormGroup;
  userid:number = 0;
  formSubmitted:boolean = false;
  checkBoxValue:boolean = false;
  partialValue = {
      Id:0,
      Name:'',
      Email:''
  }

  constructor(private authenticationService:AuthenticationService, private fb:FormBuilder, private router:Router, private customValidation:CustomValidationsService) { 
    this.updateAccountForm = this.fb.group({
      Id:[0],
      Name:['', [Validators.required]],
      Email:['', [Validators.required, Validators.email]],
      OldPassword:[''],
      NewPassword:[''],
      ConfirmPassword:['']
    });

    // Set the password fields to disabled
    this.getFormControl('OldPassword').disable();
    this.getFormControl('NewPassword').disable();
    this.getFormControl('ConfirmPassword').disable();
  }

  async ngOnInit(){
    await this.authenticationService.getUser().subscribe({
      next: res => {
        this.updateAccountForm.setValue({
          Id:res.id,
          Name:res.name,
          Email:res.email,
          OldPassword:'',
          NewPassword:'',
          ConfirmPassword:''
        });

        this.partialValue = {
          Id:res.id,
          Name:res.name,
          Email:res.email
        }

      },
      error: err => {
        this.errorMessage = "Something went wrong when loading the user data. Please try again."
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
    return this.updateAccountForm.get(controlName) as FormControl;
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
   * Checkchanged event for Change Password checkbox.
   */
  onChangePasswordCheckBoxChanged(event: Event){
    // Get the current value and store it on the partial value 
    this.partialValue = {
      Id:this.getFormControl('Id').value,
      Name:this.getFormControl('Name').value,
      Email:this.getFormControl('Email').value
    }

    const isChecked = (event.target as HTMLInputElement).checked;
    this.checkBoxValue = isChecked;
    if(isChecked){
      this.getFormControl('OldPassword').setValidators([Validators.required]);
      this.getFormControl('NewPassword').setValidators([Validators.required, this.customValidation.patternValidator()]);
      this.getFormControl('ConfirmPassword').setValidators([Validators.required]);

      this.getFormControl('OldPassword').enable();
      this.getFormControl('NewPassword').enable();
      this.getFormControl('ConfirmPassword').enable();
    }
    else{

      this.getFormControl('OldPassword').clearValidators();
      this.getFormControl('NewPassword').clearValidators();
      this.getFormControl('ConfirmPassword').clearValidators();

      this.getFormControl('OldPassword').disable();
      this.getFormControl('NewPassword').disable();
      this.getFormControl('ConfirmPassword').disable();

      this.updateAccountForm.setValue({
        ...this.partialValue,
        OldPassword:'',
        NewPassword:'',
        ConfirmPassword:''
      });
    }
    
    this.getFormControl('OldPassword').updateValueAndValidity();
    this.getFormControl('NewPassword').updateValueAndValidity();
    this.getFormControl('ConfirmPassword').updateValueAndValidity();

    console.log(isChecked);
    console.log(this.updateAccountForm.value);
  }

  /**
   * Validator to check if the new and confirm password are match.
   * TODO: This should be done on custom validator. But due to complexity to recreate the form, we need to this as separate method.
   * Need to check possible ways to do this gracefully.
   * @returns If Confirm and New password are matched
   */
  passwordMatchInvalid():boolean{
    const ctrlToCheck = this.getFormControl('ConfirmPassword');
    return(ctrlToCheck.touched || this.formSubmitted) && !this.matchPassword();
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
    this.formSubmitted = true;
    if(this.updateAccountForm.valid){
      let canSave = true;

      if(this.checkBoxValue){
        if(!this.matchPassword()){
          canSave = false;
          this.errorMessage = "Please input correct data";
        }
      }

      if(canSave && confirm("Are you sure you want to update your account data?")){
        await this.authenticationService.updateUserAccount(this.getFormValue()).subscribe({
          next: res => {
            this.router.navigate(['']);
          },
          error: err => {
            if(err.error){
              this.errorMessage = err.error.message;
            }
            else{
              this.errorMessage = "Something went wrong when updating the data";    
            }
            console.log(err);
          }
        });
      }
    }
    else{
      this.errorMessage = "Please fill the required fields";
    }
  }

  private matchPassword():boolean{
    return this.getFormControl('NewPassword').value === this.getFormControl('ConfirmPassword').value;
  }

  private getFormValue(){
    if(this.checkBoxValue){
      return this.updateAccountForm.value;
    }
    else{
      return {...this.updateAccountForm.value,OldPassword:'',NewPassword:'',ConfirmPassword:''};
    }
  }

}
