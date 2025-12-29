import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TherapistRoutingModule } from './therapist-routing-module';
import { TherapistLayoutComponent } from './therapist-layout/therapist-layout.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import {SharedModule} from '../shared/shared-module';

@NgModule({
  declarations: [
    TherapistLayoutComponent,
    HomeComponent,
    ProfileComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    TherapistRoutingModule
  ]
})
export class TherapistModule { }
