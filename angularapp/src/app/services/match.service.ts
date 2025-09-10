import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Match, MatchPlayer } from '../models/interfaces';

@Injectable({
  providedIn: 'root'
})
export class MatchService {
  private baseUrl = 'https://8080-afdafeec333599263cdbaeceone.premiumproject.examly.io/api/Matches';

  constructor(private http: HttpClient) { }

  getMatches(): Observable<Match[]> {
    return this.http.get<Match[]>(this.baseUrl, { withCredentials: true });
  }

  getMatch(id: number): Observable<Match> {
    return this.http.get<Match>(`${this.baseUrl}/${id}`, { withCredentials: true });
  }

  getMatchPlayers(matchId: number): Observable<MatchPlayer[]> {
    return this.http.get<MatchPlayer[]>(`${this.baseUrl}/${matchId}/players`, { withCredentials: true });
  }
}
