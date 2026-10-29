import { Component, inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AppointmentsApiService } from '../../../api-services/appointments/appointments-api.service';
import { ListAppointmentsQueryDto, ListAppointmentsRequest } from '../../../api-services/appointments/appointments-api.models';
import { BaseListPagedComponent } from '../../../core/components/base-classes/base-list-paged-component';
import { SESSION_DURATION_MINUTES } from '../../../core/constants/session.constants';
import { ToasterService } from '../../../core/services/toaster.service';
import { FitConfirmDialogComponent } from '../../shared/components/fit-confirm-dialog/fit-confirm-dialog.component';
import { DialogButton, DialogResult, DialogType } from '../../shared/models/dialog-config.model';

type AppointmentDisplayStatus = 'SCHEDULED' | 'COMPLETED' | 'CANCELLED';

@Component({
  selector: 'app-my-appointments',
  standalone: false,
  templateUrl: './my-appointments.component.html',
  styleUrl: './my-appointments.component.scss',
})
export class MyAppointmentsComponent
  extends BaseListPagedComponent<ListAppointmentsQueryDto, ListAppointmentsRequest>
  implements OnInit {
  private appointmentsApiService = inject(AppointmentsApiService);
  private toasterService = inject(ToasterService);
  private translate = inject(TranslateService);
  private dialog = inject(MatDialog);

  cancellingId: number | null = null;

  constructor() {
    super();
    this.request = new ListAppointmentsRequest();
  }

  ngOnInit(): void {
    this.initList();
  }

  loadPagedData(): void {
    this.startLoading();

    this.appointmentsApiService.getMyAppointments(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        console.error(err);
        this.stopLoading(this.translate.instant('CLIENT_APPOINTMENTS.LOAD_ERROR'));
      }
    });
  }

  // "Completed" is not stored on the backend; it is derived from the start time and session length.
  getDisplayStatus(appointment: ListAppointmentsQueryDto): AppointmentDisplayStatus {
    if (appointment.status === 'Cancelled') {
      return 'CANCELLED';
    }

    const endsAt = new Date(appointment.scheduledAtUtc).getTime() + SESSION_DURATION_MINUTES * 60_000;
    return endsAt <= Date.now() ? 'COMPLETED' : 'SCHEDULED';
  }

  openCancelDialog(appointment: ListAppointmentsQueryDto): void {
    const dialogRef = this.dialog.open(FitConfirmDialogComponent, {
      width: '400px',
      data: {
        type: DialogType.WARNING,
        titleKey: 'CLIENT_APPOINTMENTS.CANCEL_DIALOG_TITLE',
        messageKey: 'CLIENT_APPOINTMENTS.CANCEL_DIALOG_MESSAGE',
        messageParams: {
          therapist: appointment.therapistName ?? '',
          date: appointment.date,
          time: appointment.time.slice(0, 5)
        },
        buttons: [
          { type: DialogButton.NO, color: 'primary' },
          { type: DialogButton.YES, color: 'warn', translationKey: 'CLIENT_APPOINTMENTS.CANCEL_CONFIRM' }
        ]
      }
    });

    dialogRef.afterClosed().subscribe((result: DialogResult | undefined) => {
      if (result?.button === DialogButton.YES) {
        this.cancelAppointment(appointment.appointmentId);
      }
    });
  }

  private cancelAppointment(appointmentId: number): void {
    this.cancellingId = appointmentId;

    this.appointmentsApiService.cancelAppointment(appointmentId).subscribe({
      next: () => {
        this.cancellingId = null;
        this.toasterService.success(this.translate.instant('CLIENT_APPOINTMENTS.CANCEL_SUCCESS'));
        this.loadPagedData();
      },
      error: (err) => {
        console.error(err);
        this.cancellingId = null;
        this.toasterService.error(this.translate.instant('CLIENT_APPOINTMENTS.CANCEL_ERROR'));
        // The appointment may have changed in the meantime (already started or cancelled).
        this.loadPagedData();
      }
    });
  }
}
