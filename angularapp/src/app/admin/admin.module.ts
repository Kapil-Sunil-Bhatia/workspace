import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { AdminMatchesComponent } from './components/admin-matches/admin-matches.component';
import { AdminPlayersComponent } from './components/admin-players/admin-players.component';
import { AdminScoresComponent } from './components/admin-scores/admin-scores.component';
import { AdminResultsComponent } from './components/admin-results/admin-results.component';


@NgModule({
  declarations: [AdminDashboardComponent, AdminMatchesComponent, AdminPlayersComponent, AdminScoresComponent, AdminResultsComponent],
  imports: [
    CommonModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
