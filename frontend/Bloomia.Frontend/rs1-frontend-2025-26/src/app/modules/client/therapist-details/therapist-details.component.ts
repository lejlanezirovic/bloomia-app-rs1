import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { TherapistsApiService } from '../../../api-services/therapists/therapists-api.service';
import { ReviewsApiService } from '../../../api-services/reviews/reviews-api.service';
import { AppointmentsApiService } from '../../../api-services/appointments/appointments-api.service';
import { GetTherapistByIdQueryDto } from '../../../api-services/therapists/therapists-api.models';
import {
  CreateReviewCommand,
  GetReviewsByTherapistIdQueryDto
} from '../../../api-services/reviews/reviews-api.models';
import { SessionType } from '../../../api-services/appointments/appointments-api.models';
import { ToasterService } from '../../../core/services/toaster.service';
import { TherapistAvailabilityApiService } from '../../../api-services/therapistAvailability/therapistAvailability-api.service';
import { ListMyWorkingDatesAndTimesResponse, WorkingTimeSlotsDto } from '../../../api-services/therapistAvailability/therapistAvailability-api.models';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { environment } from '../../../../environments/environment';

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
  selector: 'app-therapist-details',
  standalone: false,
  templateUrl: './therapist-details.component.html',
  styleUrl: './therapist-details.component.scss',
})
export class TherapistDetailsComponent extends BaseComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private therapistsApiService = inject(TherapistsApiService);
  private reviewsApiService = inject(ReviewsApiService);
  private appointmentsApiService = inject(AppointmentsApiService);
  private toasterService = inject(ToasterService);
  private fb = inject(FormBuilder);
  private therapistAvailabilityApiService = inject(TherapistAvailabilityApiService);
  private sanitizer = inject(DomSanitizer);

  therapist: GetTherapistByIdQueryDto | null = null;
  reviews: GetReviewsByTherapistIdQueryDto[] = [];
  canReview = false;

  workingTimes: ListMyWorkingDatesAndTimesResponse | null = null;
  currentMonth = new Date();
  selectedDateKey: string | null = null;
  selectedSlot: WorkingTimeSlotsDto | null = null;


  therapistId = 0;
  isSubmitting = false;
  reviewsTotalCount = 0;
  returnTo: 'list' | 'saved' = 'list';

  selectedDocumentUrl: SafeResourceUrl | null = null;
  selectedDocumentName: string | null = null;


  isBooking = false;
  readonly SessionType = SessionType;

  reviewForm = this.fb.group({
    rating: [0, [Validators.required, Validators.min(1), Validators.max(5)]],
    comment: ['']
  });

  bookingForm = this.fb.group({
    therapistAvailabilityId: [null as number | null, Validators.required],
    sessionType: [null as SessionType | null, Validators.required]
  });

  ngOnInit(): void {

    const id = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    const from = this.activatedRoute.snapshot.queryParamMap.get('from');

    if(from === 'saved') {
      this.returnTo = 'saved';
    } else {
      this.returnTo = 'list';
    }

    if(!id) {
      this.stopLoading('Invalid therapist id.');
      return;
    }

    this.therapistId = id;
    this.loadData();
  }

  loadData(): void {
    this.startLoading();

    forkJoin({
      therapist: this.therapistsApiService.getById(this.therapistId),
      reviewsResponse: this.reviewsApiService.getByTherapistId(this.therapistId, { page: 1, pageSize: 5 }),
      canReview: this.reviewsApiService.canReview(this.therapistId),
      workingTimes: this.therapistAvailabilityApiService.getWorkingDatesAndTimesForClient(this.therapistId)
    }).subscribe({
      next: ({ therapist, canReview, reviewsResponse, workingTimes }) => {
        this.therapist = therapist;
        this.reviews = reviewsResponse.items;
        this.reviewsTotalCount = reviewsResponse.totalItems;
        this.workingTimes = workingTimes;
        this.canReview = canReview;
        

        const stillExists = this.selectedDateKey ? this.workingTimes.workingDates?.some(x => x.date === this.selectedDateKey)
        : false;

        if(stillExists && this.selectedDateKey) {
          this.currentMonth = this.parseLocalDate(this.selectedDateKey);
        } else {
          const firstDate = this.workingTimes.workingDates?.[0]?.date ?? null;
          if(firstDate) {
            this.selectedDateKey = firstDate;
            this.currentMonth = this.parseLocalDate(firstDate);
          } else {
            this.selectedDateKey = null;
          }
        }

        this.stopLoading();
      },
      error: (err) => {
        console.error(err);
        this.stopLoading('An error occurred while loading therapist details. Please try again later.');
      }
    });
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
    return this.therapist?.therapyTypes.map(x => x.name || '').filter(Boolean) || [];
  }

  getStarsArray(rating: number): number[] {
    return Array(Math.floor(rating)).fill(0);
  }

  getEmptyStarsArray(rating: number): number[] {
    return Array(5 - Math.floor(rating)).fill(0);
  }

  selectRating(value: number): void {
    this.reviewForm.patchValue({ rating: value });
  }


  submitReview(): void {
    if(this.reviewForm.invalid) {
      this.reviewForm.markAllAsTouched();
      this.toasterService.error('Please choose a rating.');
      return;
    }

    const payload: CreateReviewCommand = {
      therapistId: this.therapistId,
      rating: this.reviewForm.value.rating!,
      comment: this.reviewForm.value.comment || null
    };

    this.isSubmitting = true;

    this.reviewsApiService.create(payload).subscribe({
      next: () => {
        this.toasterService.success('Review submitted successfully.');
        this.reviewForm.reset({
          rating: 0,
          comment: ''
        });
        this.loadData();
        this.isSubmitting = false;
      },
      error: (err) => {
        console.error(err);
        this.toasterService.error(err?.error?.message || 'Failed to submit review.');
        this.isSubmitting = false;
      }
    });
  }

  loadAllReviews(): void {
    this.reviewsApiService.getByTherapistId(this.therapistId, { page: 1, pageSize: 100 }).subscribe({
      next: (response) => {
        this.reviews = response.items;
        this.reviewsTotalCount = response.totalItems;
      },
      error: (err) => {
        console.error(err);
        this.toasterService.error('Failed to load all reviews. Please try again later.');
      }
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('bs-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });
  }


  goBack(): void {
    if(this.returnTo === 'saved') {
      this.router.navigate(['/client/saved-therapists']);
    } else {
      this.router.navigate(['/client/list-therapists']);
    }
  }

  get currentMonthLabel(): string {
    return this.currentMonth.toLocaleDateString('en-US',
       { month: 'long',
         year: 'numeric' });
  }

  get weekDayLabels(): string[] {
    return ['Pon', 'Uto', 'Sri', 'Čet', 'Pet', 'Sub', 'Ned'];
  }

  get selectedDateLabel(): string {
    if(!this.selectedDateKey) 
      return 'Odaberite datum';

    const date = this.parseLocalDate(this.selectedDateKey);

    const weekday = date.toLocaleDateString('en-US', 
      { weekday: 'long'}
    );

    const formattedDate = date.toLocaleDateString('bs-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric'
    });

    return `${weekday}, ${formattedDate}`;
  }

  get selectedWorkingDate() {
    if(!this.selectedDateKey || !this.workingTimes)
      return null;

    return this.workingTimes.workingDates.find(x => x.date === this.selectedDateKey) ?? null;
  }

  get selectedDateSlots(): WorkingTimeSlotsDto[] {
    return this.selectedWorkingDate?.allSlotsOfDate ?? [];
  }

  selectDay(day: CalendarDayVm): void {
    if(!day.hasSlots)
      return;

    this.selectedDateKey = day.dateKey;
    this.selectedSlot = null;

    if(!day.inCurrentMonth) {
      this.currentMonth = new Date(day.date.getFullYear(), day.date.getMonth(), 1);
    }
  }

  get calendarDays(): CalendarDayVm[] {
    const year = this.currentMonth.getFullYear();
    const month = this.currentMonth.getMonth();

    const firstDayOfMonth = new Date(year, month, 1);
    const startOffset = this.getMondayBasedDayIndex(firstDayOfMonth);
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    const days : CalendarDayVm[] = [];

    for(let i = startOffset; i > 0; i--) {
      const date = new Date(year, month, 1 - i);
      days.push(this.buildCalendarDay(date, false));
    }

    for(let day = 1; day <= daysInMonth; day++) {
      const date = new Date(year, month, day);
      days.push(this.buildCalendarDay(date, true));
    }

    while(days.length % 7 !== 0) {
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
      hasFreeSlots: slots.some(x => !x.isBooked),
      hasBookedSlots: slots.some(x => x.isBooked)
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

  formatTime(time: string): string {
    if(!time) return '';

    return time.length >= 5 ? time.substring(0, 5) : time;
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
    if(slot.isBooked) 
      return;

    this.selectedSlot = slot;

    this.bookingForm.patchValue({
      therapistAvailabilityId: slot.therapistAvailabilityId,
      sessionType: null
    });

  }

  selectSessionType(type: SessionType): void {
    this.bookingForm.patchValue({
      sessionType: type
    });
  }


  onBookedSlotClick(slot: WorkingTimeSlotsDto): void {
    console.log('Booked slot clicked:', slot);
  }

  bookAppointment(): void {
    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      this.toasterService.error('Please select a session type.');
      return;
    }

    const formValue = this.bookingForm.getRawValue();

    this.isBooking = true;

    this.appointmentsApiService.createAppointment(formValue.therapistAvailabilityId!, formValue.sessionType!)
    .subscribe({
      next: (response) => {
        this.toasterService.success(response.note || 'Appointment booked successfully.');

        this.clearBookingSelection();

        this.loadData();
        this.isBooking = false;
      },
      error: (err) => {
        console.error(err);
        this.toasterService.error('Failed to book appointment.');
        this.isBooking = false;
      }
    });
  }

  clearBookingSelection(): void {
    this.selectedSlot = null;
    this.bookingForm.reset();
  }

  get therapistDocuments() {
    return this.therapist?.documents || [];
  }

  get profileImageSrc(): string {
    const profileImage = this.therapist?.profileImage;

    if (!profileImage) {
      return 'assets/images/user.png';
    }

    if (profileImage.startsWith('http://') || profileImage.startsWith('https://')) {
      return profileImage;
    }

    return `${environment.apiUrl}${profileImage}`;
  }

  getAbsoluteFileUrl(relativePath: string): string {
    if(!relativePath) 
      return '';
    
    if (relativePath.startsWith('http://') || relativePath.startsWith('https://')) {
      return relativePath;
    }

    return `${environment.apiUrl}${relativePath}`;
  }

  openDocument(document: { filePath: string; fileName: string }): void {
    const url = this.getAbsoluteFileUrl(document.filePath);
    this.selectedDocumentUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    this.selectedDocumentName = document.fileName;
  }

  closeDocumentViewer(): void {
    this.selectedDocumentUrl = null;
    this.selectedDocumentName = null;
  }

  getTherapyTypeLabel(name: string): string {
    switch (name) {
      case 'COGNITIVE_BEHAVIORAL_THERAPY':
        return 'Cognitive behavioral therapy';

      case 'PSYCHODYNAMIC_THERAPY':
        return 'Psychodynamic therapy';

      case 'INTERPERSONAL_PSYCHOTHERAPY':
        return 'Interpersonal psychotherapy';

      case 'COGNITIVE_PROCESSING_THERAPY':
        return 'Cognitive processing therapy';

      case 'ANIMAL_ASSISTED_THERAPY':
        return 'Animal-assisted therapy';

      case 'ART_THERAPY':
        return 'Art therapy';

      case 'MUSIC_THERAPY':
        return 'Music therapy';

      case 'GROUP_THERAPY':
        return 'Group therapy';

      case 'FAMILY_THERAPY':
        return 'Family therapy';

      case 'BRAIN_STIMULATION_THERAPY':
        return 'Brain stimulation therapy';

      case 'DIALECTICAL_BEHAVIORAL_THERAPY':
        return 'Dialectical behavioral therapy';

      default:
        return name;
    }
  }
}
