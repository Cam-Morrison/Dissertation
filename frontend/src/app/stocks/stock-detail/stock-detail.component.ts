import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { shareReplay } from 'rxjs/operators';
import { AuthGuard } from 'src/app/shared/services/auth.guard';
import { MyDataService } from 'src/app/shared/services/data.service';

@Component({
  selector: 'app-stock-detail',
  templateUrl: './stock-detail.component.html',
  styleUrls: ['./stock-detail.component.css'],
})
export class StockDetailComponent implements OnInit {
  ticker?: string | null;
  tickerValid: boolean = true;
  dataPoints: any = [];
  selectedChart: any = 'area';
  isLoading = true;
  detailsLoaded = false;
  panelOpenState = true;
  details?: any;
  metrics: any | undefined;
  prediction: any = [];
  lastPrice?: number;
  AIassistance: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private MyDataService: MyDataService,
    private matSnackBar: MatSnackBar,
    private auth: AuthGuard
  ) {
    var userObj = this.auth.getDecodedToken();
    this.AIassistance = userObj.AIpreference.toLowerCase() === 'true';
  }

  ngOnInit(): void {
    this.ticker = this.route.snapshot.paramMap.get('ticker');
    let call = this.MyDataService.GetDetailsPageContent(this.ticker!).pipe(
      shareReplay()
    );
    call.subscribe(
      (resp: any) => {
        if (resp['error'] != null) {
          this.pageNotFound();
        } else {
        for (var key in resp['items']) {
          var dt = resp['items'][key];
          if(Number(dt["open"]) != 0.00) {
            this.dataPoints.push([
              [new Date(dt['date'])],
              [
                Number(dt['open']),
                Number(dt['high']),
                Number(dt['low']),
                Number(dt['close']),
              ],
            ]);
            this.lastPrice = Number(dt['close']);
          }
        }
        //Company description
        try {
          dt = resp['details']['assetProfile'];
          this.details = {
            companyUrl: dt['website'],
            sector: dt['industry'],
            name: this.ticker,
            country: dt['country'],
            city: dt['city'],
            employees: dt['fullTimeEmployees'],
            description: dt['longBusinessSummary'],
          };
        } catch(Exception) {}

        //Company financial metrics
        try {
          dt = resp['metrics']['defaultKeyStatistics'];
          this.metrics = {
            marketCap: dt['enterpriseValue']['longFmt'],
            marketCapCompressed: dt['enterpriseValue']['fmt'],
            forwardPE: dt['forwardPE']['fmt'],
            profitMargins: dt['profitMargins']['fmt'],
            sharesOutstanding: dt['sharesOutstanding']['fmt'],
            sharesHeldByInsider: dt['heldPercentInsiders']['fmt'],
            sharesHeldByInstituions: dt['heldPercentInstitutions']['fmt'],
            yield: dt['yield']['fmt'],
            LastDividEnd: dt['lastDividendDate']['fmt'],
            lastDividendValue: dt['lastDividendValue']['fmt'],
          };
        } catch(Exception) {
          console.log(Exception)
        }

        this.detailsLoaded = true;

        //Stock price prediction loading in
        this.prediction = resp['Prediction'].replace("[", ""); 
        this.prediction = this.prediction.replace("]", ""); 
        this.prediction = this.prediction.split(',').map(Number);

        //All info loaded
        this.isLoading = false;
      }
      },
      (error: any) => {
        //If stock doesn't exist go back to stock listings
        console.log('error is: ' + error);
        this.pageNotFound();
      }
    );
    // var detailsCall = this.MyDataService.getStockDetails(this.ticker!).pipe(
    //   shareReplay()
    // );
    // detailsCall.subscribe(
    //   (detailsResp: any) => {
    //     console.log(detailsResp);
    //     try {
    //       var dt = detailsResp['assetProfile'];
    //       this.details = {
    //         companyUrl: dt['website'],
    //         sector: dt['industry'],
    //         name: this.ticker,
    //         country: dt['country'],
    //         city: dt['city'],
    //         employees: dt['fullTimeEmployees'],
    //         description: dt['longBusinessSummary'],
    //       };
    //     } catch(Exception) {}
    //     this.detailsLoaded = true;
    //   },
    //   (error) => {
    //     //Empty because if stock does not have information on one of these categories it can through an error
    //   }
    // );
  }

  pageNotFound() {
    this.router.navigate(['/stocks']).then(() => {
      this.matSnackBar.open(
        'We cannot find the stock you are looking for',
        'Close',
        {
          duration: 5000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
        }
      );
    });
  }

  onBack(): void {
    this.router.navigate(['/stocks']);
  }

  addToWatchlist() {
    var resp = this.MyDataService.AddToWatchlist(this.ticker!).pipe(
      shareReplay()
    );
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
