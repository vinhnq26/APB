import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { ThemeSharedModule } from '@abp/ng.theme.shared';

@NgModule({
  declarations: [HomeComponent],
  imports: [SharedModule, HomeRoutingModule, ThemeSharedModule],
})
export class HomeModule { }
