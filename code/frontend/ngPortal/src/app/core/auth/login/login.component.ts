import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { Login, TokenResponse } from '../../../shared/models/model';
import { PortalService } from '../../services/portal.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  inProgress: boolean = false;
  model: Login = { username: "", password: "" };

  constructor(private _journalService: PortalService, private _snackBar: MatSnackBar,
    private router: Router) {
  }
  onLoginClick() {
    this.inProgress = true;
    this._journalService.postLogin(this.model).pipe(finalize(() => {
      this.inProgress = false;
     
    })).subscribe({
      next: (v) => { this.onLoginSuccess(v) },
      error: (e) => { this.onLoginError(e) },
      complete: () => { this.onLoginComplete() }
    });
  }

  onLoginSuccess(v: TokenResponse) {
       localStorage.setItem("jwt", v.token);

    this._snackBar.open("Success : Logged In.", "X",
      { duration: 5000 });

    this.router.navigate(['/journal-list']);
  }
  onLoginError(v: any) {
    console.log(v);
    this._snackBar.open("Error : Login Failed.", "X",
      { duration: 5000 });
  }
  onLoginComplete() {

  }
}
