import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import {
  FormControl,
  FormGroupDirective,
  NgForm,
  Validators,
} from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MyDataService } from '../../shared/services/data.service';
import { shareReplay } from 'rxjs/operators';
import { DashboardComponent } from './dashboard.component';
import { Router } from '@angular/router';

/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  titleFormControl: any;
  matcher: any;
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return !!(
      control &&
      control.invalid &&
      (control.dirty || control.touched || isSubmitted)
    );
  }
}

@Component({
  selector: 'editNameDialog',
  templateUrl: 'edit-title.component.html',
  styleUrls: ["./dashboard.component.scss"]
})
export class editNameDialog {
  public titleFormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(16),
    Validators.minLength(3),
  ]);
  public matcher = new MyErrorStateMatcher();

  constructor(
    private backend: MyDataService,
    private matSnackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) private data: any,
    public dialogRef: MatDialogRef<editNameDialog>,
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  change(): void {
    let newTitle = this.titleFormControl.value;
    let resp = this.backend.updateWatchListTitle(newTitle!).pipe(
      shareReplay()
    );
    resp.subscribe(
      (response: any) => {},
      (error) => {
        var msg = error.error.text.toString();
        if(msg == "Please do not use any rude words.") {
          this.matSnackBar.open(`${msg}`, 'Close', {
            duration: 5000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
          });
        } else {
          window.location.reload();
        }
      }
    );
  }
}



