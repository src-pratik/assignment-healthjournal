import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalUploadComponent } from './journal-upload.component';

describe('JournalUploadComponent', () => {
  let component: JournalUploadComponent;
  let fixture: ComponentFixture<JournalUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JournalUploadComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JournalUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
