import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import jwt_decode from 'jwt-decode';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private matSnackbar: MatSnackBar
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.loggedIn()) {
      return true;
    } else {
      this.router.navigate(['login']).then(() => {
        this.matSnackbar.open(
          'You must sign in to access your dashboard.',
          'Close',
          {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'top',
          }
        );
      });
      return false;
    }
  }

  public loggedIn() {
    return !!localStorage.getItem('token');
  }

  public getToken() {
    return localStorage.getItem('token')
  }

  public getDecodedToken(): any {
    try {
      var token = this.getToken();
      return jwt_decode(token!);
    } catch(Error) {
      return null;
    }
  }
}
