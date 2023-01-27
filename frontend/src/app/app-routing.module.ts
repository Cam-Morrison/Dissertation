import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StocksModule } from './stocks/stocks.module';
import { AdminModule } from './admin/admin.module';
import { UserModule } from './user/user.module';
import { NewsModule } from './news/news.module';

const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full'},
  { path: '**', redirectTo: '/dashboard', pathMatch: 'full'},
]; 

@NgModule({
  imports: [
    UserModule,
    AdminModule,
    NewsModule,
    StocksModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
