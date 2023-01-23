import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'confirmation-dialog',
  templateUrl: 'confirmation-dialog.html',
})
export class ConfirmationDialog {
  message: string = 'Are you sure?';
  confirmButtonText = 'Yes';
  cancelButtonText = 'Cancel';
  url = "";
  username = "";
  password = "";
  public errorMsg: string = "";

  public confirmationForm = new FormGroup({
    input: new FormControl('', 
    [
      Validators.required
    ])});

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: any,
    private dialogRef: MatDialogRef<ConfirmationDialog>,
    private HTTP : HttpClient,
    private router: Router,
  ) {
    if (data) {
      this.message = data.message || this.message;
      this.url = data.url || this.url;
      this.username = data.username || this.username;
      this.password = data.password || this.password;
      if (data.buttonText) {
        this.confirmButtonText = data.buttonText.ok || this.confirmButtonText;
        this.cancelButtonText = data.buttonText.cancel || this.cancelButtonText;
      }
    }
  }

  register() {
    if(this.confirmationForm.get('input')?.value == this.password) 
    {
        this.HTTP.post(`${this.url}/register/`, 
        {
          userName: this.username,
          password: this.password,
          confirmPassword: this.confirmationForm.get('input')?.value
        }, 
        { 
          responseType: 'text' 
        })
        .subscribe((response: string) => {
          console.log(response);
          if(response == "Please do not use any rude words.") {
            this.errorMsg = response;
          } else {
            this.dialogRef.close()
            localStorage.setItem('token', response)
            this.router.navigate(['/stocks'])
          }
        }, 
        (error: HttpErrorResponse) => {
          this.errorMsg = error.error
        })
    }
    else
    {
      this.errorMsg = "Passwords do not match"
    }
  }
}
