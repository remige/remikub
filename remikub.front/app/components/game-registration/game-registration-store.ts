import { observable, computed, action } from "mobx";
import { IGameResult } from "../../model/igame-result";

export class GameRegistrationStore {

    @observable private _games: IGameResult[] = [];
    @computed public get games() { return this._games; }
    @action public setGames(games: IGameResult[]) { this._games = games; }

    @observable private _newGame: string = "";
    @computed public get newGame() { return this._newGame; }
    @action public setNewGame(newGame: string = "") { this._newGame = newGame; }
}