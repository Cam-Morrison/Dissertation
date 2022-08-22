import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from "@angular/platform-browser";
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { AppComponent } from "./app.component";
import { NgApexchartsModule } from "ng-apexcharts";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { DashboardComponent } from './dashboard/dashboard.component';
import { NewsComponent } from './news/news.component';
import { OverlayModule } from '@angular/cdk/overlay'
import { CdkMenuModule } from '@angular/cdk/menu';
import { StocksComponent } from './stocks/stocks.component'
import {A11yModule} from '@angular/cdk/a11y';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSortModule } from '@angular/material/sort';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    NewsComponent,
    StocksComponent,
  ],
  imports: [
    BrowserModule,
    CdkMenuModule,
    NgApexchartsModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatTableModule,
    MatSortModule,
    MatInputModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    A11yModule,
    MatDividerModule,
    OverlayModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
