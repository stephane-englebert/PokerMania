export interface LoggedMember{
    id: number,
    role: string,
    pseudo: string,
    email: string,
    bankroll: number,
    isPlaying: boolean,
    isDisconnected: boolean,
    currentTournament: number
}