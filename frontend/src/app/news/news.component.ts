import { Component, OnInit } from '@angular/core';
import { MyDataService } from '../shared/services/data.service';
import { shareReplay } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthGuard } from '../shared/services/auth.guard';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss']
})
export class NewsComponent implements OnInit {
  [x: string]: any;
  sub: any;
  dataPoints: any;
  newsList: any = [];
  isLoading: boolean = true;
  public AIassistance = true;

  constructor(private MyDataService: MyDataService, private matSnackBar: MatSnackBar, private auth: AuthGuard) { }

  ngOnInit():void {
    let resp = this.MyDataService.getDailyNews().pipe(shareReplay());
    this.sub =  resp.subscribe((data: any)=> {
        for (var key in data) {
          //Gets rid of repetitive photo of bloomberg logo
          if(data[key]["source"] == "Bloomberg")
          {
            data[key]["image"] = null
          }
          this.newsList.push(data[key])
        }
        var userObj = this.auth.getDecodedToken();
        this.AIassistance = userObj.AIpreference.toLowerCase() === 'true';
        console.log(this.AIassistance);
    },   
    (error: any) => {});  
    this.isLoading = false;
  }

  linkCoppied() {
    this.matSnackBar.open(
      'Link copied to clipboard',
      'Close',
      {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
      }
    );
  }
}
