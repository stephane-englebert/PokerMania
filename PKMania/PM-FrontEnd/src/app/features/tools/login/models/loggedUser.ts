import { LoggedMember } from "./loggedMember";

export interface LoggedUserModel{
    token: string;
    loggedMember: LoggedMember;
}