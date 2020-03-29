import { fetcher } from "./../network/fetcher";
import { ICard } from "../model/icard";
import { IGameResult } from "../model/igame-result";

export class GameService {

    public async games() {
        return await fetcher.httpGet<IGameResult[]>("/games");
    }

    public async createGame(gameName: string) {
        return await fetcher.httpPost<{}>(`/games/${gameName}`);
    }

    public async addUserToGame(userName: string, gameId: string) {
        return await fetcher.httpPut<{}>(`/games/${gameId}/users/${userName}`);
    }

    public async board(gameId: string) {
        return await fetcher.httpGet<ICard[][]>(`/games/${gameId}/board`);
    }

    public async hand(gameId: string, userName: string) {
        return await fetcher.httpGet<ICard[]>(`/games/${gameId}/hand/${userName}`);
    }

    public async drawCard(gameId: string, userName: string) {
        return await fetcher.httpPut<{}>(`/games/${gameId}/hand/${userName}/draw-card`);
    }
}