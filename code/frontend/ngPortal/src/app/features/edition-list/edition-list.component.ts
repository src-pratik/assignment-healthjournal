import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { PortalService } from 'src/app/core/services/portal.service';
import { EditionListDTO } from '../../shared/models/model';

@Component({
  selector: 'app-edition-list',
  templateUrl: './edition-list.component.html',
  styleUrls: ['./edition-list.component.css']
})
export class EditionListComponent implements OnInit, AfterViewInit {


  dataSource = new MatTableDataSource<EditionListDTO>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  displayedColumns: string[] = ['createdOn', 'actions'];

  journalId!: string;
  inProgress: boolean = true;
  hasError: boolean = false;

  navigationSource!: string;

  constructor(private _journalService: PortalService, private _snackBar: MatSnackBar,
    private router: Router, private activatedRoute: ActivatedRoute) {

  }
  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe({
      next: paramMap => {
        if (paramMap.has('id') == false) {
          this.router.navigate(['/']);
        }
        this.journalId = paramMap.get('id')!;

        if (paramMap.has('navsource') == true) {
          this.navigationSource = paramMap.get('navsource')!;
        }
      }
    });
  }
  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.loadDataSource();
  }
  getTitle(): string {
    return "Journal Editions"
  }
  loadDataSource() {
    this.hasError = false;
    this.inProgress = true;
    this._journalService.getEditions(this.journalId).pipe(finalize(() => {
      this.inProgress = false;
     
    })).subscribe({
      next: (v) => { this.onGetSucess(v) },
      error: (e) => { this.onGetError(e) },
      complete: () => { this.onGetComplete() }
    });
  }

  onGetSucess(v: EditionListDTO[]) {
    console.log(v);
    this.dataSource.data = v;

  }
  onGetError(v: any) {
    this.hasError = true;
  }
  onGetComplete() {

  }

  onViewClick(id: string) {
    console.log(id);
    this.router.navigate(['/edition-view', { 'id': id, 'journalid': this.journalId, 'navsource': this.navigationSource }]);
  }
  onBackClick() {

    switch (this.navigationSource) {
      case "subscriptions":
        this.router.navigate(['/journal-subscriptions']);
        break;
      case "publications":
        this.router.navigate(['/journal-publications']);
        break;

      default:
        this.router.navigate(['/journal-subscriptions']);
        break;
    }

  }
}
