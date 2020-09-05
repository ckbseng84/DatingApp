import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { error } from '@angular/compiler/src/util';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valueFromHome: any;
  @Output() cancelRegister = new EventEmitter();

  model: any = {};
  count = 0;
  constructor() { }

  ngOnInit() {
    console.log(this.valueFromHome);
  }

  register(){

    console.log(this.model);
    console.log(this.count++);
  }

  cancel(){
    this.cancelRegister.emit(false); // can be any object
  }


}
