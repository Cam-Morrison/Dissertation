import { MyDataService } from '../shared/services/data.service';
import { Component, OnInit, AfterViewInit, Inject} from "@angular/core";
import { shareReplay } from 'rxjs/operators';
import { Observable } from 'rxjs/internal/Observable';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef, MatDialogActions, MatDialogContent} from '@angular/material/dialog';

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"]
})

export class DashboardComponent implements OnInit, AfterViewInit {
  stocks: any[] = [];
  watchlistTitle: string = "";
  public chartTitle: string = "Portfolio movement";
  public isLoading = true;
  loadingError = false;
  private sub: any;

  dailyMovement: number = 0;
  ticker?: string | null;
  tickerValid: boolean = true;
  dataPoints: any = [];
  selectedChart: any = 'area';
  previousCloses: any[] = [];
  currentPrices: any[] = [];
  public portfolioChart = "treemap";
  public portfolioDataPoints: any[] = [];
  public editmode: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public editmodeObs: Observable<boolean> = this.editmode.asObservable();

  constructor(private MyDataService: MyDataService, private matSnackBar: MatSnackBar, public dialog: MatDialog) {}
  
  ngOnInit():void {
    // Read in ticker, get its price datapoints and daily movement
    let resp = this.MyDataService.getWatchList().pipe(shareReplay());
    this.sub =  resp.subscribe((data: any)=> {
      this.watchlistTitle = data["title"];
      this.stocks = data["stocks"];

      for (let i = 0; i < this.stocks.length; i++) {
        this.dailyMovement += this.stocks[i]["regularMarketChangePercent"];
        this.previousCloses.push(this.stocks[i]["regularMarketPreviousClose"])
        this.currentPrices.push(this.stocks[i]["regularMarketPrice"])
        this.portfolioDataPoints.push([{x: `${this.stocks[i]["symbol"]}`, y: Number(this.stocks[i]["regularMarketChangePercent"].toFixed(2))}]);
      }
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

  ngAfterViewInit() {
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(editNameDialog, {
      data: {name: this.watchlistTitle},
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      let newName = result;
    });
  }
}

export interface DialogData {
  animal: string;
  name: string;
}

@Component({
  selector: 'editNameDialog',
  templateUrl: 'edit-title.component.html',
  styleUrls: ["./dashboard.component.scss"]
})
export class editNameDialog {
  constructor(
    public dialogRef: MatDialogRef<editNameDialog>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
