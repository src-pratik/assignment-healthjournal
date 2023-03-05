import { Component } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-topnavbar',
  templateUrl: './topnavbar.component.html',
  styleUrls: ['./topnavbar.component.css']
})
export class TopnavbarComponent {
  constructor(private jwtHelper: JwtHelperService) { }

  ngOnInit(): void {
  }

  role!: string;

  isRole(input: any) {
    if (input === this.role)
      return true;
    else
      return false
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      if (this.jwtHelper.decodeToken(token).roles) {
        this.role = this.jwtHelper.decodeToken(token).roles
      }
      return true;
    }
    return false;
  }

  logOut = () => {
    this.role = '';
    localStorage.removeItem("jwt");
  }
}
