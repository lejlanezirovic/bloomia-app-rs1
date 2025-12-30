import { Component, inject, OnInit } from '@angular/core';
import { TherapistsApiService } from '../../../api-services/therapists/therapists-api.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { GetTherapistByIdQueryDto } from '../../../api-services/therapists/therapists-api.models';

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {

  private therapistsApi = inject(TherapistsApiService);
  private currentUserService = inject(CurrentUserService);

  therapist: GetTherapistByIdQueryDto | null = null;
  isLoading = true;
  errorMessage: string | null = null;
  
  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.errorMessage = null;

    const currentUser = this.currentUserService.snapshot;
    const therapistId = currentUser?.therapistId;

    if(!therapistId) {
      this.errorMessage = 'TherapistId not found';
      this.isLoading = false;
      return;
    }

    this.therapistsApi.getById(therapistId).subscribe({
      next: (response) => {
        this.therapist = response;
        this.isLoading = false;
        console.log('Therapist profile loaded:', response);
        console.log('TherapistId:', response.id);
      },
      error: (error) => {
        console.error('Error loading therapist profile:', error);
        this.errorMessage = 'Failed to load profile. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  get fullName(): string {
    return this.therapist?.fullname || `${this.therapist?.firstname || ''} ${this.therapist?.lastname || ''}`.trim();
  }

  get specializations(): string {
    return this.therapist?.specialization || 'Not specified';
  }

  get therapyTypes(): string[] {
    return this.therapist?.therapyTypes.map(tt => tt.name || 'Not specified') || [];
  }

  onEditProfile(): void {
    console.log('Edit profile clicked');
  }

  //TODO
  onUploadPhoto(event: Event): void {
    const input = event.target as HTMLInputElement;
    if(input.files && input.files[0]) {
      const file = input.files[0];
      console.log('Uploading photo:', file);
      // TODO
    }
  }

  onUploadDocument(event: Event): void {
    //TODO
  }
}

