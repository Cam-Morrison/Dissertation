import { MyDataService } from '../../shared/services/data.service';
import { Component, OnInit, AfterViewInit, ViewChild, ChangeDetectorRef} from "@angular/core";
import { delay, shareReplay } from 'rxjs/operators';
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

export class DashboardComponent implements OnInit, AfterViewInit {
  stocks: any[] = [];
  watchlistTitle: string = "";
  chartTitle: string = "Today's performance";
  public isLoading: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  public loadingError: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  username: string | undefined;

  dailyMovement: number = 0;
  dailyMovementPerc: number = 0;
  ticker?: string | null;
  tickerValid: boolean = true;
  dataPoints: any = [];
  selectedChart: any = 'area';
  portfolioDataPoints: any[] = [];
  editmode: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  editmodeObs: Observable<boolean> = this.editmode.asObservable();
  articles: any[] = [];
  articleIdentifiers: number[] = [];

  constructor
  (
    private MyDataService: MyDataService, 
    private matSnackBar: MatSnackBar, 
    private auth: AuthGuard,
    public dialog: MatDialog
  )
  {
    try{
      var userObj = auth.getDecodedToken();
      this.username = userObj.user;
    }catch(Exception){}
  }
    
  ngOnInit():void {
    // Read in ticker, get its price datapoints and daily movement
    let resp = this.MyDataService.getWatchList().subscribe((data: any) => {
      this.watchlistTitle = data["title"];
      this.stocks = data["stocks"];
      let previousCloses = [];
      let currentPrices = [];
      for (let i = 0; i < this.stocks.length; i++) {
        console.log(this.stocks[i])
        previousCloses.push(this.stocks[i]["regularMarketPreviousClose"].toFixed(2))
        currentPrices.push(this.stocks[i]["regularMarketPrice"].toFixed(2))
      }
    },   
    (error) => {
      this.loadingError.next(true);
    });

    let newsCall = this.MyDataService.getReadingList().subscribe((call: any) => {
        call.forEach((article: any) => {
          this.articles.push(JSON.parse(article["article"]));
          this.articleIdentifiers.push(article["ArticleId"]);
        });
    },   
    (error: any) => {
      this.loadingError.next(true);
    });
  }

  ngAfterViewInit(){
    setTimeout(()=>{                         
      this.isLoading.next(false);
    }, 3000);
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

  removeFromReadingList(articleId: number) {
    let resp = this.MyDataService.removeFromReadingList(articleId).pipe(
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

  openDialog(): void {
    const dialogRef = this.dialog.open(editNameDialog, {});

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      let newName = result;
    });
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
