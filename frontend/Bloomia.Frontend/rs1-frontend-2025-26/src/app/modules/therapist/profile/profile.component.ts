import { Component, inject, OnInit } from '@angular/core';
import { TherapistsApiService } from '../../../api-services/therapists/therapists-api.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { GetTherapistByIdQueryDto, TherapistAvailabilityDto } from '../../../api-services/therapists/therapists-api.models';
import { ListMyWorkingDatesAndTimesResponse, WorkingTimeSlotsDto } from '../../../api-services/therapistAvailability/therapistAvailability-api.models';
import { forkJoin, last, Observable } from 'rxjs';
import { TherapistAvailabilityApiService } from '../../../api-services/therapistAvailability/therapistAvailability-api.service';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { TherapyTypesApiService } from '../../../api-services/therapy-types/therapy-types-api.service';
import { TherapyTypeOptionDto } from '../../../api-services/therapy-types/therapy-types-api.models';
import { environment } from '../../../../environments/environment';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { DialogHelperService } from '../../shared/services/dialog-helper.service';
import { DialogButton } from '../../shared/models/dialog-config.model';

interface CalendarDayVm {
  date: Date;
  dateKey: string;
  dayNumber: number;
  inCurrentMonth: boolean;
  isToday: boolean;
  isSelected: boolean;
  hasSlots: boolean;
  hasFreeSlots: boolean;
  hasBookedSlots: boolean;
}

@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent extends BaseComponent implements OnInit {

  private therapistsApi = inject(TherapistsApiService);
  private therapistAvailabilityApi = inject(TherapistAvailabilityApiService);
  private currentUserService = inject(CurrentUserService);
  private therapyTypesApi = inject(TherapyTypesApiService);
  private sanitizier = inject(DomSanitizer);
  private dialogHelper = inject(DialogHelperService);

  therapist: GetTherapistByIdQueryDto | null = null;
  workingTimes: ListMyWorkingDatesAndTimesResponse | null = null;

  isEditMode = false;

  editSpecialization = '';
  editDescription = '';
  editFirstname = '';
  editLastname = '';
  editEmail = '';
  editPhoneNumber = '';

  allTherapyTypes: TherapyTypeOptionDto[] = [];
  selectedTherapyTypeIds: number[] = [];

  selectedProfileImageFile: File | null = null;
  selectedDocumentFile: File | null = null;
  profileImagePreviewUrl: string | null = null;

  currentMonth = new Date();
  selectedDateKey: string | null = null;

  selectedDocumentUrl: SafeResourceUrl | null = null;
  selectedDocumentName: string | null = null;
  private pendingDocumentObjectUrl: string | null = null;

  isDraggingFile = false;
  isDraggingProfileImage = false;
  
  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.startLoading();

    const currentUser = this.currentUserService.snapshot;
    const therapistId = currentUser?.therapistId;

    if (!therapistId) {
      this.stopLoading('TherapistId not found');
      return;
    }

    forkJoin({
      profile: this.therapistsApi.getById(therapistId),
      workingTimes: this.therapistAvailabilityApi.listMyWorkingDatesAndTimes(),
      therapyTypes: this.therapyTypesApi.list()
    }).subscribe({
      next: (result) => {
        this.therapist = result.profile;
        this.workingTimes = result.workingTimes;
        this.allTherapyTypes = result.therapyTypes;

        const firstDate = this.workingTimes.workingDates?.[0]?.date ?? null;
        if (firstDate) {
          this.selectedDateKey = firstDate;
          this.currentMonth = this.parseLocalDate(firstDate);
        }

        this.stopLoading();
      },
      error: (error) => {
        console.error('Error loading therapist profile/calendar:', error);
        this.stopLoading('Failed to load profile. Please try again later.');
      }
    });
  }

  private setSelectedProfileImage(file: File): void {
    if(!file.type.startsWith('image/')) {
      return;
    }

    this.selectedProfileImageFile = file;

    const reader = new FileReader();
    reader.onload = () => {
      this.profileImagePreviewUrl = reader.result as string;
    };

    reader.readAsDataURL(file);
  }

  onProfileImageDragOver(event: DragEvent): void {
    if(!this.isEditMode)
      return;

    event.preventDefault();
    event.stopPropagation();
    this.isDraggingProfileImage = true;
  }

  onProfileImageDragLeave(event: DragEvent): void {
    if(!this.isEditMode)
      return;

    event.preventDefault();
    event.stopPropagation();
    this.isDraggingProfileImage = false;
  }

  onProfileImageDrop(event: DragEvent): void {
    if(!this.isEditMode)
      return;

    event.preventDefault();
    event.stopPropagation();
    this.isDraggingProfileImage = false;

    const file = event.dataTransfer?.files?.[0];
    if(!file)
      return;

    this.setSelectedProfileImage(file);
  }

  onFileDragOver(event: DragEvent): void {
    if(!this.isEditMode)
      return;

    event.preventDefault();
    event.stopPropagation();
    this.isDraggingFile = true;
  }

  onFileDragLeave(event: DragEvent): void {
    if(!this.isEditMode)
      return;

    event.preventDefault();
    event.stopPropagation();
    this.isDraggingFile = false;
  }

  onFileDrop(event: DragEvent): void {
    if(!this.isEditMode)
      return;

    event.preventDefault();
    event.stopPropagation();
    this.isDraggingFile = false;

    const file = event.dataTransfer?.files?.[0];
    if(!file)
      return;

    if(file.type !== 'application/pdf' && !file.name.toLowerCase().endsWith('.pdf')) {
      return;
    }

    this.selectedDocumentFile = file;
  }

  get profileImageSrc(): string {
    if (this.profileImagePreviewUrl) {
      return this.profileImagePreviewUrl;
    }

    const profileImage = this.therapist?.profileImage;
    if (!profileImage) {
      return 'assets/images/user.png';
    }

    if (profileImage.startsWith('http://') || profileImage.startsWith('https://')) {
      return profileImage;
    }

    return `${environment.apiUrl}${profileImage}`;
  }

  get therapistDocuments() {
    return this.therapist?.documents || [];
  }

  get currentMonthLabel(): string {
    return this.currentMonth.toLocaleDateString('en-US', 
      { month: 'long', year: 'numeric' });
  }

  get weekDayLabels(): string[] {
    return ['Pon', 'Uto', 'Sri', 'Čet', 'Pet', 'Sub', 'Ned'];
  }

  get selectedDateLabel(): string {
  if (!this.selectedDateKey) 
    return 'Odaberite datum';

  const date = this.parseLocalDate(this.selectedDateKey);

  const weekday = date.toLocaleDateString('en-US', {
    weekday: 'long'
  });

  const formattedDate = date.toLocaleDateString('bs-BA', {
    day: '2-digit',
    month:'2-digit',
    year: 'numeric'
  });

  return `${weekday}, ${formattedDate}`;
}

