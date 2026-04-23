import { Component, inject, OnInit } from '@angular/core';
import { TherapistsApiService } from '../../../api-services/therapists/therapists-api.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ListTherapistsQueryDto, ListTherapistsRequest, ListTherapistsResponse } from '../../../api-services/therapists/therapists-api.models'; 
import { SavedTherapistsApiService } from '../../../api-services/savedTherapists/savedTherapists-api.service';
import { AddTherapistToSavedTherapistsCommandDto } from '../../../api-services/savedTherapists/savedTherapists-api.models';
import { ToasterService } from '../../../core/services/toaster.service';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';

@Component({
  selector: 'app-list-therapists',
  standalone: false,
  templateUrl: './list-therapists.component.html',
  styleUrl: './list-therapists.component.scss',
})
export class ListTherapistsComponent extends BaseListPagedComponent<ListTherapistsQueryDto,ListTherapistsRequest> implements OnInit {

//ima paging mozemo implementirati

  private router=inject(Router);
  private apiService=inject(TherapistsApiService);
  private saveTherapistService=inject(SavedTherapistsApiService);
  private toastService=inject(ToasterService);

  therapistsList:ListTherapistsQueryDto[]=[];
  private searchTimeout:any;
  savedTherapistDto:AddTherapistToSavedTherapistsCommandDto|null=null;

  constructor(){
    super();
    this.request=new ListTherapistsRequest();
  }
  ngOnInit(): void {
   this.initList();
  }

  loadAllTherapists():void{
   
    this.apiService.list(this.request).subscribe({
      next:(response)=>{
        this.therapistsList=response.items;
        this.isLoading=false;
      },
      error:(err)=>{
        this.errorMessage='Failed to load therapists.';
        console.error(err);
        this.isLoading=false;
      }
    });
  }

  loadPagedData(): void {
    this.startLoading();

     this.apiService.list(this.request).subscribe({
      next:(response)=>{
        this.handlePageResult(response);
        console.info(response);
        this.therapistsList=response.items;
        this.stopLoading();
      },
      error:(err)=>{
        this.errorMessage='Failed to load therapists.';
        console.error(err);
       this.stopLoading();
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

    this.request.paging.page=1;
    this.loadPagedData();
  }
  getMaleGender(){
    this.request.genderId=1;
    this.request.paging.page=1;
    this.loadPagedData();
  }

  //sort
  sortByrating(){
    this.request.sortByRatingDesc=!this.request.sortByRatingDesc;
    this.loadPagedData();
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
     this.loadPagedData();
  }
  onSearchInput(event:Event):void{
    const value=(event.target as HTMLInputElement).value;
    clearTimeout(this.searchTimeout);
    this.searchTimeout=setTimeout(()=>{
      this.searchByName(value);
    }, 400);//400ms
  }
  addTherapistToSavedTherapists(t_Id:number){
    this.saveTherapistService.addTherapistToSavedTherapists({therapistId:t_Id}).subscribe({
      next:(response)=>{
        this.savedTherapistDto=response;
        this.toastService.success("Therapist saved!")
      },
      error:(err)=>{
        this.toastService.error("Therapist already saved ");
        console.error(err);
      }
    })
  }

  //messageClick(therapistId:number){
    //this.router.navigate(['client/direct-chats'], {
      //queryParams:{therapistId}
    //});
  //}
  messageIconClick(therapistId:number){
      this.router.navigate([`client/direct-chats/${therapistId}/details`]);
  }
}
