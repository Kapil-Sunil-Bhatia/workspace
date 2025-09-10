import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserTeam } from '../models/interfaces';

@Injectable({
  providedIn: 'root'
})
export class UserTeamService {
  private baseUrl = 'https://8080-afdafeec333599263cdbaeceone.premiumproject.examly.io/api/user-teams';

  constructor(private http: HttpClient) { }

  submitTeam(contestId: number, matchPlayerIds: number[]): Observable<UserTeam> {
    return this.http.post<UserTeam>(`${this.baseUrl}/submit`,
      { contestId, matchPlayerIds },
      { withCredentials: true }
    );
  }

  getUserTeamByContest(contestId: number): Observable<UserTeam> {
    return this.http.get<UserTeam>(`${this.baseUrl}/by-contest/${contestId}`, { withCredentials: true });
  }

  getTeamScore(userTeamId: number): Observable<{ score: number }> {
    return this.http.get<{ score: number }>(`${this.baseUrl}/${userTeamId}/score`, { withCredentials: true });
  }
}
