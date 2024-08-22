import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent implements OnInit{
  model: any={};
  //loggedIn = false; // not good for security
  //currentUser$: Observable<User | null> = of(null);//because accountService was private

  constructor(public accountService: AccountService, private router:Router, private toastr: ToastrService) {}

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
      next: _ =>{ this.router.navigateByUrl('/members')
      //error: error => this.toastr.error(error.error)
      this.model = {};
      }
    })
  }

  // login(){
  //   this.accountService.login(this.model).subscribe({
  //     next: response => {
  //       this.router.navigateByUrl('/members')
  //       console.log(response);
  //       //this.loggedIn = true;
  //     },
  //     error: error => console.log(error)
  //   })
  // }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/')
   // this.loggedIn = false;
  }

}
