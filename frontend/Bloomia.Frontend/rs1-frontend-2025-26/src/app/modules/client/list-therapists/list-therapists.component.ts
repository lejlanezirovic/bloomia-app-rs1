import { Component, inject, OnInit, ViewChild, ElementRef, HostListener} from '@angular/core';
import { TherapistsApiService } from '../../../api-services/therapists/therapists-api.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ListTherapistsQueryDto, ListTherapistsRequest, ListTherapistsResponse } from '../../../api-services/therapists/therapists-api.models'; 
import { SavedTherapistsApiService } from '../../../api-services/savedTherapists/savedTherapists-api.service';
import { AddTherapistToSavedTherapistsCommandDto } from '../../../api-services/savedTherapists/savedTherapists-api.models';
import { ToasterService } from '../../../core/services/toaster.service';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';


@Component({
  selector: 'app-list-therapists',
  standalone: false,
  templateUrl: './list-therapists.component.html',
  styleUrl: './list-therapists.component.scss',
})
export class ListTherapistsComponent extends
  BaseListPagedComponent<ListTherapistsQueryDto,ListTherapistsRequest> implements OnInit {

  private router=inject(Router);
  private apiService=inject(TherapistsApiService);
  private saveTherapistService=inject(SavedTherapistsApiService);
  private toastService=inject(ToasterService);

  therapistsList:ListTherapistsQueryDto[]=[];
  private searchTimeout:any;
  savedTherapistDto:AddTherapistToSavedTherapistsCommandDto|null=null;

  @ViewChild('searchInput') searchInputRef?: ElementRef<HTMLInputElement>;
  nameSuggestions: string[] = [];
  filteredNames: string[] = [];

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
        this.updateNameSuggestions(response.items);

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
        this.updateNameSuggestions(response.items);
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

    const query = value?.trim().toLowerCase() ?? '';
    this.filteredNames = query ? this.nameSuggestions.filter(n => n.toLowerCase().includes(query)): [];

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

  openTherapistProfile(therapistId: number): void {
    this.router.navigate([`client/therapist-details/${therapistId}`], {
      queryParams: { from: 'list' }
    });
  }

  //messageClick(therapistId:number){
    //this.router.navigate(['client/direct-chats'], {
      //queryParams:{therapistId}
    //});
  //}

  private updateNameSuggestions(items: ListTherapistsQueryDto[]): void {
    const newNames = items.map(t => t.fullname).filter((n): n is string => !!n);
    const merged = new Set([...this.nameSuggestions, ...newNames]);
    this.nameSuggestions = Array.from(merged).sort();
  }
  onNameSelected(event: MatAutocompleteSelectedEvent): void {
    const selected = event.option.value as string;
    if (this.searchInputRef) {
      this.searchInputRef.nativeElement.value = selected;
    }

    const parts = selected.trim().split(/\s+/);
    this.request.firstname = parts[0] || null;
    this.request.lastname = parts.length > 1 ? parts.slice(1).join(' ') : null;

    this.filteredNames = [];
    this.request.paging.page = 1;
    this.loadPagedData();
  }
  messageIconClick(therapistId:number){
      this.router.navigate([`client/direct-chats/${therapistId}/details`]);
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardShortcuts(event: KeyboardEvent): void {
    const target = event.target as HTMLElement;
    const isTyping = target.tagName === 'INPUT' || target.tagName === 'TEXTAREA';

    if ((event.ctrlKey && event.key.toLowerCase() === 'k') || (!isTyping && event.key === '/')) {
      event.preventDefault();
      this.searchInputRef?.nativeElement.focus();
      return;
    }

    if (event.key === 'Escape') {
      if (this.searchInputRef) {
        this.searchInputRef.nativeElement.value = '';
      }
      this.filteredNames = [];
      this.searchByName(null);
      this.searchInputRef?.nativeElement.blur();
      return;
    }

    if (isTyping) return;

    switch (event.key.toLowerCase()) {
      case 'm':
        this.getMaleGender();
        break;
      case 'f':
        this.getFemaleGender();
        break;
      case 'r':
        this.sortByrating();
        break;
    }
  }
}
