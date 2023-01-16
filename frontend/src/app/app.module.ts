import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from "@angular/platform-browser";
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { AppComponent } from "./app.component";
import { ChartComponent, NgApexchartsModule } from "ng-apexcharts";
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
import { A11yModule} from '@angular/cdk/a11y';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSortModule } from '@angular/material/sort';
import { MatSnackBarModule } from '@angular/material/snack-bar'
import { MatSelectModule } from '@angular/material/select';
import { myChartModule } from './shared/component/charts/chart.module';
import { MatCardModule} from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './shared/services/auth.guard';
import { AdminComponent } from './admin/admin.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule} from '@angular/material/dialog';
import { ConfirmationDialog } from './login/confirmation-dialog.component';
import { TokenInterceptor } from './shared/services/token.interceptor';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { MatTooltipModule } from '@angular/material/tooltip';
import { editNameDialog } from './dashboard/edit-title.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { PreferencesPageComponent } from './preferences-page/preferences-page.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    NewsComponent,
    LoginComponent,
    AdminComponent,
    ConfirmationDialog,
    editNameDialog,
    PreferencesPageComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    CdkMenuModule,
    NgApexchartsModule,
    MatTooltipModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    myChartModule,
    FormsModule,
    MatDialogModule,
    MatSlideToggleModule,
    MatCardModule,
    MatToolbarModule,
    MatSnackBarModule,
    MatSelectModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    ClipboardModule,
    MatSidenavModule,
    MatButtonModule,
    MatIconModule,
    A11yModule,
    MatFormFieldModule,
    MatDividerModule,
    OverlayModule,
    ReactiveFormsModule,
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    DashboardComponent
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
