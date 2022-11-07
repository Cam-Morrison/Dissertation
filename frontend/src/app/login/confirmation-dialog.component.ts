import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

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
  public errorMsg = false;

  public confirmationForm = new FormGroup({
    input: new FormControl('', 
    [
      Validators.required
    ])});

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: any,
    private dialogRef: MatDialogRef<ConfirmationDialog>
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
        this.dialogRef.close(true);
    }
    else
    {
        this.errorMsg = true;
    }
  }
}
