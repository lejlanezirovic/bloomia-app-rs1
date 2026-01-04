import { Component, inject, OnInit } from '@angular/core';
import { TherapistsApiService } from '../../../api-services/therapists/therapists-api.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ListTherapistsQueryDto, ListTherapistsRequest, ListTherapistsResponse } from '../../../api-services/therapists/therapists-api.models'; 


@Component({
  selector: 'app-list-therapists',
  standalone: false,
  templateUrl: './list-therapists.component.html',
  styleUrl: './list-therapists.component.scss',
})
export class ListTherapistsComponent implements OnInit {

  private router=inject(Router);
  private apiService=inject(TherapistsApiService);

  therapistsList:ListTherapistsQueryDto[]=[];
  total=0;

  isLoading=false;
  errorMessage:string|null=null;

  request=new ListTherapistsRequest();
  private searchTimeout:any;

  ngOnInit(): void {
    this.loadAllTherapists();
  }

  loadAllTherapists():void{
    this.isLoading=true;
    this.errorMessage=null;

    this.apiService.list(this.request).subscribe({
      next:(response)=>{
        this.therapistsList=response.items;
        this.total=response.totalItems;
        this.isLoading=false;
      },
      error:(err)=>{
        this.errorMessage='Failed to load therapists.';
        console.error(err);
        this.isLoading=false;
      }
    });
  }

  //rating - stars
  getStars(rating:number): number[]{
      return Array(Math.floor(rating)).fill(0);
  }
  getEmptyStars(rating:number):number[]{
    return Array(5-Math.floor(rating)).fill(0);
  }

  getFemaleGender(){
    this.request.genderId=2;
    this.loadAllTherapists();
  }
  getMaleGender(){
    this.request.genderId=1;
    this.loadAllTherapists();
  }

  //sort
  sortByrating(){
    this.request.sortByRatingDesc=!this.request.sortByRatingDesc;
    this.loadAllTherapists();
  }
  searchByName(payload:string|null){
    const name=payload?.trim();
    if(!name){
      this.request.firstname=null;
      this.request.lastname=null ;  
    }
    else{
      this.request.firstname=name;
      this.request.lastname=null;  
    }

    this.request.paging.page=1;
    this.loadAllTherapists();
  }
  onSearchInput(event:Event):void{
    const value=(event.target as HTMLInputElement).value;
    clearTimeout(this.searchTimeout);
    this.searchTimeout=setTimeout(()=>{
      this.searchByName(value);
    }, 400);//400ms
  }
}
