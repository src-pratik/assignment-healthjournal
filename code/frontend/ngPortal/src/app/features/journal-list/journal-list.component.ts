import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';

import { PortalService } from 'src/app/core/services/portal.service';
import { JournalDetailsDTO } from 'src/app/shared/models/model';

@Component({
  selector: 'app-journal-list',
  templateUrl: './journal-list.component.html',
  styleUrls: ['./journal-list.component.css']
})
export class JournalListComponent implements OnInit, AfterViewInit {

  dataSource = new MatTableDataSource<JournalDetailsDTO>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.loadDataSource();
  }

  inProgress: boolean = true;
  hasError: boolean = false;

  inProgressAction: boolean = false;

  displayedColumns: string[] = ['name', 'createdBy', 'createdOn', 'actions'];

  mode!: string;

  constructor(private _journalService: PortalService, private _snackBar: MatSnackBar,
    private router: Router, private activatedRoute: ActivatedRoute) {
  }
  ngOnInit(): void {
    this.mode = this.activatedRoute.snapshot.routeConfig?.path!;
  }


  actionAllowSubscribe(): boolean {
    switch (this.mode) {
      case 'journal-list':
        return true;
      default:
        return false;
    }
  }

  actionAllowNew(): boolean {
    switch (this.mode) {
      case 'journal-publications':
        return true;
      default:
        return false;
    }
  }

  actionAllowViewEditions(): boolean {
    switch (this.mode) {
      case 'journal-subscriptions':
        return true;
      case 'journal-publications':
        return true;
      default:
        return false;
    }
  }

  actionAllowUploadEditions(): boolean {
    switch (this.mode) {
      case 'journal-publications':
        return true;
      default:
        return false;
    }
  }

  getTitle(): string {
    switch (this.mode) {

      case 'journal-list':
        return 'Journal Catlog';
      case 'journal-subscriptions':
        return 'My Journal Subscriptions';
      case 'journal-publications':
        return 'Manage My Journal Publications';
      default:
        return 'Journal Catlog';

    }
  }

  loadDataSource() {
    this.hasError = false;
    console.log(this.mode)

    switch (this.mode) {

      case 'journal-list':
        this.loadJournalList();
        break;

      case 'journal-subscriptions':
        this.loadJournalsSubscribed();
        break;

      case 'journal-publications':
        this.loadJournalsPublished();
        break;

      default:
        this.loadJournalList();
        break;
    }


  }

  loadJournalList() {
    setTimeout(() => {
      this.inProgress = true;
      this._journalService.getJournals().pipe(finalize(() => {
        this.inProgress = false;
       
      })).subscribe({
        next: (v) => { this.onGetSucess(v) },
        error: (e) => { this.onGetError(e) },
        complete: () => { this.onGetComplete() }
      });
    }, 1000);
  }

  loadJournalsSubscribed() {
    setTimeout(() => {
      this.inProgress = true;
      this._journalService.getJournalsSubscribed().pipe(finalize(() => {
        this.inProgress = false;
       
      })).subscribe({
        next: (v) => { this.onGetSucess(v) },
        error: (e) => { this.onGetError(e) },
        complete: () => { this.onGetComplete() }
      });
    }, 1000);
  }

  loadJournalsPublished() {
    setTimeout(() => {
      this.inProgress = true;
      this._journalService.getJournalsPublished().pipe(finalize(() => {
        this.inProgress = false;
       
      })).subscribe({
        next: (v) => { this.onGetSucess(v) },
        error: (e) => { this.onGetError(e) },
        complete: () => { this.onGetComplete() }
      });
    }, 1000);
  }

  onGetSucess(v: JournalDetailsDTO[]) {
    console.log(v);
    this.dataSource.data = v;
  }

  onGetError(v: any) {
    this.hasError = true;
  }
  onGetComplete() {

  }

  onViewClick(id: string) {
    let navSource: string = "subscriptions";

    if (this.mode === "journal-publications")
      navSource = "publications"

    this.router.navigate(['/edition-list', { 'id': id, 'navsource': navSource }]);
  }
  onUploadClick(id: string) {
    console.log(id);
    this.router.navigate(['/journal-upload', { 'id': id }]);
  }

  onNewClick() {

    this.router.navigate(['/journal-entry']);
  }

  onSubscribeClick(id: string) {
    this.saveSubcription(id);
  }

  onSaveSubscriptionSucces(v: any) {
    this._snackBar.open("Success : Journal Subscribed.", "X",
      { duration: 5000 });

    this.router.navigate(['/journal-subscriptions',]);
  }

  onSaveSubscriptionError(e: any) {
    this._snackBar.open("Error : Unable to subscribe journal.", "X",
      { duration: 5000 });
  }

  onSaveSubscriptionComplete() {

  }

  saveSubcription(id: string) {
    setTimeout(() => {
      this.inProgressAction = true;
      this._journalService.postSubscription(id).pipe(finalize(() => {
        this.inProgressAction = false;
       
      })).subscribe({
        next: (v) => { this.onSaveSubscriptionSucces(v) },
        error: (e) => { this.onSaveSubscriptionError(e) },
        complete: () => { this.onSaveSubscriptionComplete() }
      });
    }, 1000);
  }

}