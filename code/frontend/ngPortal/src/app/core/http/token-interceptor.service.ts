import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { catchError, tap, throwError } from "rxjs";

export const authInterceptor: HttpInterceptorFn = (request, next) => {

  const token = localStorage.getItem("jwt");
  if (token) {
    if (request.headers.has('Authorization') === false) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }
  }
  return next(request).pipe(
    catchError(handleError),
    //tap(resp => console.log('response', resp))
  );

}

const handleError = (error: HttpErrorResponse) => {
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