import { Component, inject, OnInit } from '@angular/core';
import { ClientsApiService } from '../../../api-services/clients/clients-api.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { GetClientProfileByIdQueryDTO } from '../../../api-services/clients/clients-api.models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-profile',
  standalone: false,
  templateUrl: './my-profile.component.html',
  styleUrl: './my-profile.component.scss',
})
export class MyProfileComponent implements OnInit {

  //1. treba mi api servis i current user servis da vidimo koji je klijent
  private apiService=inject(ClientsApiService);
  private currentUserService=inject(CurrentUserService);//sve dostupne metode iz ovog servisa
  private router=inject(Router);

  //2. deklarisati klijenta
  //error poruku
  //i isloading
  client:GetClientProfileByIdQueryDTO | null=null;
  isLoading=true;
  errorMessage:string|null=null;

  
 

  ngOnInit(): void {
    this.loadClientProfile();
  }

  loadClientProfile():void{
    this.isLoading=true;
    this.errorMessage=null;
    this.apiService.getMyProfile().subscribe({
      next: (responseClient)=>{
        this.client=responseClient;
        this.isLoading=false;
      },
      error:(err)=>{
        this.errorMessage='Something went wrong.';
        this.isLoading=false;
        console.error(err);
      }
    });
  }
  goToSelfTests():void{
    this.router.navigate(['/client/my-self-tests']);
  }
  goToJournals():void{
    this.router.navigate(['client/my-journals']);
  }
}
