import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>,
    next: HttpHandler): Observable<HttpEvent<any>> {

    const idToken = 'Bearer ' + localStorage.getItem("token");
    console.log(req);

    if (idToken) {
      //Content-Type: application/json;charset=utf-8
      const cloned = req.clone({
        // headers: req.headers.set("Authorization",
        //     "Bearer " + idToken)
        headers: req.headers.append('Content-Type', 'application/json').append('Authorization', idToken),
        body: req.body
      });
      return next.handle(cloned);
    }
    else {
      return next.handle(req);
    }
  }
}
