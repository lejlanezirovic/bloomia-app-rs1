import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TherapistLayoutComponent } from './therapist-layout/therapist-layout.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { DirectChatsComponent } from './direct-chats/direct-chats.component';
import { DirectChatsDetailsComponent } from './direct-chats/direct-chats-details/direct-chats-details.component';

const routes: Routes = [
  {
    path:'',
    component: TherapistLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
      },
      {
        path: 'home', 
        component: HomeComponent
      },
      {
        path: 'profile',
        component: ProfileComponent
      },
       {
        path: 'direct-chats',
        component: DirectChatsComponent
      },
      {
        path:'direct-chats/:clientId/direct-chats-details',
        component:DirectChatsDetailsComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TherapistRoutingModule { }
