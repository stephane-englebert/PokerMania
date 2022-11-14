export interface TournamentsTypes {
  id: number;
  buyIn: number;
  lateRegistrationLevel: number;
  startingStack: number;
  rebuy: boolean;
  rebuyLevel: number;
  levelsDuration: number;
  minPlayers: number;
  maxPlayers: number;
  playersPerTable: number;
  maxPaidPlaces: number;
  gainsSharingNr: number;
}
