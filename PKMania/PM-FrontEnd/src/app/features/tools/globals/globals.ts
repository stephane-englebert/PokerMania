import { Injectable } from "@angular/core";

@Injectable()

export class GlobalConst{
    // Principal Host
    HTTP_HOST: string = '/api';  // voir proxy.conf.js

    // API Endpoints
    API_MEMBERS: string = this.HTTP_HOST + '/Members';
    API_TOKEN: string = this.HTTP_HOST + '/Token';
}