import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './core/app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './core/material.module';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { JournalEntryComponent } from './features/journal-entry/journal-entry.component';
import { JournalListComponent } from './features/journal-list/journal-list.component';
import { JournalUploadComponent } from './features/journal-upload/journal-upload.component';
import { EditionListComponent } from './features/edition-list/edition-list.component';
import { EditionViewComponent } from './features/edition-view/edition-view.component';
import { PdfViewerComponent } from './shared/components/pdf-viewer/pdf-viewer.component';
import { DashboardComponent } from './shared/components/dashboard/dashboard.component';
import { TopnavbarComponent } from './shared/components/topnavbar/topnavbar.component';
import { LoginComponent } from './core/auth/login/login.component';
import { authInterceptor } from './core/http/token-interceptor.service';
import { JwtModule } from '@auth0/angular-jwt';

@NgModule({
  declarations: [
    AppComponent,
    JournalEntryComponent,
    JournalListComponent,
    JournalUploadComponent,
    EditionListComponent,
    EditionViewComponent,
    PdfViewerComponent,
    DashboardComponent,
    TopnavbarComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        allowedDomains: ["localhost:4200"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [provideHttpClient(
    withInterceptors([authInterceptor]),
  )],
  bootstrap: [AppComponent]
})
export class AppModule { }
