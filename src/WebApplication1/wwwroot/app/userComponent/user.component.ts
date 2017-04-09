import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { User } from '../models/user';

@Component({
    moduleId: module.id,
    selector: 'user-details',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.css']
})

export class UserComponent implements OnInit {

    constructor(private userService: UserService) { }

    user: User;

    ngOnInit() {
        this.userService.getUser('1246/2011')
            .subscribe((user: User) => {
                this.user = user;
            });
    }
}