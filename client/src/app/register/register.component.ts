import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
  //@Input() uersFromHomeComponent: any; // parent to child
  @Output() cancelRegister = new EventEmitter(); // child to parent 
  model: any = {}

  constructor(private accountServicw: AccountService) {}
  ngOnInit(): void {
  }

  register(){
    this.accountServicw.register(this.model).subscribe({
      next: response=>{
      console.log(response);
      },
      error: error => console.log(error)
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
    }

}
