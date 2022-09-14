import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { shareReplay } from 'rxjs/operators';
import { MyDataService } from 'src/app/shared/services/data.service';

@Component({
  selector: 'app-stock-detail',
  templateUrl: './stock-detail.component.html',
  styleUrls: ['./stock-detail.component.css'],
})
export class StockDetailComponent implements OnInit {
  ticker: string | null | undefined;
  tickerValid: boolean = true;
  dataPoints: any = [];
  selectedChart: any = 'area';
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private MyDataService: MyDataService,
    private matSnackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.ticker = this.route.snapshot.paramMap.get('ticker');
    let resp = this.MyDataService.getStockHistory(this.ticker!).pipe(
      shareReplay()
    );
    resp.subscribe(
      (data: any) => {
        if (data['resultsCount'] === 0) {
          this.pageNotFound();
        }
        for (var key in data['results']) {
          var dt = data['results'][key];
          console.log(dt);
          this.dataPoints.push([
            [new Date(dt['t'])],
            [
              Number(dt['o']),
              Number(dt['h']),
              Number(dt['l']),
              Number(dt['c']),
            ],
          ]);
        }
        this.isLoading = false;
      },
      (error) => {
        console.log('error is: ' + error);
        this.onBack();
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

  onBack(): void {
    this.router.navigate(['/stocks']);
  }
}
