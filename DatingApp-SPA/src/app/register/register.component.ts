import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { error } from '@angular/compiler/src/util';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valueFromHome: any;
  @Output() cancelRegister = new EventEmitter();

  model: any = {};
 
  constructor(private authService: AuthService) { }

  ngOnInit() {
    console.log(this.valueFromHome);
  }

  register(){
    console.log(this.model);
    this.authService.register(this.model).subscribe(() => {
      console.log('register successful');
    }, error => {
      console.log(error);
    });
  }

  cancel(){
    this.cancelRegister.emit(false); // can be any object
  }


}
