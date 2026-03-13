import { Component, inject, OnInit } from '@angular/core';
import { SavedTherapistsApiService } from '../../../api-services/savedTherapists/savedTherapists-api.service';
import { GetSavedTherapistByNameResponse, ListSavedTherapistInfoDto, ListSavedTherapistsQuery } from '../../../api-services/savedTherapists/savedTherapists-api.models';
import { BasePagedQuery } from '../../../core/models/paging/base-paged-query';
import { PageRequest } from '../../../core/models/paging/page-request';
import { ToasterService } from '../../../core/services/toaster.service';
import { FitConfirmDialogComponent } from '../../shared/components/fit-confirm-dialog/fit-confirm-dialog.component';
import { DialogType,DialogButton, DialogResult } from '../../shared/models/dialog-config.model';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'app-saved-therapists',
  standalone: false,
  templateUrl: './saved-therapists.component.html',
  styleUrl: './saved-therapists.component.scss',
})
export class SavedTherapistsComponent implements OnInit {

  private apiService=inject(SavedTherapistsApiService);
  private toasterService=inject(ToasterService);
  private dialog=inject(MatDialog);


  savedTherapists:ListSavedTherapistInfoDto[]=[];
  page=1;
  pageSize=10;
  totalCount=0;

  isLoading = false;
  errorMessage: string | null = null;
  isSearching=false;

  removeSavedTherapistMessage:string|null=null;
  removeSavedTherapistsMessage:string|null=null;

  therapist:ListSavedTherapistInfoDto|null=null;
  searchedSavedTherapists:GetSavedTherapistByNameResponse|null=null;
  
  ngOnInit(): void {    
    this.loadSavedTherapists();
  }

//list terapeuta, paging
  loadSavedTherapists(){
    this.isLoading=true;
    this.errorMessage=null;

    const query:ListSavedTherapistsQuery={
        paging: {
          page: this.page,
          pageSize:this.pageSize
        }
    };

    this.apiService.getAllSavedTherapists(query).subscribe({
      next:(response)=>{
        this.savedTherapists=response.items;
        this.totalCount=response.totalItems;
        this.isLoading=false;
      },
      error:(err)=>{
        this.errorMessage = "Failed to load saved therapists";
        this.toasterService.error("No saved therapists found.");
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

    //mogu ga pronaci u saved therapists
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
    //kako promjeniti prikaz liste spremljenih terapeuta odmah nakon sto obrisemo nekog? 
    // samo ucitamo ponovo listu spremljenih terapeuta ali kazemo bez tog therapistId koji je obrisan. gore u next bloku
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

  searchByName(event:Event){
    //moram proslijediti event input
    //event ce biti tipa string
    //i taj string koji uzmemo iz inputa saljemo kao request
    //svaki put on input se poziva metoda
    const searchInput=(event.target as HTMLInputElement).value;
    if(searchInput.length===0){
      this.isSearching=false;
      this.searchedSavedTherapists=[];
      this.loadSavedTherapists();
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
}
