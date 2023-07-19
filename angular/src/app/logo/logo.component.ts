import { Component, HostBinding } from '@angular/core';

@Component({
  selector: 'app-logo',
  templateUrl: './logo.component.html',
  styleUrls: ['./logo.component.scss']
})
export class LogoComponent {
  @HostBinding('class.mx-auto')
  marginAuto = true;

  get smallScreen() {
    return window.innerWidth < 992;
  }
}
