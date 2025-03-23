import { Component } from '@angular/core';
import { ErrorProviderService } from '../../../services/error-provider.service';

@Component({
  selector: 'app-error-page',
  imports: [],
  templateUrl: './error-page.component.html',
  styleUrl: './error-page.component.css'
})
export class ErrorPageComponent {
  errorMessage:string|null=null;
  constructor(private errorProvider:ErrorProviderService){
    this.errorProvider.error$.subscribe({
      next: err => {
        if(err)
          this.errorMessage = err;
        else
          this.errorMessage = '';
      }
    })
  }
}
