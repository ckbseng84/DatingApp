import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { error } from '@angular/compiler/src/util';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/user';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valueFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  user: User;
  bsconfig: Partial<BsDatepickerConfig>;
  constructor(private authService: AuthService,
              private alertify: AlertifyService,
              private fb: FormBuilder,
              private router: Router) { }

  ngOnInit() {
    this.bsconfig = {
      containerClass: 'theme-red',
      showWeekNumbers: false,
    };
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
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],

    }, {
      validators: this.passwordMatchValidator
    });
  }
  register(){
    if (this.registerForm.valid){
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Registration successful');
      }, error => {
        console.log('register failed');
        console.log(error);
        this.alertify.error(error);
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        });
      });

    }
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
