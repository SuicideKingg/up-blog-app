import { NgIf } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthenticationService } from '../../services/auhentication.service';

@Component({
  standalone:true,
  selector: 'app-home-page',
  imports: [RouterOutlet, RouterLink, NgIf],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css'
})
export class HomePageComponent implements OnInit{
  user_full_name!:string;
  dropdownOpen = false;

  constructor(private router:Router, private authService:AuthenticationService){
    console.log("Constructor called");
  }

  ngOnInit(): void {
    console.log("ngOnit called")
    this.authService.user$.subscribe(value => {
      console.log(value);
      if(value){
        this.user_full_name = value.fullName;
      }
      else{
        this.user_full_name = '';
      }
    })
  }

  toggleDropdown(event: Event) {
    event.preventDefault(); // Prevent default anchor action
    this.dropdownOpen = !this.dropdownOpen;
  }

  @HostListener('document:click', ['$event'])
  closeDropdown(event: Event) {
    if (!(event.target as HTMLElement).closest('.dropdown')) {
      this.dropdownOpen = false;
    }
  }

  linkClick(){
    this.dropdownOpen = false;
  }

  logout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
