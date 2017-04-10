import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { UserComponent } from './userComponent/user.component';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { UserService } from './services/user.service'

@NgModule({
    imports: [BrowserModule, HttpModule, FormsModule],
    declarations: [AppComponent, UserComponent],
    providers: [UserService],
    bootstrap: [AppComponent]
})
export class AppModule { }
