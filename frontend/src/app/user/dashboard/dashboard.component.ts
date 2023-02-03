import { MyDataService } from '../../shared/services/data.service';
import { Component, OnInit, AfterViewInit, ViewChild} from "@angular/core";
import { shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog} from '@angular/material/dialog';
import { editNameDialog } from './edit-title.component';
import { AuthGuard } from '../../shared/services/auth.guard';

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"]
})

export class DashboardComponent implements OnInit {
  stocks: any[] = [];
  watchlistTitle: string = "";
  chartTitle: string = "Today's performance";
  isLoading = true;
  loadingError = false;
  private sub: any;
  public username: string | undefined;

  dailyMovement: number = 0;
  dailyMovementPerc: number = 0;
  ticker?: string | null;
  tickerValid: boolean = true;
  dataPoints: any = [];
  selectedChart: any = 'area';
  public portfolioChart = "treemap";
  public portfolioDataPoints: any[] = [];
  public editmode: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public editmodeObs: Observable<boolean> = this.editmode.asObservable();

  constructor(
    private MyDataService: MyDataService, 
    private matSnackBar: MatSnackBar, 
    private auth: AuthGuard,
    public dialog: MatDialog) 
    {
      try{
        var userObj = auth.getDecodedToken();
        this.username = userObj.user;
      }catch(Exception){}
    }

  ngOnInit():void {
    // Read in ticker, get its price datapoints and daily movement
    let resp = this.MyDataService.getWatchList().pipe(shareReplay());
    let date = new Date();
    this.sub =  resp.subscribe((data: any)=> {
      this.watchlistTitle = data["title"];
      this.stocks = data["stocks"];
      var previousCloses = [];
      var currentPrices = [];
      for (let i = 0; i < this.stocks.length; i++) {
        previousCloses.push(this.stocks[i]["regularMarketPreviousClose"])
        currentPrices.push(this.stocks[i]["regularMarketPrice"])
        this.portfolioDataPoints.push([{x: `${this.stocks[i]["symbol"]}`, y: Number(this.stocks[i]["regularMarketChangePercent"].toFixed(2))}]);
      }
      const yesterdayPrices = previousCloses.reduce((accumulator, obj) => {
        return accumulator + Number(obj);
      }, 0).toFixed(2);

      const todaysPrices = currentPrices.reduce((accumulator, obj) => {
        return accumulator + Number(obj);
      }, 0).toFixed(2);

      const todayDate = new Date(); 
      const yesterdayDate = new Date();  
      const todaysDayOfMonth = todayDate.getDate(); 
      yesterdayDate.setDate(todaysDayOfMonth - 1); 

      this.dataPoints.push([
        [new Date(yesterdayDate)],
        [
          Number(yesterdayPrices),
        ]
      ]);
      this.dataPoints.push([
        [new Date(todayDate)],
        [
          Number(todaysPrices),
        ]
      ]);
      this.dailyMovement = ((todaysPrices - yesterdayPrices) / yesterdayPrices) * 100;
    },   
    (error) => {
      this.loadingError = true;
    });  
    this.isLoading = false;
  }

  removeFromWatchlist(ticker: any, index: number) {
    let resp = this.MyDataService.RemoveFromWatchlist(ticker).pipe(
      shareReplay()
    );
    resp.subscribe(
      (response: any) => {
        this.matSnackBar.open(`${response.toString()}`, 'Close', {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
        });
      },
      (error) => {
        this.matSnackBar.open(`${error.error.text.toString()}`, 'Close', {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
        });
      }
    );
    window.location.reload();
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(editNameDialog, {});

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      let newName = result;
    });
  }
}
