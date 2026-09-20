import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PublicLayoutComponent } from './public-layout/public-layout.component';
import { SearchProductsComponent } from './search-products/search-products.component';
import { ArticlePageComponent } from './articles/articles-page/article-page/article-page.component';
import { ArticleDetailsComponent } from './articles/article-details/article-details/article-details.component';
import { LandingComponent } from './landing/landing/landing.component';

const routes: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      {
        path: '',
        component: LandingComponent
      },
      {
        path: 'articles',
        component: ArticlePageComponent
      },
      {
        path: 'articles/:id',
        component: ArticleDetailsComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule {}
