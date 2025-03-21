import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoadingService } from './services/loading.service';
import { Observable } from 'rxjs';
import { AsyncPipe, NgIf } from '@angular/common';

@Component({
  standalone:true,
  selector: 'app-root',
  imports: [RouterOutlet, NgIf, AsyncPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{

  loading$!:Observable<boolean>
  constructor(private loadingService:LoadingService) {
    
   }
  ngOnInit(): void {
    setTimeout(() => {
      this.loading$ = this.loadingService.loadingValue$;  
    });
  }

  title = 'Up Blog App';
}
