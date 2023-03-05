import { AfterViewInit, Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { buffer, finalize } from 'rxjs';
import { PortalService } from 'src/app/core/services/portal.service';

@Component({
  selector: 'app-edition-view',
  templateUrl: './edition-view.component.html',
  styleUrls: ['./edition-view.component.css']
})
export class EditionViewComponent implements OnInit, AfterViewInit {

  editionId!: string;
  journalId!: string;
  inProgress: boolean = true;
  hasError: boolean = false;
  pdfdata!: any;
  navigationSource!: string;

  constructor(private _journalService: PortalService, private _snackBar: MatSnackBar,
    private router: Router, private activatedRoute: ActivatedRoute) {

  }
  ngAfterViewInit(): void {

    setTimeout(() => {
      this.loadDataSource();
    }, 1000);

  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe({
      next: paramMap => {
        if (paramMap.has('id') == false) {
          this.router.navigate(['/']);
        }
        if (paramMap.has('journalid') == false) {
          this.router.navigate(['/']);
        }

        this.editionId = paramMap.get('id')!;
        this.journalId = paramMap.get('journalid')!;

        if (paramMap.has('navsource') == true) {
          this.navigationSource = paramMap.get('navsource')!;
        }
      }
    });
  }


  loadDataSource() {
    this.inProgress = true;
    this.hasError = false;
    this._journalService.downloadEdition(this.editionId).pipe(finalize(() => {
      this.inProgress = false;
     
    })).subscribe({
      next: (v) => { this.onGetSucess(v) },
      error: (e) => { this.onGetError(e) },
      complete: () => { this.onGetComplete() }
    });
  }

  onGetSucess(v: any) {
    console.log(v);
    this.pdfdata = v;
  }
  onGetError(v: any) {
    this.hasError = true;
  }
  onGetComplete() {

  }
  onBackClick() {
    this.router.navigate(['/edition-list', { 'id': this.journalId, 'navsource': this.navigationSource }]);
  }

}
