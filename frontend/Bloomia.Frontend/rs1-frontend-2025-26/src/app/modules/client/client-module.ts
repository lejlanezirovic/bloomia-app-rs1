import {NgModule} from '@angular/core';

import {ClientRoutingModule} from './client-routing-module';
import {SharedModule} from '../shared/shared-module';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { ClientLayoutComponent } from './client-layout/client-layout.component';
import { CommonModule } from '@angular/common';
import { MySelfTestsComponent } from './my-self-tests/my-self-tests.component';
import { MyJournalsComponent } from './my-journals/my-journals.component';
import { ListTherapistsComponent } from './list-therapists/list-therapists.component';
import { SelfTestsComponent } from './self-tests/self-tests.component';
import { SelfTestDetailsComponent } from './self-tests/self-test-details/self-test-details.component';
import { SubmitSelfTestComponent } from './self-tests/submit-self-test/submit-self-test.component';

import { JournalsComponent } from './journals/journals.component';
import { HomeComponent } from './home/home.component';
import { JournalDetailsComponent } from './journals/journal-details/journal-details.component';

import { ListTherapistsComponent } from './list-therapists/list-therapists.component';
import { SelfTestsComponent } from './self-tests/self-tests.component';
import { SelfTestDetailsComponent } from './self-tests/self-test-details/self-test-details.component';


@NgModule({
  declarations: [
    MyProfileComponent,
    ClientLayoutComponent,
    MySelfTestsComponent,
    MyJournalsComponent,
    ListTherapistsComponent,
    SelfTestsComponent,
    SelfTestDetailsComponent,

    SubmitSelfTestComponent,
    JournalsComponent,
    HomeComponent,
    JournalDetailsComponent

    SubmitSelfTestComponent

    MyJournalsComponent,
    ListTherapistsComponent,
    SelfTestsComponent,
    SelfTestDetailsComponent
    MyJournalsComponent

  ],
  imports: [
    SharedModule,
    CommonModule,
    ClientRoutingModule
  ]
})
export class ClientModule { }
