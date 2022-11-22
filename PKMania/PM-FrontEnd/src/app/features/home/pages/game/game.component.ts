import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  nbAllPlayers: number = 0;
  nbPlayers: number = 0;
  menuOption: number = 1;
  trStatus: string = "unknown";
  msgToPlayer: string = "Vous ne pouvez pas rejoindre ce tournoi.";
  seatsPlayersIds: number[] = [0,0,0,0,0,0,0,0,0,0];

  constructor() {
    this.nbAllPlayers = 10;
    this.nbPlayers = 9;
    this.trStatus = "waitingForPlayers";
    this.trStatus = "ongoing";
    this.msgToPlayer = "Le tournoi démarrera lorsque tous les joueurs auront rejoint le lobby.";
  }

  ngOnInit(): void {
  }

  switchMenu(nbMenu: number) {
    this.menuOption = nbMenu;
  }

  isMenuOptionActive(nbOption: number) {
    if (this.menuOption == nbOption) { return "active" } else { return ""; }
  }

  raiseHand() {
    console.log("Raise hand!!!");
  }

  chooseRaiseAmount(amount: number) {
    console.log(amount);
  }

}
