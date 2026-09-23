import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout/admin-layout/admin-layout.component';
import { NotificationLogsComponent } from './admin-layout/notification-logs/notification-logs/notification-logs.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'notification-logs',
        pathMatch: 'full'
      },
      {
        path: 'notification-logs',
        component: NotificationLogsComponent
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