import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  count = 0;
  constructor() { }

  ngOnInit() {
  }

  register(){

    console.log(this.model);
    console.log(this.count++);
  }

  cancel(){
    console.log('cancel');
  }

}
