import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyDataService {

  private api_key: string = "gnVoGPUtVWs6WuZxrn3sbHcp7RaVn633";
  currentDate = new DatePipe('en-GB').transform(new Date(), 'YYYY-MM-dd');

  constructor(private http: HttpClient){}

  getStockPrice(ticker: string, from: string){
    return this.http.get(`https://api.polygon.io/v2/aggs/ticker/${ticker}/range/1/day/2018-01-01/${this.currentDate}?adjusted=true&sort=asc&apiKey=${this.api_key}`);
  }

  getAllStocks(){
    return this.http.get(`https://api.polygon.io/v2/aggs/grouped/locale/us/market/stocks/2022-08-17?adjusted=true&include_otc=true&apiKey=${this.api_key}`);
  }
}
