import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../shared/services/auth.guard';
import { NewsComponent } from './news.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from '../shared/services/token.interceptor';
import { NgxPictureModule, CLOUDINARY_CONFIG } from 'ngx-picture';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [NewsComponent],
  imports: [
    CommonModule,
    NgxPictureModule.forRoot(CLOUDINARY_CONFIG),
    ClipboardModule, 
    MatTooltipModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatSnackBarModule,
    RouterModule.forChild([
      {
        path: 'news',
        loadChildren: () => import('./news.module').then(m => m.NewsModule),
        component: NewsComponent,
        canActivate: [AuthGuard]
      },
    ]),
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
export class NewsModule { }
