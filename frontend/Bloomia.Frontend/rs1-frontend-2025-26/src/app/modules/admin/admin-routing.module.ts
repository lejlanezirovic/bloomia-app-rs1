import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout/admin-layout/admin-layout.component';
import { NotificationLogsComponent } from './admin-layout/notification-logs/notification-logs/notification-logs.component';
import { AnalyticsComponent } from './admin-layout/analytics/analytics/analytics.component';
import { ArticlesListComponent } from './admin-layout/articles/articles-list/articles-list.component';
import { ArticlesAddComponent } from './admin-layout/articles/articles-add/articles-add.component';
import { ArticlesEditComponent } from './admin-layout/articles/articles-edit/articles-edit.component';

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
      },
      {
        path: 'articles',
        component: ArticlesListComponent
      },
      {
        path: 'articles/add',
        component: ArticlesAddComponent
      },
      {
        path: 'articles/:id/edit',
        component: ArticlesEditComponent
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