import { Component, inject,OnInit } from '@angular/core';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { ClientsApiService } from '../../../api-services/clients/clients-api.service';
import { GetClientProfileByIdQueryDTO } from '../../../api-services/clients/clients-api.models';
import { Router } from '@angular/router';
import { NotificationsService } from '../../../core/services/notifications.service';
import { NotificationTokenApiService } from '../../../api-services/notifications/notifications-api.service';
import { ToasterService } from '../../../core/services/toaster.service';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit{

  private apiService=inject(ClientsApiService);
  private router=inject(Router);
  client:GetClientProfileByIdQueryDTO|null=null;
  private notificationService=inject(NotificationsService);
  private notificationTokenApiService=inject(NotificationTokenApiService);
  private toaster=inject(ToasterService);
  isLoading=false;
  errorMessage:string|null=null;

  async ngOnInit(): Promise<void> {
    this.getClientBasicInfo();
    const token=await this.notificationService.requestPermissionAndGetToken();
    await this.notificationService.listenForForegroundMessages();
    console.log("registered token:", token);
    this.showJournalReminder();
  }

  getClientBasicInfo(){
    this.isLoading=true;
    this.errorMessage=null;

    this.apiService.getMyProfile().subscribe({
      next:(response)=>{
        this.client=response;
        this.isLoading=false;
      },
      error:(err)=>{
        this.errorMessage="failed to load home page";
      }
    });
  }

  showJournalDetails(){
    this.router.navigate(['client/journals']);
  }
  showJournalReminder(){
    this.notificationTokenApiService.sendJournalReminder().subscribe({
      next:(res)=>{
        console.log("notification sent: ", res)
        this.toaster.success('Test notification sent successfully');
      },
      error:(err)=>{
        this.toaster.error("Failed to sent the notification");
        console.error(err);
      }
    })
  }
}
