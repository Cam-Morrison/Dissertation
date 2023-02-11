import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource} from '@angular/material/table';  
import { MatPaginator } from '@angular/material/paginator';
import { shareReplay } from 'rxjs/internal/operators/shareReplay';
import { AuthGuard } from '../shared/services/auth.guard';
import { MyDataService } from '../shared/services/data.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LiveAnnouncer } from '@angular/cdk/a11y';

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
  respMsg: string | undefined;

  @ViewChild(MatPaginator, { static: true })
  paginator!: MatPaginator;

  constructor(
    private auth: AuthGuard,
    private myDataService: MyDataService,
    private matSnackBar: MatSnackBar,
  ) {
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
        this.dataSource = new MatTableDataSource<any>(this.dataPoints.slice().reverse());
        this.dataSource.paginator = this.paginator;
        this.isLoading = false;
      },
      (error: string) => {
        console.log('error is: ' + error);
      }
    );
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  lock(element: any) {
    let resp = this.myDataService
      .lockAccount(Number(element.UID))
      .pipe(shareReplay());
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
