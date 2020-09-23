import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { error } from '@angular/compiler/src/util';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valueFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;

  model: any = {};

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      username: new FormControl(),
      password: new FormControl(),
      confirmPassword: new FormControl()

    });
  }

  register(){

    // this.authService.register(this.model).subscribe(() => {
    //  this.alertify.success('registration success!');
    // }, error => {
    //   // this.alertify.error(error);
    //   console.log(error);
    // });
    console.log(this.registerForm.value)
  }

  cancel(){
    this.cancelRegister.emit(false); // can be any object
  }


}
