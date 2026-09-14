import { Component, inject, OnInit } from '@angular/core';
import { SavedTherapistsApiService } from '../../../api-services/savedTherapists/savedTherapists-api.service';
import { GetSavedTherapistByNameResponse, ListSavedTherapistInfoDto, ListSavedTherapistsQuery } from '../../../api-services/savedTherapists/savedTherapists-api.models';
import { BasePagedQuery } from '../../../core/models/paging/base-paged-query';
import { PageRequest } from '../../../core/models/paging/page-request';
import { ToasterService } from '../../../core/services/toaster.service';
import { FitConfirmDialogComponent } from '../../shared/components/fit-confirm-dialog/fit-confirm-dialog.component';
import { DialogType,DialogButton, DialogResult } from '../../shared/models/dialog-config.model';
import { MatDialog } from '@angular/material/dialog';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';
import { Router } from '@angular/router';
import { from } from 'rxjs';


@Component({
  selector: 'app-saved-therapists',
  standalone: false,
  templateUrl: './saved-therapists.component.html',
  styleUrl: './saved-therapists.component.scss',
})
export class SavedTherapistsComponent extends BaseListPagedComponent<ListSavedTherapistInfoDto,ListSavedTherapistsQuery> implements OnInit {
 

  private apiService=inject(SavedTherapistsApiService);
  private toasterService=inject(ToasterService);
  private dialog=inject(MatDialog);
  private router=inject(Router);

  savedTherapists:ListSavedTherapistInfoDto[]=[];
  page=1;
  pageSize=10;
  totalCount=0;

  isSearching=false;

  removeSavedTherapistMessage:string|null=null;
  removeSavedTherapistsMessage:string|null=null;

  therapist:ListSavedTherapistInfoDto|null=null;
  searchedSavedTherapists:GetSavedTherapistByNameResponse|null=null;

  minRatingError:string|null=null;

  constructor(){
    super();
    this.request=new ListSavedTherapistsQuery();
  }
  ngOnInit(): void {    

   this.initList();
  }
  loadPagedData(): void {
    this.startLoading();

     this.apiService.getAllSavedTherapists(this.request).subscribe({
      next:(response)=>{
        this.handlePageResult(response);
        this.savedTherapists=response.items;
        this.totalCount=response.totalItems;
       this.stopLoading();
      },
      error:(err)=>{
        this.errorMessage = "Failed to load saved therapists";
        this.toasterService.error("No saved therapists found.");
        this.stopLoading();
      }
    });
  }

  getStars(rating:number): number[]{
      return Array(Math.floor(rating)).fill(0);
  }
  getEmptyStars(rating:number):number[]{
    return Array(5-Math.floor(rating)).fill(0);
  }
  removeTherapistFromSavedTherapists(therapistId:number){

    this.savedTherapists.forEach(element => {
        if(element.therapistId==therapistId){
            this.therapist=element;
        }
    });

    this.apiService.removeSavedTherapist(therapistId).subscribe({
      next:(response)=>{
        this.removeSavedTherapistMessage=response;
        this.toasterService.success(`Therapist ${this.therapist?.fullname} removed from your saved therapists list!`);
           this.savedTherapists= this.savedTherapists.filter(t=> t.therapistId!==therapistId);
      },
      error:(err)=>{
        this.toasterService.error("Something went wrong. Try again!");
        console.error(err);
      }
    });
  }

  openConfirmDialogToRemoveAllSavedTherapists(){
    const dialogRef=this.dialog.open(FitConfirmDialogComponent,{
      width:'400px',
      data:{
        type:DialogType.WARNING,
        title:'Confirm removing all saved therapists',
        message:'Are you sure you want to remove all saved therapists from your list?',
        buttons:[{
          type:DialogButton.CANCEL,
           color: 'primary'
        },
        {
          type:DialogButton.DELETE,
           color: 'warn',
          label: 'Yes, remove all'
        }]
      }
    });

    dialogRef.afterClosed().subscribe((result: DialogResult | undefined)=>{
        if(result?.button === DialogButton.DELETE){
          this.removeAllSavedTherapists();
        }
    })
  }

  removeAllSavedTherapists(){

    this.apiService.removeAllSavedTherapists().subscribe({
      next:(response)=>{
          this.removeSavedTherapistsMessage=response;
          this.toasterService.success("You removed all saved therapists from your list!");
          this.savedTherapists=[];
          this.searchedSavedTherapists=[];
          this.isSearching=false;
      },
      error:(err)=>{
        this.toasterService.error("Something went wrong. Try again!");
        console.error(err);
      }
    });
  }

  applyExtraFilters():void{
    this.request.paging.page=1;
    this.loadPagedData();
  }

  onSpecializationFilterInput(event:Event):void{
    const value=(event.target as HTMLInputElement).value;
    this.request.specialization=value?.trim() || null;
    this.applyExtraFilters();
  }

  onMinRatingFilterInput(event:Event):void{
    const value=(event.target as HTMLInputElement).value;

    if(!value){
      this.minRatingError=null;
      this.request.minRating=null;
      this.applyExtraFilters();
      return;
    }

    const num=Number(value);
    if(isNaN(num) || num<1 || num>5){
      this.minRatingError='Ocjena mora biti izmedju 1 i 5';
      this.request.minRating=null;
      return;
    }

    this.minRatingError=null;
    this.request.minRating=num;
    this.applyExtraFilters();
  }

  onTherapyTypeFilterInput(event:Event):void{
    const value=(event.target as HTMLInputElement).value;
    this.request.therapyType=value?.trim() || null;
    this.applyExtraFilters();
  }

  toggleSortByRating():void{
    this.request.sortByRatingDesc=!this.request.sortByRatingDesc;
    this.applyExtraFilters();
  }

  searchByName(event:Event){
    const searchInput=(event.target as HTMLInputElement).value;
    if(searchInput.length===0){
      this.isSearching=false;
      this.searchedSavedTherapists=[];
      this.loadPagedData();
    }
    if(searchInput.length<2)
      return;
    this.isSearching=true;
    this.apiService.getSavedTherapistByName(searchInput).subscribe({
     next:(response)=>{
       this.searchedSavedTherapists=response;
      this.toasterService.info(`Results according to your input ${searchInput}`);
      },
      error:(err)=>{
       this.toasterService.info(`That therapist is not in your saved therapist list! input ${searchInput}`);
        console.error(err);
     }
   });
  }

  openTherapistProfile(therapistId: number): void {
    this.router.navigate([`client/therapist-details/${therapistId}`], {
      queryParams: { from: 'saved' }
    });
  }
}
