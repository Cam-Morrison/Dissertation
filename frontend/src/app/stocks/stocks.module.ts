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
import { StockDetailGuard } from './stock-detail/stock-detail.guard';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { NgApexchartsModule } from "ng-apexcharts";
import { MatSnackBarModule } from '@angular/material/snack-bar'

@NgModule({
  declarations: [
    StocksComponent,
    StockDetailComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatInputModule,
    MatTableModule,
    MatIconModule,
    MatSortModule,
    MatRippleModule,
    MatSnackBarModule,
    NgApexchartsModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {
        path: 'stocks', component: StocksComponent },
      { 
        path: 'stocks/:ticker', 
        canActivate: [StockDetailGuard],
        component: StockDetailComponent
      }
    ])

  ],
  bootstrap: [StocksComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class StocksModule { }
