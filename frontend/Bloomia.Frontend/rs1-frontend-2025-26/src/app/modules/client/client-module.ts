import {NgModule} from '@angular/core';

import {ClientRoutingModule} from './client-routing-module';
import {SharedModule} from '../shared/shared-module';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { ClientLayoutComponent } from './client-layout/client-layout.component';
import { CommonModule } from '@angular/common';
import { MySelfTestsComponent } from './my-self-tests/my-self-tests.component';
import { MyJournalsComponent } from './my-journals/my-journals.component';


@NgModule({
  declarations: [
    MyProfileComponent,
    ClientLayoutComponent,
    MySelfTestsComponent,
    MyJournalsComponent
  ],
  imports: [
    SharedModule,
    CommonModule,
    ClientRoutingModule
  ]
})
export class ClientModule { }
