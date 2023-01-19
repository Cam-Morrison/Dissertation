import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthGuard } from './auth.guard';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor (private auth: AuthGuard, private router: Router) {}

  canActivate() 
  {
    try {
      var userObj = this.auth.getDecodedToken();
      let role = userObj.role;
      if(role == "User") 
      {
        this.router.navigate(['dashboard']);
        return false;
      } 
      else {
        return true;
      }
    } catch(Exception) {
      this.router.navigate(['dashboard']);
      return false;
    }
  }
}
