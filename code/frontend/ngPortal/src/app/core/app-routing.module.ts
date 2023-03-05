import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from '../app.component';
import { DashboardComponent } from '../shared/components/dashboard/dashboard.component';
import { EditionListComponent } from '../features/edition-list/edition-list.component';
import { EditionViewComponent } from '../features/edition-view/edition-view.component';
import { JournalEntryComponent } from '../features/journal-entry/journal-entry.component';
import { JournalListComponent } from '../features/journal-list/journal-list.component';
import { JournalUploadComponent } from '../features/journal-upload/journal-upload.component';
import { LoginComponent } from './auth/login/login.component';

//const routes: Routes = [];
const routes: Routes = [
  { path: 'journal-entry', component: JournalEntryComponent },
  { path: 'journal-upload', component: JournalUploadComponent },
  { path: 'edition-list', component: EditionListComponent },
  { path: 'edition-view', component: EditionViewComponent },
  { path: 'journal-list', component: JournalListComponent },
  { path: 'journal-subscriptions', component: JournalListComponent },
  { path: 'journal-publications', component: JournalListComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', component: DashboardComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {


}
