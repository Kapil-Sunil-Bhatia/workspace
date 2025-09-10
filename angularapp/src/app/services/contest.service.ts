import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contest } from '../models/interfaces';

@Injectable({
  providedIn: 'root'
})
export class ContestService {
  private baseUrl = 'https://8080-afdafeec333599263cdbaeceone.premiumproject.examly.io/api/Contests';

  constructor(private http: HttpClient) { }

  getContestsByMatch(matchId: number): Observable<Contest[]> {
    return this.http.get<Contest[]>(`${this.baseUrl}/by-match/${matchId}`, { withCredentials: true });
  }

  createContest(matchId: number): Observable<Contest> {
    return this.http.post<Contest>(this.baseUrl, { matchId }, { withCredentials: true });
  }

  joinContest(contestId: number): Observable<Contest> {
    return this.http.post<Contest>(`${this.baseUrl}/${contestId}/join`, {}, { withCredentials: true });
  }

  getContest(contestId: number): Observable<Contest> {
    return this.http.get<Contest>(`${this.baseUrl}/${contestId}`, { withCredentials: true });
  }
}
