import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Match } from '../../models/interfaces';
import { MatchService } from '../../services/match.service';

@Component({
  selector: 'app-match-list',
  template: `
    <div class="container mt-4">
      <h2>Cricket Matches</h2>
      
      <div class="row">
        <div class="col-md-6 mb-3" *ngFor="let match of matches">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">{{match.teamA}} vs {{match.teamB}}</h5>
              <p class="card-text">
                <small class="text-muted">{{match.date | date:'short'}}</small>
              </p>
              <button class="btn btn-primary" (click)="viewMatch(match.matchId)">
                View Match
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class MatchListComponent implements OnInit {
  matches: Match[] = [];

  constructor(
    private matchService: MatchService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadMatches();
  }

  loadMatches(): void {
    this.matchService.getMatches().subscribe(matches => {
      this.matches = matches;
    });
  }

  viewMatch(matchId: number): void {
    this.router.navigate(['/matches', matchId]);
  }
}
