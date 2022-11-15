export interface TournamentDetails {
  id: number;
  status: string,
  name: string;
  buyIn: number;
  rebuy: boolean;
  rebuyLevel: number;
  prizePool: number
  playersPerTable: number;
  maxPaidPlaces: number;
  registrationsNumber: number;
  minPlayers: number;
  maxPlayers: number;
  startingStack: number;
  levelsDuration: number;
  gainsSharingNr: number;
  startedOn: string;
  finishedOn: string;
  tournamentType: number;
  realPaidPlaces: number;
}
