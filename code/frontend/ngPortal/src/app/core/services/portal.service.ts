import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { EditionListDTO, JournalDetailsDTO, JournalDTO, TokenResponse } from 'src/app/shared/models/model';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class PortalService {

  public journalCache: Map<string, JournalDTO> = new Map();

  constructor(private http: HttpClient,) { }

  postLogin(dto: any) {
    return this.http.post<TokenResponse>(`${environment.apiBaseUrl}/api/identity/login`, dto);
  }
  getJournals() {
    return this.http.get<JournalDetailsDTO[]>(`${environment.apiBaseUrl}/api/Journals`);
  }
  getJournalsSubscribed() {
    return this.http.get<JournalDetailsDTO[]>(`${environment.apiBaseUrl}/api/Journals/subscribed`);
  }
  getJournalsPublished() {
    return this.http.get<JournalDetailsDTO[]>(`${environment.apiBaseUrl}/api/Journals/published`);
  }

  getJournal(guid: string) {
    return this.http.get<JournalDTO>(`${environment.apiBaseUrl}/api/Journals/${guid}`);
  }

  postJournal(dto: JournalDTO) {
    return this.http.post<JournalDTO>(`${environment.apiBaseUrl}/api/Journals`, dto);
    // .pipe(
    //   catchError(this.handleError)
    // );
  }

  postEdition(dto: FormData) {
    return this.http.post<string>(`${environment.apiBaseUrl}/api/Editions`, dto);
    // .pipe(
    //   catchError(this.handleError)
    // );
  }

  getEditions(journalId: string) {
    return this.http.get<EditionListDTO[]>(`${environment.apiBaseUrl}/api/Editions/${journalId}`);
  }
  downloadEdition(editionId: string) {
    return this.http.get(`${environment.apiBaseUrl}/api/Editions/${editionId}/download`);
  }

  postSubscription(journalId: string) {
    return this.http.post<string>(`${environment.apiBaseUrl}/api/Subscriptions/${journalId}`, {});
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }
    // Return an observable with a user-facing error message.
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }

}