get selectedWorkingDate() {
  if (!this.selectedDateKey || !this.workingTimes) return null;

  return this.workingTimes.workingDates.find(x => x.date === this.selectedDateKey) ?? null;
}

get selectedDateSlots(): WorkingTimeSlotsDto[] {
  return this.selectedWorkingDate?.allSlotsOfDate ?? [];
}

selectDay(day: CalendarDayVm): void {
  if (!day.hasSlots) return;

  this.selectedDateKey = day.dateKey;

  if (!day.inCurrentMonth) {
    this.currentMonth = new Date(day.date.getFullYear(), day.date.getMonth(), 1);
  }
}

  get calendarDays(): CalendarDayVm[] {
    const year = this.currentMonth.getFullYear();
    const month = this.currentMonth.getMonth();

    const firstDayOfMonth = new Date(year, month, 1);
    const startOffset = this.getMondayBasedDayIndex(firstDayOfMonth);
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    const days: CalendarDayVm[] = [];

    for (let i = startOffset; i > 0; i--) {
      const date = new Date(year, month, 1 - i);
      days.push(this.buildCalendarDay(date, false));
    }

    for (let day = 1; day <= daysInMonth; day++) {
      const date = new Date(year, month, day);
      days.push(this.buildCalendarDay(date, true));
    }

    while (days.length % 7 !== 0) {
      const nextDay = days.length - (startOffset + daysInMonth) + 1;
      const date = new Date(year, month + 1, nextDay);
      days.push(this.buildCalendarDay(date, false));
    }

    return days;
  }

  private buildCalendarDay(date: Date, inCurrentMonth: boolean): CalendarDayVm {
    const dateKey = this.toDateKey(date);
    const slots = this.getSlotsForDate(dateKey);

    return {
      date,
      dateKey,
      dayNumber: date.getDate(),
      inCurrentMonth,
      isToday: this.toDateKey(new Date()) === dateKey,
      isSelected: this.selectedDateKey === dateKey,
      hasSlots: slots.length > 0,
      hasFreeSlots: slots.some(slot => !slot.isBooked),
      hasBookedSlots: slots.some(slot => slot.isBooked),
    };
  }

  private getSlotsForDate(dateKey: string): WorkingTimeSlotsDto[] {
    const workingDate = this.workingTimes?.workingDates?.find(x => x.date === dateKey);
    return workingDate?.allSlotsOfDate ?? [];
  }

  private getMondayBasedDayIndex(date: Date): number {
    const jsDay = date.getDay();
    return jsDay === 0 ? 6 : jsDay - 1;
  }

  private toDateKey(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private parseLocalDate(dateString: string): Date {
    const [year, month, day] = dateString.split('-').map(Number);
    return new Date(year, month - 1, day);
  }

  get fullName(): string {
    if(!this.therapist) return '';

    return (
      this.therapist.fullname || 
      `${this.therapist.firstname || ''} ${this.therapist.lastname || ''}`.trim()
    );
  }

  get therapyTypes(): string[] {
    return this.therapist?.therapyTypes?.map(tt => tt.name || 'Not specified') || [];
  }

  get availability(): TherapistAvailabilityDto[]  {
    return this.therapist?.availability || [];
  }

  getStarsArray(): number[] {
    const rating = Math.round(this.therapist?.ratingAvg || 0);
    return Array(rating).fill(0);
  }

  formatDate(date: string): string {
    if(!date) return '';

    const parsedDate = new Date(date);
    return parsedDate.toLocaleDateString('bs-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  }

  formatTime(time: string): string {
    if(!time) return '';

    return time.length >= 5 ? time.substring(0, 5) : time;
  }

  get specializationText(): string {
    return this.therapist?.specialization || 'Not specified';
  }

  previousMonth(): void {
    this.currentMonth = new Date(
      this.currentMonth.getFullYear(),
      this.currentMonth.getMonth() - 1,
      1
    );
  }

  nextMonth(): void {
    this.currentMonth = new Date(
      this.currentMonth.getFullYear(),
      this.currentMonth.getMonth() + 1,
      1
    );
  }

  onSlotClick(slot: WorkingTimeSlotsDto): void {
    console.log('Clicked free slot:', slot);
  }

  onBookedSlotClick(slot: WorkingTimeSlotsDto): void {
    console.log('Clicked booked slot:', slot);
  }


  onEditProfile(): void {
    if(!this.therapist)
      return;

    this.isEditMode = true;
    this.editSpecialization = this.therapist.specialization || '';
    this.editDescription = this.therapist.description || '';
    this.selectedTherapyTypeIds = this.therapist.therapyTypes?.map(x => x.id) || [];
    this.editFirstname = this.therapist.firstname || '';
    this.editLastname = this.therapist.lastname || '';
    this.editEmail = this.therapist.email || '';
    this.editPhoneNumber = this.therapist.phoneNumber || '';

    this.selectedProfileImageFile = null;
    this.selectedDocumentFile = null;
    this.profileImagePreviewUrl = null;
  }

  cancelEdit(): void {
    this.isEditMode = false;
    this.editSpecialization = '';
    this.editDescription = '';
    this.editFirstname = '';
    this.editLastname = '';
    this.editEmail = '';
    this.editPhoneNumber = '';
    this.selectedTherapyTypeIds = [];
    this.selectedProfileImageFile = null;
    this.selectedDocumentFile = null;
    this.profileImagePreviewUrl = null;
    this.isDraggingProfileImage = false;

    if(this.pendingDocumentObjectUrl) {
      URL.revokeObjectURL(this.pendingDocumentObjectUrl);
      this.pendingDocumentObjectUrl = null;
    }
  }

  toggleTherapyType(therapyTypeId: number): void {
    if(this.selectedTherapyTypeIds.includes(therapyTypeId)) {
      this.selectedTherapyTypeIds = this.selectedTherapyTypeIds.filter(x => x !== therapyTypeId);
    } else {
      this.selectedTherapyTypeIds = [...this.selectedTherapyTypeIds, therapyTypeId];
    }
  }

  isTherapyTypeSelected(therapyTypeId: number): boolean {
    return this.selectedTherapyTypeIds.includes(therapyTypeId);
  }

  triggerProfileImageInput(fileInput: HTMLInputElement): void {
    if(!this.isEditMode)
      return;

    fileInput.click();
  }

  triggerDocumentInput(fileInput: HTMLInputElement): void {
    if(!this.isEditMode)
      return;
    fileInput.click();
  }

  onProfileImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if(!input.files || !input.files.length)
      return;

    this.setSelectedProfileImage(input.files[0]);
  }

  onUploadDocument(event: Event): void {
    const input = event.target as HTMLInputElement;

    if(!input.files || !input.files.length)
      return;
    
    this.selectedDocumentFile = input.files[0];
  }

  saveProfileChanges(): void {
    if(!this.therapist)
      return;

    this.startLoading();

    const payload = {
      firstname: this.editFirstname,
      lastname: this.editLastname,
      email: this.editEmail,
      phoneNumber: this.editPhoneNumber,
      specialization: this.editSpecialization,
      description: this.editDescription,
      therapyTypeIds: this.selectedTherapyTypeIds
    };

    this.therapistsApi.update(this.therapist.id, payload).subscribe({
      next: () => {
        this.uploadOptionalFilesAfterUpdate();
      },
      error: (error) => {
        console.error('Error updating therapist profile:', error);
        this.stopLoading('Failed to update profile. Please try again later.');
      }
    });
  }

  private uploadOptionalFilesAfterUpdate(): void {
    const uploadCalls: any[] = [];

    if(this.selectedProfileImageFile) {
      uploadCalls.push(this.therapistsApi.uploadProfileImage(this.selectedProfileImageFile));
    }

    if(this.selectedDocumentFile) {
      uploadCalls.push(this.therapistsApi.uploadTherapistDocument(this.selectedDocumentFile, 1));
    }

    if(uploadCalls.length === 0) {
      this.finishSaveFlow();
      return;
    }

    forkJoin(uploadCalls).subscribe({
      next: () => this.finishSaveFlow(),
      error: (error) => {
        console.error('Error uploading files:', error);
        this.stopLoading('Profile updated but failed to upload files. Please try uploading them separately.');
      }
    })
  }

  private finishSaveFlow(): void {
    this.isEditMode = false;

    if (this.pendingDocumentObjectUrl) {
      URL.revokeObjectURL(this.pendingDocumentObjectUrl);
      this.pendingDocumentObjectUrl = null;
    }

    this.loadProfile();
  }

  getAbsoluteFileUrl(relativePath: string): string {
    if(!relativePath) 
      return '';
    if(relativePath.startsWith('http://') || relativePath.startsWith('https://')) {
      return relativePath;
    }
    return `${environment.apiUrl}${relativePath}`;
  }

  openExistingDocument(document: { filePath: string; fileName: string }): void {
    const url = this.getAbsoluteFileUrl(document.filePath);
    this.selectedDocumentUrl = this.sanitizier.bypassSecurityTrustResourceUrl(url);
    this.selectedDocumentName = document.fileName;
  }

  openPendingDocument(): void {
    if(!this.selectedDocumentFile)
      return;

    if(this.pendingDocumentObjectUrl) {
      URL.revokeObjectURL(this.pendingDocumentObjectUrl);
    }

    this.pendingDocumentObjectUrl = URL.createObjectURL(this.selectedDocumentFile);
    this.selectedDocumentUrl = this.sanitizier.bypassSecurityTrustResourceUrl(this.pendingDocumentObjectUrl);
    this.selectedDocumentName = this.selectedDocumentFile.name;
  }

  removePendingDocument(): void {
    const pendingName = this.selectedDocumentFile?.name ?? 'selected file';

    this.dialogHelper.file.confirmDelete(pendingName).subscribe((result) => {
      if(!result || result.button !== DialogButton.DELETE) {
        return;
      }

      if (this.pendingDocumentObjectUrl) {
        URL.revokeObjectURL(this.pendingDocumentObjectUrl);
        this.pendingDocumentObjectUrl = null;
      }

      if (this.selectedDocumentName === pendingName) {
        this.selectedDocumentName = null;
        this.selectedDocumentUrl = null;
      }

      this.selectedDocumentFile = null;
    });
  }

  deleteExistingDocument(documentId: number, fileName: string): void {
    this.dialogHelper.file.confirmDelete(fileName).subscribe((result) => {
      if(!result || result.button !== DialogButton.DELETE) {
        return;
      }
      this.startLoading();

      this.therapistsApi.deleteTherapistDocument(documentId).subscribe({
        next: () => {
          if(this.selectedDocumentUrl) {
            this.closeDocumentViewer();
          }

          this.loadProfile();
        },
        error: (error) => {
          console.error('Error deleting document:', error);
          this.stopLoading('Failed to delete document. Please try again later.');
        }
      });

    });
  }

  closeDocumentViewer(): void {
    this.selectedDocumentUrl = null;
    this.selectedDocumentName = null;
  }

}

