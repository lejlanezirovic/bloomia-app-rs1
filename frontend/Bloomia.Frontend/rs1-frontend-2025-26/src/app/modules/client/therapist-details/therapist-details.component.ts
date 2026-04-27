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
import { AppointmentsForReviewDto } from '../../../api-services/appointments/appointments-api.models';
import { ToasterService } from '../../../core/services/toaster.service';
import { validateHorizontalPosition } from '@angular/cdk/overlay';

@Component({
  selector: 'app-therapist-details',
  standalone: false,
  templateUrl: './therapist-details.component.html',
  styleUrl: './therapist-details.component.scss',
})
export class TherapistDetailsComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private therapistsApiService = inject(TherapistsApiService);
  private reviewsApiService = inject(ReviewsApiService);
  private appointmentsApiService = inject(AppointmentsApiService);
  private toasterService = inject(ToasterService);
  private fb = inject(FormBuilder);

  therapist: GetTherapistByIdQueryDto | null = null;
  reviews: GetReviewsByTherapistIdQueryDto[] = [];
  appointmentsForReview: AppointmentsForReviewDto[] = [];

  therapistId = 0;
  isLoading = true;
  isSubmitting = false;
  errorMessage: string | null = null;
  reviewsTotalCount = 0;
  returnTo: 'list' | 'saved' = 'list';

  reviewForm = this.fb.group({
    appointmentId: [null as number | null],
    rating: [0, [Validators.required, Validators.min(1), Validators.max(5)]],
    comment: ['']
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
      this.errorMessage = 'Invalid therapist id.';
      this.isLoading = false;
      return;
    }

    this.therapistId = id;
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.errorMessage = null;

    forkJoin({
      therapist: this.therapistsApiService.getById(this.therapistId),
      reviewsResponse: this.reviewsApiService.getByTherapistId(this.therapistId, { page: 1, pageSize: 5 }),
      appointmentsForReview: this.appointmentsApiService.getAppointmentsForReview(this.therapistId)
    }).subscribe({
      next: ({ therapist, reviewsResponse, appointmentsForReview }) => {
        this.therapist = therapist;
        this.reviews = reviewsResponse.items;
        this.reviewsTotalCount = reviewsResponse.totalItems;
        this.appointmentsForReview = appointmentsForReview;
        
        if(this.appointmentsForReview.length === 1) {
          this.reviewForm.patchValue({
            appointmentId: this.appointmentsForReview[0].appointmentId
          });
        }

        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'An error occurred while loading therapist details. Please try again later.';
        this.isLoading = false;
      }
    });
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

  get shouldShowAppointmentSelect(): boolean {
    return this.appointmentsForReview.length > 1;
  }

  selectRating(value: number): void {
    this.reviewForm.patchValue({ rating: value });
  }

  get canReview(): boolean {
    return this.appointmentsForReview.length > 0;
  }

  submitReview(): void {
    if(!this.canReview) {
      this.toasterService.error('You cannot review a therapist you had no appointment with.');
      return;
    }

    if(this.appointmentsForReview.length === 1 && !this.reviewForm.value.appointmentId) {
      this.reviewForm.patchValue({
        appointmentId: this.appointmentsForReview[0].appointmentId
      });
    }

    if(this.reviewForm.invalid || !this.reviewForm.value.appointmentId) {
      this.reviewForm.markAllAsTouched();
      this.toasterService.error('Please choose a rating.');
      return;
    }

    const payload: CreateReviewCommand = {
      appointmentId: this.reviewForm.value.appointmentId!,
      rating: this.reviewForm.value.rating!,
      comment: this.reviewForm.value.comment || null
    };

    this.isSubmitting = true;

    this.reviewsApiService.create(payload).subscribe({
      next: () => {
        this.toasterService.success('Review submitted successfully.');
        this.reviewForm.patchValue({
          rating: 0,
          comment: '',
          appointmentId: null
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

  formatAppointment(date: string): string {
    return new Date(date).toLocaleString('bs-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    })
  }

  goBack(): void {
    if(this.returnTo === 'saved') {
      this.router.navigate(['/client/saved-therapists']);
    } else {
      this.router.navigate(['/client/list-therapists']);
    }
  }

}
