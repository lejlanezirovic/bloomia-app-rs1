import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientLayoutComponent } from './client-layout/client-layout.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { MySelfTestsComponent } from './my-self-tests/my-self-tests.component';
import { MyJournalsComponent } from './my-journals/my-journals.component';
import { ListTherapistsComponent } from './list-therapists/list-therapists.component';
import { SelfTestsComponent } from './self-tests/self-tests.component';
import { SelfTestDetailsComponent } from './self-tests/self-test-details/self-test-details.component';
import { SubmitSelfTestComponent } from './self-tests/submit-self-test/submit-self-test.component';

import { JournalsComponent } from './journals/journals.component';
import { HomeComponent } from './home/home.component';
import { JournalDetailsComponent } from './journals/journal-details/journal-details.component';

const routes: Routes = [
    {
      path:'',
      component:ClientLayoutComponent,
      children:[
        {
          path:'',
          redirectTo:'my-profile',
          pathMatch:'full'
        },
        {
          path:'my-profile',
          component:MyProfileComponent
        },
        {
          path:'my-self-tests',
          component:MySelfTestsComponent
        },
        {
          path:'my-journals',
          component:MyJournalsComponent
        },
        {
          path:'list-therapists',
          component:ListTherapistsComponent
        },
        {
          path:'self-tests',
          component:SelfTestsComponent
        },
        {
          path:'self-tests/self-test-details/:id',
          component:SelfTestDetailsComponent

        },
        {
          path:'self-tests/submit-self-test/:id',
          component:SubmitSelfTestComponent

        },
        {
          path:'journals',
          component:JournalsComponent
        },
        {path:'home',
          component:HomeComponent

        },
        {
          path:'journals/journal-details',
          component:JournalDetailsComponent


import { ListTherapistsComponent } from './list-therapists/list-therapists.component';
import { SelfTestsComponent } from './self-tests/self-tests.component';
import { SelfTestDetailsComponent } from './self-tests/self-test-details/self-test-details.component';
const routes: Routes = [
    {
      path:'',
      component:ClientLayoutComponent,
      children:[
        {
          path:'',
          redirectTo:'my-profile',
          pathMatch:'full'
        },
        {
          path:'my-profile',
          component:MyProfileComponent
        },
        {
          path:'my-self-tests',
          component:MySelfTestsComponent
        },
        {
          path:'my-journals',
          component:MyJournalsComponent
        },
        {
          path:'list-therapists',
          component:ListTherapistsComponent
        },
        {
          path:'self-tests',
          component:SelfTestsComponent
        },
        {
          path:'self-tests/self-test-details/:id',
          component:SelfTestDetailsComponent
        }
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientRoutingModule { }
