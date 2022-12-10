import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyDataService 
{
  private myBackEndService: string = "https://localhost:7299";

  constructor(private http: HttpClient){}

  getWatchList() {
    return this.http.get(`${this.myBackEndService}/getWatchlist`);
  }

  getStockHistory(ticker: string)
  {
    return this.http.get(`${this.myBackEndService}/history/${ticker}`);
  }

  getStockDetails(ticker: string)
  {
    return this.http.get(`${this.myBackEndService}/details/${ticker}`);
  }

  getAllMovers()
  {
    return this.http.get(`${this.myBackEndService}/movers`);
  }

  getDailyNews()
  {
    return this.http.get(`${this.myBackEndService}/news/daily`);
  }

  getStocksBySearch(ticker: string) {
    return this.http.get(`${this.myBackEndService}/stocks/search?q=${ticker}`);
  }
}
