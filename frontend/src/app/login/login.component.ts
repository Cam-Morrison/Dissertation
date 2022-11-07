import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ConfirmationDialog } from './confirmation-dialog.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private myBackEndService: string = "https://localhost:7299";
  errorMessage: string = "";
  userForm = new FormGroup({
    username: new FormControl('', 
    [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(16)
    ]),
    password: new FormControl('', 
    [ 
      Validators.required,
      Validators.minLength(6),
      Validators.maxLength(32)
    ])
  });
  matcher = new MyErrorStateMatcher();

  constructor(
    private HTTP : HttpClient,
    private router: Router,
    public dialog: MatDialog
    ) { }

  ngOnInit(): void {
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

  openDialog() {
    const dialogRef = this.dialog.open(ConfirmationDialog,{
      data:{
        message: 'Please confirm your new password.',
        url: this.myBackEndService,
        username: this.userForm.get('username')?.value,
        password: this.userForm.get('password')?.value,
        buttonText: {
          ok: 'Register',
          cancel: 'Cancel'
        }
      }
    });
  }
}

/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    //condition true
    const isSubmitted = form && form.submitted;
    //false
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
