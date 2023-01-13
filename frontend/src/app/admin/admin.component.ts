import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { shareReplay } from 'rxjs/internal/operators/shareReplay';
import { AuthGuard } from '../shared/services/auth.guard';
import { MyDataService } from '../shared/services/data.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { share } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

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

  constructor(
    private auth: AuthGuard, 
    private myDataService: MyDataService,
    private matSnackBar: MatSnackBar) {
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

  public lock(UserID: number) {
    let resp = this.myDataService.lockAccount(UserID).pipe(shareReplay());
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
