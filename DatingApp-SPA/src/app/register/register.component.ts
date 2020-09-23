import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { error } from '@angular/compiler/src/util';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

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

  constructor(private authService: AuthService,
              private alertify: AlertifyService,
              private fb: FormBuilder) { }

  ngOnInit() {
    this.createRegisterForm();
  }
  createRegisterForm(){
    this.registerForm = this.fb.group({
      username: ['', [
        Validators.required,
        Validators.pattern('^[A-Za-z][A-Za-z0-9]*$'),
      ]],
      password: ['', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(8),
      ]],
      confirmPassword: ['', Validators.required],
      gender: ['male'],
      knownAs: ['', Validators.required],
      dateOfBirth:[null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],

    }, {
      validators: this.passwordMatchValidator
    });
  }
  register(){

    // this.authService.register(this.model).subscribe(() => {
    //  this.alertify.success('registration success!');
    // }, error => {
    //   // this.alertify.error(error);
    //   console.log(error);
    // });
    console.log(this.registerForm.value);
  }

  cancel(){
    this.cancelRegister.emit(false); // can be any object
  }
  passwordMatchValidator(g: FormGroup){
    return g.get('password').value === g.get('confirmPassword').value ? null : {mismatch: true}; // todo not sure what mismatch does
  }

}
