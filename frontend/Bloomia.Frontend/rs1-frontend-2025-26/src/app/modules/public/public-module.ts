import {NgModule} from '@angular/core';

import {PublicRoutingModule} from './public-routing-module';
import {PublicLayoutComponent} from './public-layout/public-layout.component';
import {SearchProductsComponent} from './search-products/search-products.component';
import {SharedModule} from '../shared/shared-module';
import { ArticleDetailsComponent } from './articles/article-details/article-details/article-details.component';
import { ArticlePageComponent } from './articles/articles-page/article-page/article-page.component';
import { LandingComponent } from './landing/landing/landing.component';


@NgModule({
  declarations: [
    PublicLayoutComponent,
    SearchProductsComponent,
    ArticleDetailsComponent,
    ArticlePageComponent,
    LandingComponent
  ],
  imports: [
    SharedModule,
    PublicRoutingModule,
  ]
})
export class PublicModule { }
