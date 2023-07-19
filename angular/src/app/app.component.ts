import { ReplaceableComponentsService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { LogoComponent } from './logo/logo.component';
import { eThemeBasicComponents } from '@abp/ng.theme.basic';
import { LoginComponent } from './Account/Login/login.component';
import { eIdentityComponents } from '@abp/ng.identity';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <abp-dynamic-layout></abp-dynamic-layout>
  `,
})
export class AppComponent implements OnInit {
  constructor(private replaceableComponents: ReplaceableComponentsService) { }

  ngOnInit() {
    this.replaceableComponents.add({
      component: LoginComponent,
      key: eThemeBasicComponents.AccountLayout,
    });
    // this.replaceableComponents.add({
    //   component: LogoComponent,
    //   key: eThemeBasicComponents.Routes,
    // });
  }
}
