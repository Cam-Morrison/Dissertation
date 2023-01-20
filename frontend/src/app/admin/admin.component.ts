import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { shareReplay } from 'rxjs/internal/operators/shareReplay';
import { AuthGuard } from '../shared/services/auth.guard';
import { MyDataService } from '../shared/services/data.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import {LiveAnnouncer} from '@angular/cdk/a11y';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
  displayedColumns: string[] = ['Time', 'UserID', 'User', 'Action', 'Status'];
  public username: any;
  dataPoints: any[] = [];
  dataSource: any;
  isLoading: boolean | undefined;
  sortedData: any[] = [];
  respMsg: string | undefined;

  @ViewChild(MatTable, {static: false}) table : any 

  constructor(
    private auth: AuthGuard, 
    private myDataService: MyDataService,
    private matSnackBar: MatSnackBar,
    private _liveAnnouncer: LiveAnnouncer) {
    try {
      var userObj = auth.getDecodedToken();
      this.username = userObj.user;
    } catch (Exception) {}
  }

  ngOnInit(): void {
    let resp = this.myDataService.getAuditLog().pipe(shareReplay());
    resp.subscribe(
      (tasks: any) => {
        for (var i in tasks) {
          var task = tasks[i];
          var entry: any = {
            T: task['TaskTime'],
            UID: task['UserId'],
            UN: task['UserName'],
            AN: task['ActionName'],
            L: task['UserIsAccountLocked'],
          };
          this.dataPoints.push(entry);
        }
        this.dataSource = new MatTableDataSource(this.dataPoints);
        this.isLoading = false;
      },
      (error: string) => {
        console.log('error is: ' + error);
      }
    );
  }

  public lock(element: any) {
    let resp = this.myDataService.lockAccount(Number(element.UID)).pipe(shareReplay());
    resp.subscribe(
      (response: any) => {
        this.respMsg = response;
      },
      (error) => {
        this.respMsg = error.error.text;
      }
    );
    window.location.reload(); 
  }
}
