import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatSlideToggle, MatSlideToggleChange } from '@angular/material/slide-toggle';
import { MatSnackBar } from '@angular/material/snack-bar';
import { shareReplay } from 'rxjs/internal/operators/shareReplay';
import { MyDataService } from '../../shared/services/data.service';
import { of, timer, concatMap, interval } from 'rxjs';
import { AuthGuard } from '../../shared/services/auth.guard';

@Component({
  selector: 'app-preferences-page',
  templateUrl: './preferences-page.component.html',
  styleUrls: ['./preferences-page.component.scss']
})
export class PreferencesPageComponent implements OnInit {

  public AiAssist: boolean;
  public cooldownMessage: string = "";
  public isDisabled: boolean = false;
  public loadedBool: boolean = false;

  @ViewChild(MatSlideToggle, { static: false }) toggleComp!: MatSlideToggle;


  constructor(private myDataService: MyDataService, private matSnackBar: MatSnackBar, private auth: AuthGuard) {
    var userObj = this.auth.getDecodedToken();
    this.AiAssist = userObj.AIpreference.toLowerCase() === 'true';
    console.log(this.AiAssist);
  }

  ngOnInit(): void {
    this.loadedBool = true;
  }

  public toggle(event: MatSlideToggleChange) {
    let resp = this.myDataService.toggleAIpreference().pipe(shareReplay());
    
    resp.subscribe(
      (response: any) => {
        var token = response.toString();
        localStorage.removeItem('token');
        localStorage.setItem('token', token);
      },
      (error: any) => {
        var token = error.error.text.toString();
        localStorage.removeItem('token');
        localStorage.setItem('token', token);
      }
    );
    this.isDisabled = true;
    this.cooldownMessage = "Please wait 15 seconds before changing preference again.";

    interval(15000).subscribe(n => {
       this.isDisabled = false; 
       this.cooldownMessage = "";
    });
  }
}
