import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';
import { editNameDialog } from './dashboard/edit-title.component';
import { ConfirmationDialog } from './login/confirmation-dialog.component';
import { PreferencesPageComponent } from './preferences-page/preferences-page.component';
import { AuthGuard } from '../shared/services/auth.guard';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../shared/services/token.interceptor';
import { RouterModule } from '@angular/router';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NgApexchartsModule } from 'ng-apexcharts';
import { A11yModule } from '@angular/cdk/a11y';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { myChartModule } from '../shared/component/charts/chart.module';
import { OverlayModule } from '@angular/cdk/overlay';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    DashboardComponent,
    LoginComponent,
    ConfirmationDialog,
    editNameDialog,
    PreferencesPageComponent
  ],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    NgApexchartsModule,
    MatTooltipModule,
    myChartModule,
    FormsModule,
    MatDialogModule,
    MatSlideToggleModule,
    MatCardModule,
    MatToolbarModule,
    MatSnackBarModule,
    MatSelectModule,
    MatInputModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    A11yModule,
    OverlayModule,
    MatFormFieldModule,
    MatDividerModule,
    RouterModule.forChild([
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard]
      },
      { 
        path: 'login', 
        component: LoginComponent
      },
      { path: 'assistance',
        loadChildren: () => import('./user.module').then(m => m.UserModule),
        component: PreferencesPageComponent, 
        canActivate: [AuthGuard]
      }
    ])
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class UserModule { }
