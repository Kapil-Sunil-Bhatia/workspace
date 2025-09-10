export interface User {
  userId: string;
  email: string;
  isAdmin: boolean;
}

export interface Match {
  matchId: number;
  teamA: string;
  teamB: string;
  date: string;
  matchPlayers?: MatchPlayer[];
  contests?: Contest[];
}

export interface Player {
  playerId: number;
  name: string;
  role: PlayerRole;
  team: string;
}

export enum PlayerRole {
  Batsman = 0,
  Bowler = 1,
  AllRounder = 2,
  WicketKeeper = 3
}

export interface MatchPlayer {
  matchPlayerId: number;
  matchId: number;
  playerId: number;
  score: number;
  player: Player;
}

export interface Contest {
  contestId: number;
  matchId: number;
  createdByUserId: string;
  joinedByUserId?: string;
  status: ContestStatus;
  winnerUserId?: string;
  match: Match;
  createdByUser: User;
  joinedByUser?: User;
  userTeams?: UserTeam[];
}

export enum ContestStatus {
  Pending = 0,
  Ongoing = 1,
  Completed = 2
}

export interface UserTeam {
  userTeamId: number;
  contestId: number;
  userId: string;
  userTeamPlayers: UserTeamPlayer[];
}

export interface UserTeamPlayer {
  userTeamId: number;
  matchPlayerId: number;
  matchPlayer: MatchPlayer;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  isAdmin?: boolean;
}
