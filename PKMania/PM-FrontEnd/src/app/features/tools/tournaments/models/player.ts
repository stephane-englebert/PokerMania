import { Card } from "./card";

export interface Player {
  id: number;
  pseudo: string;
  generalRanking: number;
  tableRanking: number;
  eliminated: boolean;
  sittingAtTable: number;
  seatNr: number;
  stack: number;
  moneyInPot: number;
  disconnected: boolean;
  turnToPlay: boolean;
  privateCards: Card[];
  bonusTime: number;
}
