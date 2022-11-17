import { Injectable } from "@angular/core";

@Injectable()

export class GlobalConst{
    // Principal Host
    HTTP_HOST: string = '/api';  // voir proxy.conf.js

    // Hub PkMania
    HUB_PK: string = '/pkhub';

    // API Endpoints
    API_MEMBERS: string = this.HTTP_HOST + '/Members';
    API_TOKEN: string = this.HTTP_HOST + '/Token';
    API_TOURNAMENTS_LIST: string = this.HTTP_HOST + '/TournamentsList';
    API_TOURNAMENTS_TYPES: string = this.HTTP_HOST + '/TournamentsTypes';
    API_REGISTRATIONS: string = this.HTTP_HOST + '/Registrations';
}
