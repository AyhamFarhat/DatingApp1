import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent implements OnInit{
  model: any={};
  //loggedIn = false; // not good for security
  //currentUser$: Observable<User | null> = of(null);//because accountService was private

  constructor(public accountService: AccountService) {}

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currentUser$; // because accountService was private
    //this.getCurrentUser();
  }

  // after adding the currentUser$ we do not need it now
  // getCurrentUser(){
  //   this.accountService.currentUser$.subscribe({
  //     next: user => this.loggedIn = !! user, //turns user object to boolean
  //     error : error => console.log(error)
  //   })
  // }

  login(){
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        //this.loggedIn = true;
      },
      error: error => console.log(error)
    })
  }

  logout(){
    this.accountService.logout();
   // this.loggedIn = false;
  }

}
