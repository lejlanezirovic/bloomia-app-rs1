import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientLayoutComponent } from './client-layout/client-layout.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { MySelfTestsComponent } from './my-self-tests/my-self-tests.component';
import { MyJournalsComponent } from './my-journals/my-journals.component';
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
        }
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientRoutingModule { }
