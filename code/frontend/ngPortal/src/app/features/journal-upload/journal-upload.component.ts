import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { finalize, switchMap } from 'rxjs';
import { PortalService } from 'src/app/core/services/portal.service';
import { JournalDTO } from 'src/app/shared/models/model';

@Component({
  selector: 'app-journal-upload',
  templateUrl: './journal-upload.component.html',
  styleUrls: ['./journal-upload.component.css']
})
export class JournalUploadComponent implements OnInit {

  model: JournalUploadDTO = new JournalUploadDTO();
  journal!: JournalDTO;
  inProgress: boolean = false;
  editionId!: string;

  constructor(private _journalService: PortalService, private _snackBar: MatSnackBar,
    private router: Router, private activatedRoute: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe({
      next: paramMap => {
        if (paramMap.has('id') == false) {
          this.router.navigate(['/']);
        }
        this.model.journalId = paramMap.get('id')!;
      }
    });

    this._journalService.getJournal(this.model.journalId).subscribe({
      next: (v) => { this.journal = v },
      error: (e) => { this.router.navigate(['/']); },
    })

  }

  isValidFile(): boolean {

    if (this.model.file === undefined)
      return false;
    if (this.model.file === null)
      return false;

    return true;
  }


  onUploadClick(event: any) {

    // if (this.isValidFile() === false) {
    //   this._snackBar.open("Error : Invalid file or no file selected", "X",
    //     { duration: 5000 });
    //   return;
    // }

    const formData = new FormData();
    formData.append("file", this.model.file!);
    formData.append("journalId", this.model.journalId)
    this.inProgress = true;
    this._journalService.postEdition(formData).pipe(finalize(() => {
      this.inProgress = false;
     
    })).subscribe({
      next: (v) => { this.onUploadSuccess(v) },
      error: (e) => { this.onUploadError(e) },
      complete: () => { this.onUploadComplete() }
    });
  }

  onUploadSuccess(v: string) {
    this.editionId = v;
  }
  onUploadError(e: any) {
    console.log(e);
    let errorMessage: string = e.error.Edition;

    if (errorMessage === undefined)
      errorMessage = e.error.errors[""];

    this._snackBar.open("Error : " + errorMessage, "X",
      { duration: 5000 });
  }
  onUploadComplete() {
    this._snackBar.open("Success : Journal Edition Uploaded.", "X",
      { duration: 5000 });

    this.router.navigate(['/journal-publications']);
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    console.log(file)

    if (!file)
      return;

    let fileSizeInKB = file.size / 1000;
    let allowedSizeInKB = 5120;

    // console.log(fileSizeInKB, allowedSizeInKB);

    if (fileSizeInKB > allowedSizeInKB) {
      this._snackBar.open("Error : Invalid file. File size should be less than 5MB", "X",
        { duration: 5000 });
      return;
    }

    if (file.type !== "application/pdf") {
      this._snackBar.open("Error : Invalid file. File format should be PDF", "X",
        { duration: 5000 });
      return;
    }

    this.model.file = file;


  }
  getFileName() {
    if (this.isValidFile())
      return this.model.file!.name;
    return '';

  }

  onBackClick() {
    this.router.navigate(['/journal-publications']);
  }

}
export class JournalUploadDTO {
  public journalId!: string;
  public file: File | null | undefined;
}

