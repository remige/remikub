import { fetcher } from "./../network/fetcher";
import { ICard } from "../model/icard";
import { IGameResult } from "../model/igame-result";
import { userContext } from "../context/user-context";
import { orderBy } from "lodash-es";

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

    public async deleteGame(gameId: string) {
        await fetcher.httpDelete<{}>(`/games/${gameId}`);
    }

    public async board(gameId: string) {
        return await fetcher.httpGet<ICard[][]>(`/games/${gameId}/board`);
    }

    public async currentUser(gameId: string) {
        return await fetcher.httpGet<string>(`/games/${gameId}/current-user`);
    }

    public async users(gameId: string) {
        return await fetcher.httpGet<string[]>(`/games/${gameId}/users`);
    }

    public async hand(gameId: string) {
        return orderBy(await fetcher.httpGet<ICard[]>(`/games/${gameId}/hand/${userContext.userName}`), ["value"]);
    }

    public async drawCard(gameId: string, hand: ICard[]) {
        return await fetcher.httpPut<{}>(`/games/${gameId}/hand/${userContext.userName}/draw-card`, hand);
    }

    public async play(gameId: string, board: ICard[][], hand: ICard[]) {
        return await fetcher.httpPut<{}>(`/games/${gameId}/play/${userContext.userName}`, { board, hand });
    }

    public async autoPlay(gameId: string) {
        return await fetcher.httpPut<{}>(`/games/${gameId}/play/${userContext.userName}/auto`);
    }

    public async addBot(gameId: string) {
        return await fetcher.httpPut<{}>(`/games/${gameId}/users/bot`);
    }
}