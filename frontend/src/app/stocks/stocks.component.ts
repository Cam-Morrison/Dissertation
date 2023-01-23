import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MyDataService } from '../shared/services/data.service';
import { debounceTime, distinctUntilChanged, shareReplay, switchMap } from 'rxjs/operators';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-stocks',
  templateUrl: './stocks.component.html',
  styleUrls: ['./stocks.component.scss'],
})
export class StocksComponent implements OnInit, AfterViewInit {
  dataPoints: any[] = [];
  displayedColumns: string[] = ['Ticker', 'PE ratio', 'Price', 'Rating', 'Change'];
  dataSource: any;
  isLoading = true;

  searchForm:FormGroup = new FormGroup({
    search:new FormControl('')
  })
  public stockList:Array<any> = [];

  dropdownVisible = true;

  hideSearchResults() {
    this.dropdownVisible = false;
  }

  constructor
  (
    private MyDataService: MyDataService,
    private router: Router,
  )  { 
    this.searchForm.get('search')?.valueChanges.pipe(
      debounceTime(1000),
      switchMap((v) => this.MyDataService.getStocksBySearch(v)),
    )
    .subscribe((resp: any) => {
      try{
        console.log(resp)
        this.stockList = resp;
      }catch(Exception) {}
    })
  }
  
  ngOnInit() {
    //Read in ticker
    var count = 0;
    let resp = this.MyDataService.getAllMovers().pipe(shareReplay());
    resp.subscribe(
      (movers: any) => {
        console.log(movers);
        for (var key in movers["quotes"]) {
          var item = movers["quotes"][key];
          var stock: any = {
            T: item['shortName'],
            PE: item['trailingPE'],
            P: item['regularMarketPrice'],
            R: item['averageAnalystRating'],
            C: item['currency'],
            S: item['symbol'],
            M: item['regularMarketChangePercent']
          };
          this.dataPoints.push(stock);
        }
        this.dataSource = new MatTableDataSource(this.dataPoints);
        this.isLoading = false;
      },
      (error) => {
        console.log('error is: ' + error);
      }
    );
  }

  ngAfterViewInit() {}

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  onBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
