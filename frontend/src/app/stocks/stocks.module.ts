import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StocksComponent } from './stocks.component';
import { RouterModule } from '@angular/router';
import { StockDetailComponent } from './stock-detail/stock-detail.component';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';

@NgModule({
  declarations: [
    StocksComponent,
    StockDetailComponent
  ],
  imports: [
    CommonModule,
    MatInputModule,
    MatTableModule,
    RouterModule.forChild([
      {
        path: 'stocks', component: StocksComponent },
      {
        path: 'stocks/:ticker',
        component: StockDetailComponent
      }
    ])

  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class StocksModule { }
