import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl} from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private myBackEndService: string = "https://localhost:7299";
  errorMessage: string = "";
  userForm = new FormGroup({
    username: new FormControl(''),
    password: new FormControl('')
  });

  constructor(private HTTP : HttpClient,
    private router: Router) { }

  ngOnInit(): void {
  }

  register(): void {
  }

  login() {
    this.HTTP.post(`${this.myBackEndService}/login/`, 
    {
      userName: this.userForm.get('username')?.value,
      password: this.userForm.get('password')?.value
    }, 
    { 
      responseType: 'text' 
    })
    .subscribe((response) => {
      localStorage.setItem('token', response);
      this.router.navigate(['/stocks'])
    },
    (error: HttpErrorResponse) => {
      var resp = error.error;
      if(error.error[0] == "{")
      {
        resp = JSON.parse(error.error);
        resp = resp["errors"];
        if(resp["UserName"] != null) 
        {
          resp = resp["UserName"];
        }
        else 
        {
          resp = resp["Password"];
        }
      }
      this.errorMessage = resp;

    })
  }

  quickValidation(){
    if(this.userForm.get('username')?.value == null)
    {
      //todo
    }
  }
}
