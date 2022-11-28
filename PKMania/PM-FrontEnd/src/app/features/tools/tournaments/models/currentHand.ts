import { Player } from "./player";
import { Card } from "./card";
export interface CurrentHand {
  guid: string;
  tournamentId: number;
  tableNr: number;
  startedOn: string;
  finishedOn: string;
  progress: number;
  pot: number;
  players: Player[];
  seatNrPlayerToPlay: number;
  seatNrButton: number;
  seatNrSmallBlind: number;
  seatNrBigBlind: number;
  cardsPack: Card[];
  flop: Card[];
  turn: Card;
  river: Card;
  handHistory: string;
}
