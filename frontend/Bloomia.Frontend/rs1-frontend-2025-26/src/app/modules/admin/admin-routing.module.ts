import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout/admin-layout/admin-layout.component';
import { NotificationLogsComponent } from './admin-layout/notification-logs/notification-logs/notification-logs.component';
import { AnalyticsComponent } from './admin-layout/analytics/analytics/analytics.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'analytics',
        pathMatch: 'full'
      },
      {
        path: 'notification-logs',
        component: NotificationLogsComponent
      },
      {
        path: 'analytics',
        component: AnalyticsComponent
      }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AdminRoutingModule {}