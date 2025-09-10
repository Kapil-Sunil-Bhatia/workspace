import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingComponent } from './components/loading/loading.component';
import { ToastComponent } from './components/toast/toast.component';



@NgModule({
  declarations: [LoadingComponent, ToastComponent],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
