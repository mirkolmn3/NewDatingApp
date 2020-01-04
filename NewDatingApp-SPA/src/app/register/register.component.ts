import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  // @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(private authService: AuthService) {}

  ngOnInit() {}
  // tslint:disable-next-line: no-trailing-whitespace

  // tslint:disable-next-line: one-line
  register() {
    // console.log(this.model);
    this.authService.register(this.model).subscribe(() => {
      console.log('registration didn\'t fail ;)');
    }, error => {
      console.log(error);
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }
}
