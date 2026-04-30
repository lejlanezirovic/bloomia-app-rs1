import { Component, inject, OnInit } from '@angular/core';
import { TherapistsApiService } from '../../../api-services/therapists/therapists-api.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { GetTherapistByIdQueryDto, TherapistAvailabilityDto } from '../../../api-services/therapists/therapists-api.models';
import { ListMyWorkingDatesAndTimesResponse, WorkingTimeSlotsDto } from '../../../api-services/therapistAvailability/therapistAvailability-api.models';
import { forkJoin } from 'rxjs';
import { TherapistAvailabilityApiService } from '../../../api-services/therapistAvailability/therapistAvailability-api.service';
import { BaseComponent } from '../../../core/components/base-classes/base-component';

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

  therapist: GetTherapistByIdQueryDto | null = null;
  workingTimes: ListMyWorkingDatesAndTimesResponse | null = null;


  currentMonth = new Date();
  selectedDateKey: string | null = null;
  
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
      workingTimes: this.therapistAvailabilityApi.listMyWorkingDatesAndTimes()
    }).subscribe({
      next: ({ profile, workingTimes }) => {
        this.therapist = profile;
        this.workingTimes = workingTimes;

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

  get specializations(): string {
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
    const input = event.target as HTMLInputElement;

    if (input.files && input.files[0]) {
      const file = input.files[0];
      console.log('Uploading document:', file);
      // TODO: upload dokumenta
    }
  }
}

