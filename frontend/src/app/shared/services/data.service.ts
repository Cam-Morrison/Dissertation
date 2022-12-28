import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyDataService 
{
  private myBackEndService: string = "https://localhost:7299";
  private searchError: string[] = []

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
    if(ticker.length >= 1) {
      return this.http.get(`${this.myBackEndService}/tickers/search/${ticker}`);
    }
    return this.searchError;
  }

  AddToWatchlist(ticker:string) {
    return this.http.get(`${this.myBackEndService}/addToWatchlist/${ticker}`);
  }

  RemoveFromWatchlist(ticker:string) {
    return this.http.get(`${this.myBackEndService}/removeFromWatchlist/${ticker}`)
  }
}
