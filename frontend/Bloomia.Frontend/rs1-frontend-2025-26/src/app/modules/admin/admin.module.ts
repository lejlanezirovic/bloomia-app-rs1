import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminLayoutComponent } from './admin-layout/admin-layout/admin-layout.component';
import { NotificationLogsComponent } from './admin-layout/notification-logs/notification-logs/notification-logs.component';
import { SharedModule } from '../shared/shared-module';
import { MatMenuModule } from '@angular/material/menu';

@NgModule({
  declarations: [
    AdminLayoutComponent,
    NotificationLogsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    AdminRoutingModule,
    MatMenuModule
  ]
})
export class AdminModule {}