import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StocksComponent } from './stocks.component';
import { RouterModule } from '@angular/router';
import { StockDetailComponent } from './stock-detail/stock-detail.component';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [
    StocksComponent,
    StockDetailComponent
  ],
  imports: [
    CommonModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatSortModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {
        path: 'stocks', component: StocksComponent },
      {
        path: 'stocks/:ticker',
        component: StockDetailComponent
      }
    ])

  ],
  bootstrap: [StocksComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class StocksModule { }
