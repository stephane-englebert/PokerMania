import { Gain } from "./gain";

export interface Tournament {
  id: number;
  status: string;
  startedOn: string;
  finishedOn: string;
  name: string;
  tournamentType: number;
  registrationsNumber: number;
  prizePool: number;
  realPaidPlaces: number;
  gainsSharingNr: number;
  gains: Gain[];
  currentLevel: number;
  currentSmallBlind: number;
  currentBigBlind: number;
  currentAnte: number;
  nextLevel: number;
  timeBeforeNextLevel: number;
  nextSmallBlind: number;
  nextBigBlind: number;
  nextAnte: number;
}
