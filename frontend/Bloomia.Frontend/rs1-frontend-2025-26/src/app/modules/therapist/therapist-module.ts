import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TherapistRoutingModule } from './therapist-routing-module';
import { TherapistLayoutComponent } from './therapist-layout/therapist-layout.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import {SharedModule} from '../shared/shared-module';
import { DirectChatsComponent } from './direct-chats/direct-chats.component';
import { DirectChatsDetailsComponent } from './direct-chats/direct-chats-details/direct-chats-details.component';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';

@NgModule({
  declarations: [
    TherapistLayoutComponent,
    HomeComponent,
    ProfileComponent,
    DirectChatsComponent,
    DirectChatsDetailsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    TherapistRoutingModule,
    FormsModule,
    MatMenuModule,
    MatButtonModule,
    MatIconModule,
  ]
})
export class TherapistModule { }
