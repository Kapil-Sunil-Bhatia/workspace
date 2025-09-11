import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Match, Contest, MatchPlayer } from '../../models/interfaces';
import { MatchService } from '../../services/match.service';
import { ContestService } from '../../services/contest.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-match-detail',
  template: `
    <div class="container mt-4" *ngIf="match">
      <h2>{{match.teamA}} vs {{match.teamB}}</h2>
      <p class="text-muted">{{match.date | date:'short'}}</p>
      
      <div class="row">
        <div class="col-md-8">
          <h4>Player Pool</h4>
          <div class="table-responsive">
            <table class="table table-striped">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Team</th>
                  <th>Role</th>
                  <th>Score</th>
                </tr>
              </thead>
              <tbody>
                <tr *ngFor="let mp of matchPlayers">
                  <td>{{mp.player.name}}</td>
                  <td>{{mp.player.team}}</td>
                  <td>{{getPlayerRoleText(mp.player.role)}}</td>
                  <td>{{mp.score}}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        
        <div class="col-md-4">
          <h4>Contests</h4>
          
          <button class="btn btn-success mb-3" (click)="createContest()" *ngIf="authService.isAuthenticated">
            Create Contest
          </button>
          
          <div class="card mb-2" *ngFor="let contest of contests">
            <div class="card-body">
              <h6>Contest #{{contest.contestId}}</h6>
              <p class="mb-1">
                <small>Creator: {{contest.createdByUser.email}}</small>
              </p>
              <p class="mb-1" *ngIf="contest.joinedByUser">
                <small>Joined by: {{contest.joinedByUser.email}}</small>
              </p>
              <p class="mb-1">
                <span class="badge" [ngClass]="getStatusBadgeClass(contest.status)">
                  {{getContestStatusText(contest.status)}}
                </span>
              </p>
              
              <button class="btn btn-sm btn-primary" 
                      (click)="joinContest(contest.contestId)"
                      *ngIf="canJoinContest(contest)">
                Join Contest
              </button>
              
              <button class="btn btn-sm btn-info" 
                      (click)="viewContest(contest.contestId)"
                      *ngIf="canViewContest(contest)">
                View Contest
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class MatchDetailComponent implements OnInit {
  match: Match | null = null;
  contests: Contest[] = [];
  matchPlayers: MatchPlayer[] = [];
  matchId: number = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private matchService: MatchService,
    private contestService: ContestService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.matchId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadMatch();
    this.loadContests();
    this.loadMatchPlayers();
  }

  loadMatch(): void {
    this.matchService.getMatch(this.matchId).subscribe(match => {
      this.match = match;
    });
  }

  loadContests(): void {
    this.contestService.getContestsByMatch(this.matchId).subscribe(contests => {
      this.contests = contests;
    });
  }

  loadMatchPlayers(): void {
    this.matchService.getMatchPlayers(this.matchId).subscribe(players => {
      this.matchPlayers = players;
    });
  }

  createContest(): void {
    this.contestService.createContest(this.matchId).subscribe(
      contest => {
        this.loadContests();
        this.router.navigate(['/contests', contest.contestId]);
      }
    );
  }

  joinContest(contestId: number): void {
    this.contestService.joinContest(contestId).subscribe(
      () => {
        this.loadContests();
        this.router.navigate(['/contests', contestId]);
      }
    );
  }

  viewContest(contestId: number): void {
    this.router.navigate(['/contests', contestId]);
  }

  canJoinContest(contest: Contest): boolean {
    const currentUser = this.authService.currentUser;
    return !!(currentUser && 
              !contest.joinedByUserId && 
              contest.createdByUserId !== currentUser.userId);
  }

  canViewContest(contest: Contest): boolean {
    const currentUser = this.authService.currentUser;
    return !!(currentUser && 
              (contest.createdByUserId === currentUser.userId || 
               contest.joinedByUserId === currentUser.userId));
  }

  getPlayerRoleText(role: number): string {
    const roles = ['Batsman', 'Bowler', 'All Rounder', 'Wicket Keeper'];
    return roles[role] || 'Unknown';
  }

  getContestStatusText(status: number): string {
    const statuses = ['Pending', 'Ongoing', 'Completed'];
    return statuses[status] || 'Unknown';
  }

  getStatusBadgeClass(status: number): string {
    const classes = ['badge-warning', 'badge-info', 'badge-success'];
    return classes[status] || 'badge-secondary';
  }
}
