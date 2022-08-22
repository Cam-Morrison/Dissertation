import { StocksComponent } from './stocks/stocks.component';
import { NewsComponent } from './news/news.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'news', component: NewsComponent },
  { path: 'stocks', component: StocksComponent },
 // { path: '**', component: PageNotFoundComponent },  // 404PAGE
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
