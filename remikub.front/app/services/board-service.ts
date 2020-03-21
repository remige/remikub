import { fetcher } from "./../network/fetcher";
import { ICard } from "../model/icard";

export class BoardService {

    public async board() {
        return await fetcher.httpGet<ICard[]>("/board");
    }
}