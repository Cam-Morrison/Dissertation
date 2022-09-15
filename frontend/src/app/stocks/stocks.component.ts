import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MyDataService } from '../shared/services/data.service';
import { shareReplay } from 'rxjs/operators';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-stocks',
  templateUrl: './stocks.component.html',
  styleUrls: ['./stocks.component.css'],
})
export class StocksComponent implements OnInit, AfterViewInit {
  dataPoints: any[] = [];
  displayedColumns: string[] = ['Ticker', 'Volume', 'Open', 'Close', 'Change'];
  dataSource: any;
  isLoading = true;

  constructor
  (
    private MyDataService: MyDataService,
    private router: Router,
  )  { }

  ngOnInit() {
    //Read in ticker
    var count = 0;
    let resp = this.MyDataService.getAllStocks().pipe(shareReplay());
    resp.subscribe(
      (data: any) => {
        for (var key in data['results']) {
          var dt = data['results'][key];
          var percentage = Number(this.getPercentageChange(dt['o'], dt['c']));
          var stock: any = {
            T: dt['T'],
            V: dt['v'],
            O: dt['o'],
            C: dt['c'],
            M: percentage,
          };
          this.isLoading = false;
          this.dataPoints.push(stock);
          count++;
          if (count === 100) {
            break;
          }
        }
        this.dataSource = new MatTableDataSource(this.dataPoints);
      },
      (error) => {
        console.log('error is: ' + error);
      }
    );
  }

  getPercentageChange(oldNumber: number, newNumber: number) {
    return (((newNumber - oldNumber) / oldNumber) * 100).toFixed(2);
  }

  ngAfterViewInit() {}

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  onBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
