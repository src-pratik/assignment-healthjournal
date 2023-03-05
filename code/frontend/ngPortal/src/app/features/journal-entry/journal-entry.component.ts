import { JsonPipe } from '@angular/common';
import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { finalize, map } from 'rxjs';
import { PortalService } from 'src/app/core/services/portal.service';
import { JournalDTO } from 'src/app/shared/models/model';




@Component({
  selector: 'app-journal-entry',
  templateUrl: './journal-entry.component.html',
  styleUrls: ['./journal-entry.component.css']
})
export class JournalEntryComponent {

  model: JournalDTO = new JournalDTO();
  inProgress: boolean = false;

  constructor(private _journalService: PortalService, private _snackBar: MatSnackBar,
    private router: Router,) {

  }
  onBackClick() {
    this.router.navigate(['/journal-publications']);
  }
  onSubmitClick(event: any) {
    this.inProgress = true;
    this._journalService.postJournal(this.model)
      .pipe(finalize(() => {
        this.inProgress = false;
       
      })).subscribe({
        next: (v) => {

          this.model = v;

          console.log(v)
        },
        error: (e) => {
          let errorMessage: string = e.error.Journal;
          // new Map(e.error).forEach((value: any, key: any) => {
          //   errorMessage += value;
          // });
          this._snackBar.open("Error : " + errorMessage, "X",
            { duration: 5000 });
          console.error(errorMessage)
        },
        complete: () => {
          console.info('complete')
          this._snackBar.open("Journal Created.", "X",
            { duration: 5000 });

          this.router.navigate(['/journal-upload', { 'id': this.model.id }]);
        }
      });
  }
}
