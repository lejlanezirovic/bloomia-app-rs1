import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminLayoutComponent } from './admin-layout/admin-layout/admin-layout.component';
import { NotificationLogsComponent } from './admin-layout/notification-logs/notification-logs/notification-logs.component';
import { SharedModule } from '../shared/shared-module';
import { MatMenuModule } from '@angular/material/menu';
import { AnalyticsComponent } from './admin-layout/analytics/analytics/analytics.component';
import { ArticlesListComponent } from './admin-layout/articles/articles-list/articles-list.component';
import { ArticlesEditComponent } from './admin-layout/articles/articles-edit/articles-edit.component';
import { ArticlesAddComponent } from './admin-layout/articles/articles-add/articles-add.component';

@NgModule({
  declarations: [
    AdminLayoutComponent,
    NotificationLogsComponent,
    AnalyticsComponent,
    ArticlesListComponent,
    ArticlesEditComponent,
    ArticlesAddComponent
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