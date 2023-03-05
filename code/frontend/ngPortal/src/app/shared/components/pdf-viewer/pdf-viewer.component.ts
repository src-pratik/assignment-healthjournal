
import { Component, Input } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-pdf-viewer',
  templateUrl: './pdf-viewer.component.html',
  styleUrls: ['./pdf-viewer.component.css']
})
export class PdfViewerComponent {
  @Input() pdfSrc!: string ;
 
  constructor(private sanitizer: DomSanitizer) {

  }
  getSafeUrl(): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl('data:application/pdf;base64,' + this.pdfSrc);
  }
}
