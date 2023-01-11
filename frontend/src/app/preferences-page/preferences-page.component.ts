import { Component, OnInit } from '@angular/core';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { MatSnackBar } from '@angular/material/snack-bar';
import { shareReplay } from 'rxjs/internal/operators/shareReplay';
import { MyDataService } from '../shared/services/data.service';

@Component({
  selector: 'app-preferences-page',
  templateUrl: './preferences-page.component.html',
  styleUrls: ['./preferences-page.component.scss']
})
export class PreferencesPageComponent implements OnInit {

  public AiAssist: any;

  constructor(private myDataService: MyDataService, private matSnackBar: MatSnackBar) { }

  ngOnInit(): void {
  }

  public toggle(event: MatSlideToggleChange) {
    let resp = this.myDataService.toggleAIpreference().pipe(shareReplay());
    resp.subscribe(
      (response: any) => {
        this.matSnackBar.open(`${response.toString()}`, 'Close', {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
        });
        console.log(response);
      },
      (error) => {
        this.matSnackBar.open(`${error.error.text.toString()}`, 'Close', {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
        });
        console.log(error);
      }
    );
  }
}
