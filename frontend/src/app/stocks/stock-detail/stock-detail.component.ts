import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { shareReplay } from 'rxjs/operators';
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

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private MyDataService: MyDataService,
    private matSnackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.ticker = this.route.snapshot.paramMap.get('ticker');
    let priceCall = this.MyDataService.getStockHistory(this.ticker!).pipe(
      shareReplay()
    );
    priceCall.subscribe(
      (priceResp: any) => {
        console.log(priceResp)
        if (priceResp['resultsCount'] === 0) {
          this.pageNotFound();
        }
        for (var key in priceResp['items']) {
          var dt = priceResp['items'][key];
          this.dataPoints.push([
            [new Date(dt['date'])],
            [
              Number(dt['open']),
              Number(dt['high']),
              Number(dt['low']),
              Number(dt['close']),
            ],
          ]);
        }
        this.isLoading = false;
      },
      (error) => {
        //If stock doesn't exist go back to stock listings
        console.log('error is: ' + error);
        this.onBack(true);
      });
    var detailsCall = this.MyDataService.getStockDetails(this.ticker!).pipe(
      shareReplay()
    );
    detailsCall.subscribe(
      (detailsResp: any) => {
        var dt = detailsResp['results'];
        this.details = {
          "isMarketOpen": dt['active'],
          "description": dt['description'],
          "companyUrl": dt['homepage_url'],
          "listDate": dt['list_date'],
          "marketCap": dt['market_cap'],
          "name": dt['name'],
          "primaryExchange": dt['primary_exchange'],
          "sic_description": dt['sic_description'],
          "employees": dt['total_employees']
        };    
        this.detailsLoaded = true;
      },
      (error) => {
        //Empty because if stock does not have information on one of these categories it can through an error
      }
    );
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

  onBack(isError: boolean): void {
    if(isError == false) 
    {
      this.router.navigate(['/stocks']);
    }
    else 
    {
      this.router.navigate(['/stocks']).then(() => {
        this.matSnackBar.open(
          'There was an issue loading this stock, please try again later.',
          'Close',
          {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'top',
          }
        );
      });
    }
  }
}
