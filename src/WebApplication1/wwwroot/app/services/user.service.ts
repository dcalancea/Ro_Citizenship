import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class UserService {
    constructor(private http: Http) { };
    private userUrl = 'api/values/get?id=';

    getUser(dossierId: string): Observable<User> {
        //let user = new User();
        //user.Id = 123;
        //user.DossierNr = "1234";
        //user.FirstName = "John";
        //user.LastName = "Doe";
        //user.OrderNr = "1/2";
        //user.RegisterDate = new Date("1/1/2001");
        //user.ResolutionDate = new Date("1/1/2001");
        //user.Term = new Date("1/1/2001");
        //return user;

        return this.http.get(this.userUrl + dossierId).map((data: Response) => data.json())
            .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
    }
}