import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Match, MatchPlayer } from '../models/interfaces';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseUrl = 'https://8080-afdafeec333599263cdbaeceone.premiumproject.examly.io/api/admin';

  constructor(private http: HttpClient) { }

  createMatch(teamA: string, teamB: string, date: string): Observable<Match> {
    return this.http.post<Match>(`${this.baseUrl}/matches`, 
      { teamA, teamB, date }, 
      { withCredentials: true }
    );
  }

  addPlayerToMatch(matchId: number, playerId: number): Observable<MatchPlayer> {
    return this.http.post<MatchPlayer>(`${this.baseUrl}/matches/${matchId}/players`, 
      { playerId }, 
      { withCredentials: true }
    );
  }

  bulkAddPlayers(matchId: number, playerIds: number[]): Observable<MatchPlayer[]> {
    return this.http.post<MatchPlayer[]>(`${this.baseUrl}/matches/${matchId}/players/bulk`, 
      { playerIds }, 
      { withCredentials: true }
    );
  }

  updateScore(matchPlayerId: number, score: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/scores`, 
      { matchPlayerId, score }, 
      { withCredentials: true }
    );
  }

  declareResult(contestId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/results/declare`, 
      { contestId }, 
      { withCredentials: true }
    );
  }
}
