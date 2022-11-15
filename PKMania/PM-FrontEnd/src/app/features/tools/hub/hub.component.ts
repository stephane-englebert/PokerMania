import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { GlobalConst } from '../globals/globals';

@Component({
  selector: 'app-hub',
  templateUrl: './hub.component.html',
  styleUrls: ['./hub.component.css']
})
export class HubComponent implements OnInit {

  _hubConnection: HubConnection;

  constructor(
    private GBconst: GlobalConst,
  ) {
    this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:7122/pkhub').build();
    this._hubConnection.start().then(() => {

    });
  }

  ngOnInit(): void {
  }

}
