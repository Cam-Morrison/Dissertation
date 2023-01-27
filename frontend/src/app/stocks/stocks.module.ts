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
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { NgApexchartsModule } from "ng-apexcharts";
import { MatSnackBarModule } from '@angular/material/snack-bar'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { myChartModule } from '../shared/component/charts/chart.module';
import { MatExpansionModule } from '@angular/material/expansion';
import { AuthGuard } from '../shared/services/auth.guard';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../shared/services/token.interceptor';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NgxHideOnScrollModule } from 'ngx-hide-on-scroll';

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
    myChartModule,
    MatSortModule,
    MatAutocompleteModule,
    MatRippleModule,
    MatExpansionModule,
    MatSnackBarModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    NgApexchartsModule,
    NgxHideOnScrollModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      {
        path: 'stocks',
        loadChildren: () => import('./stocks.module').then(m => m.StocksModule),
        component: StocksComponent,
        canActivate: [AuthGuard]
      },
      { 
        path: 'stocks/:ticker', 
        loadChildren: () => import('./stocks.module').then(m => m.StocksModule),
        component: StockDetailComponent,
        canActivate: [AuthGuard]
      }
    ])
  ],
  bootstrap: [StocksComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ]
})
export class StocksModule { }
