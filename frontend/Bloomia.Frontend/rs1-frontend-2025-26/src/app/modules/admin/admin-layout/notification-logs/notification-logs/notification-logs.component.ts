import { Component, inject, OnInit } from '@angular/core';
import { AppointmentNotificationLogsApiService } from '../../../../../api-services/appointment-notification-logs/appointment-notification-logs-api.service';
import {  AppointmentNotificationStatus, AppointmentNotificationType, ListAppointmentNotificationLogsQueryDto, ListAppointmentNotificationLogsRequest } from '../../../../../api-services/appointment-notification-logs/appointment-notification-logs-api.model';
import { FormBuilder } from '@angular/forms';
import { BaseListPagedComponent } from '../../../../../core/components/base-classes/base-list-paged-component';

@Component({
  selector: 'app-notification-logs',
  standalone: false,
  templateUrl: './notification-logs.component.html',
  styleUrl: './notification-logs.component.scss',
})
export class NotificationLogsComponent extends BaseListPagedComponent<ListAppointmentNotificationLogsQueryDto, ListAppointmentNotificationLogsRequest> implements OnInit {

  private notificationLogsApi = inject(AppointmentNotificationLogsApiService);
  
  displayedColumns: string[] = [
    'client',
    'therapist',
    'recipientEmail',
    'notificationType',
    'status',
    'scheduledAtUtc',
    'sentAtUtc',
    'errorMessage'
  ];

  statuses = [
    { value: AppointmentNotificationStatus.Pending, label: 'Pending' },
    { value: AppointmentNotificationStatus.Sent, label: 'Sent' },
    { value: AppointmentNotificationStatus.Failed, label: 'Failed' }
  ];

   notificationTypes = [
    { value: AppointmentNotificationType.BookingConfirmation, label: 'Confirmation' },
    { value: AppointmentNotificationType.AppointmentReminder, label: 'Reminder' }
  ];

  constructor() {
    super();

    this.request = new ListAppointmentNotificationLogsRequest();

    this.request.paging.page = 1;
    this.request.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.initList();
  }

  protected override loadPagedData(): void {
    this.startLoading();

    this.notificationLogsApi.list(this.request).subscribe({
        next: response => {
          this.handlePageResult(response);
          this.stopLoading();
        },

        error: err => {
          console.error(err);
          this.stopLoading();
        }
      });
  }

   onSearchChange(search: string): void {
    this.request.search = search?.trim() || null;

    this.request.paging.page = 1;

    this.loadPagedData();
  }

  onStatusChange(status: AppointmentNotificationStatus | null): void {
    this.request.status = status;
    this.request.paging.page = 1;

    this.loadPagedData();
  }

  onNotificationTypeChange(type: AppointmentNotificationType | null): void {
    this.request.notificationType = type;
    this.request.paging.page = 1;

    this.loadPagedData();
  }

  getNotificationTypeLabel(type: string): string {
    if (type === 'BookingConfirmation') {
      return 'Confirmation';
    }

    if (type === 'AppointmentReminder') {
      return 'Reminder';
    }

    return type;
  }

}
